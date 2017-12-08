using System.Configuration;

namespace Labs_C_Sharp
{
    [ConfigurationCollection(typeof(HotKeyElement), AddItemName = "HotKey")]
    public class HotKeysCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HotKeyElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HotKeyElement)element).Command;
        }
    }
}
