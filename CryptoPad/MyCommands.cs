using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoPad
{
    public class MyCommands
    {

        static MyCommands()
        {
            SettingsCommand = new RoutedUICommand("Settings", "SettingsCommand", typeof(MainWindow));
            MessagesCommand = new RoutedUICommand("MessagesCommand", "MessagesCommand", typeof(MainWindow));
            AddPostCommand = new RoutedUICommand("AddPostCommand", "AddPostCommand", typeof(MainWindow));
        }

        public static RoutedUICommand SettingsCommand { get; set; }
        public static RoutedUICommand MessagesCommand { get; set; }
        public static RoutedUICommand AddPostCommand { get; set; }
    }
}
