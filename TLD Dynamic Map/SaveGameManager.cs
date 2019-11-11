using System.Collections.Generic;

namespace TLD_Dynamic_Map.Helpers
{
    public static class SaveGameManager
    {
        public class SaveInfo
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        public static List<SaveInfo> Saves { get; set; }
    }
}
