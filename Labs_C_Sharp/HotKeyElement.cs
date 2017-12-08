using System.Configuration;

namespace Labs_C_Sharp
{
    public class HotKeyElement : ConfigurationElement
    {
        [ConfigurationProperty("Command")]
        public string Command
        {
            get { return (string)base["Command"]; }
            set { base["Command"] = value; }
        }

        [ConfigurationProperty("Gesture")]
        public string Gesture
        {
            get { return (string)base["Gesture"]; }
            set { base["Gesture"] = value; }
        }
    }
}
