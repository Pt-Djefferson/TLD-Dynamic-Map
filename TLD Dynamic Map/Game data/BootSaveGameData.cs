using TLD_Dynamic_Map.Helpers;

namespace TLD_Dynamic_Map.Game_data
{
    public class BootSaveGameFormat
    {
        public EnumWrapper<RegionsWithMap> m_SceneName { get; set; }
        public int m_Version { get; set; }
    }
}
