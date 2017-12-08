using System.Configuration;

namespace Labs_C_Sharp
{
    public class HotKeySection : ConfigurationSection
    {
        [ConfigurationProperty("HotKeys")]
        public HotKeysCollection HotKeys
        {
            get { return (HotKeysCollection)base["HotKeys"]; }
            set { base["HotKeys"] = value; }
        }
    }
}
