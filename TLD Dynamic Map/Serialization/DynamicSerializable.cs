﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLD_Dynamic_Map.Helpers
{
    public class DynamicSerializable<T> where T : new()
    {

        private Dictionary<object, List<KeyValuePair<string, JToken>>> extraFields = new Dictionary<object, List<KeyValuePair<string, JToken>>>();
        public T Obj { get; private set; }

        public DynamicSerializable(string json)
        {
            Obj = Parse(JObject.Parse(json), typeof(T));
        }

        private dynamic Parse(JToken token, Type t, DeserializeAttribute attr = null)
        {
            bool deserialize = attr?.Json ?? false;
            bool deserializeItems = attr?.JsonItems ?? false;

            if (token.Type == JTokenType.Object)
            {
                if (typeof(IDictionary).IsAssignableFrom(t))
                    return ParseDictionary((JObject)token, t, deserializeItems);
                return ParseObject((JObject)token, t);
            }
            else if (token.Type == JTokenType.Array)
            {
                return ParseArray((JArray)token, t, deserializeItems);
            }
            else if (token.Type == JTokenType.Boolean || token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
            {
                return token.ToObject(t);
            }
            else if (token.Type == JTokenType.String)
            {
                string s = token.ToObject<string>();
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EnumWrapper<>))
                {
                    return Activator.CreateInstance(t, s);
                }
                else if (t == typeof(DateTime))
                {
                    return DateTime.Parse(s);
                }
                if (!deserialize)
                    return s;
                return Parse(JToken.Parse(s), t);
            }
            else if (token.Type == JTokenType.Null)
            {
                return null;
            }
            else if (token.Type == JTokenType.Date)
            {
                return token.Value<DateTime>();
            }
            else
            {
                throw new Exception("Invalid token type " + token.Type);
            }
        }

        private object ParseObject(JObject obj, Type t)
        {
            var result = Activator.CreateInstance(t);
            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                 .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null)
                .ToDictionary(p => MemberToName(p));
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public)
                 .Where(f => f.GetCustomAttribute<JsonIgnoreAttribute>() == null)
                .ToDictionary(p => MemberToName(p));

            foreach (var child in obj)
            {
                if (props.ContainsKey(child.Key))
                {
                    var prop = props[child.Key];
                    var attr = prop.GetCustomAttribute<DeserializeAttribute>();
                    var childType = prop.PropertyType;
                    var childVal = Parse(child.Value, childType, attr);
                    prop.SetValue(result, childVal);
                }
                else if (fields.ContainsKey(child.Key))
                {
                    var field = fields[child.Key];
                    var attr = field.GetCustomAttribute<DeserializeAttribute>();
                    var childType = field.FieldType;
                    var childVal = Parse(child.Value, childType, attr);
                    field.SetValue(result, childVal);
                }
                else
                {
                    if (!extraFields.ContainsKey(result))
                        extraFields.Add(result, new List<KeyValuePair<string, JToken>>());
                    extraFields[result].Add(child);
                }
            }
            return result;
        }

        private object ParseArray(JArray obj, Type t, bool deserializeItems)
        {
            Type elemType = t.GetElementType();
            if (!t.IsArray)
                elemType = t.GetGenericArguments().Single();
            Array result = Array.CreateInstance(elemType, obj.Count);
            int i = 0;
            foreach (var child in obj)
            {
                result.SetValue(Parse(child, elemType, new DeserializeAttribute(null, deserializeItems)), i++);
            }
            return ReflectionUtil.ConvertArray(result, t);
        }

        private object ParseDictionary(JObject obj, Type t, bool deserializeItems)
        {
            var dict = (IDictionary)Activator.CreateInstance(t);
            var keyType = t.GetGenericArguments()[0];
            var valType = t.GetGenericArguments()[1];
            foreach (var child in obj)
            {
                dict.Add(Parse(child.Key, keyType), Parse(child.Value, valType, new DeserializeAttribute(null, deserializeItems)));
            }
            return dict;
        }

        public string Serialize()
        {
            var res = ReconstructObject(Obj);
            return JsonConvert.SerializeObject(res);
        }

        public object Reconstruct(object o, DeserializeAttribute attr = null)
        {
            object result = null;
            if (o == null)
                result = null;
            else
            {
                Type t = o.GetType();
                if (ReflectionUtil.IsBoxed(o) || o is string)
                {
                    result = o;
                }
                else if (o is IDictionary)
                {
                    result = ReconstructDictionary((IDictionary)o, attr?.JsonItems ?? false);
                }
                else if (o is ICollection || ReflectionUtil.ImplementsGenericInterface(o.GetType(), typeof(ICollection<>)))
                {
                    result = ReconstructCollection(o, attr?.JsonItems ?? false);
                }
                else if (o.GetType().IsGenericType && o.GetType().GetGenericTypeDefinition() == typeof(EnumWrapper<>))
                {
                    return o.ToString();
                }
                else
                {
                    result = ReconstructObject(o);
                }
            }

            if (attr != null && result != null && attr.Json)
                return JsonConvert.SerializeObject(result);
            return result;
        }

        public object ReconstructObject(object o)
        {
            var res = new Dictionary<string, dynamic>();
            var t = o.GetType();
            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null).ToArray();
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null).ToArray();

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<DeserializeAttribute>();
                var name = attr?.From ?? prop.Name;
                res.Add(name, Reconstruct(prop.GetValue(o), attr));
            }
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<DeserializeAttribute>();
                var name = attr?.From ?? field.Name;
                res.Add(name, Reconstruct(field.GetValue(o), attr));
            }
            if (extraFields.ContainsKey(o))
            {
                foreach (var kvp in extraFields[o])
                {
                    res.Add(kvp.Key, kvp.Value);
                }
            }
            return res;
        }

        public List<object> ReconstructCollection(dynamic col, bool serializeItems)
        {
            List<object> result = new List<object>();
            foreach (var item in col)
            {
                result.Add(Reconstruct(item, new DeserializeAttribute(null, serializeItems)));
            }
            return result;
        }

        public IDictionary ReconstructDictionary(IDictionary dict, bool serializeItems)
        {
            var res = new Dictionary<object, object>();
            foreach (var key in dict.Keys)
            {
                res.Add(Reconstruct(key), Reconstruct(dict[key], new DeserializeAttribute(null, serializeItems)));
            }
            return res;
        }

        private string MemberToName(MemberInfo m)
        {
            var attr = m.GetCustomAttribute<DeserializeAttribute>();
            if (attr != null)
                return attr.From;
            return m.Name;
        }

    }
}
