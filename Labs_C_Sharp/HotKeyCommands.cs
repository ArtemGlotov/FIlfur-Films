using System.Windows.Input;

namespace Labs_C_Sharp
{
    public static class HotKeyCommands
    {
        static HotKeyCommands()
        {
            Save = new RoutedCommand("Save", typeof(MainWindow));
            Open = new RoutedCommand("Open", typeof(MainWindow));
            Close = new RoutedCommand("Close", typeof(MainWindow));
            NextPage = new RoutedCommand("NextPage", typeof(MainWindow));
            PreviousPage = new RoutedCommand("PreviousPage", typeof(MainWindow));
        }

        public static RoutedCommand Save { get; set; }
        public static RoutedCommand Open { get; set; }
        public static RoutedCommand Close { get; set; }
        public static RoutedCommand NextPage { get; set; }
        public static RoutedCommand PreviousPage { get; set; }
    }
}
