using System.Text;

namespace TLD_Dynamic_Map.Helpers
{
    public class EncryptString
    {
        public static byte[] Compress(string json)
        {
            return CLZF.Compress(Encoding.UTF8.GetBytes(json));
        }

        public static string Decompress(byte[] bytes)
        {
            return Encoding.UTF8.GetString(CLZF.Decompress(bytes));
        }
    }
}
