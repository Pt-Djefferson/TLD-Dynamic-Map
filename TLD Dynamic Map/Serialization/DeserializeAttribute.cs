using System;

namespace TLD_Dynamic_Map.Helpers
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DeserializeAttribute : Attribute
    {
        public string From { get; private set; }
        public bool Json { get; private set; }
        public bool JsonItems { get; private set; }

        public DeserializeAttribute(string from, bool json = false, bool jsonItems = false)
        {
            From = from;
            Json = json;
            JsonItems = jsonItems;
        }
    }

}
