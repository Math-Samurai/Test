using System.Windows.Input;

namespace GUI
{
    public static class Commands
    {
        public static RoutedCommand AddDataItem = new RoutedCommand("AddDataItem", typeof(GUI.Commands));
    }
}
