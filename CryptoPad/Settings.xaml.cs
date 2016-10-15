using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace CryptoPad
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            pathTextBox.Text = Properties.Settings.Default.PathToFile;
            passwordBox1.BorderThickness = new Thickness(3, 3, 3, 3);
            passwordBox2.BorderThickness = new Thickness(3, 3, 3, 3);
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = (new FileInfo(Properties.Settings.Default.PathToFile)).DirectoryName;
            if (sfd.ShowDialog() ?? false)
            {
                pathTextBox.Text = sfd.FileName;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.PathToFile = pathTextBox.Text;
                Properties.Settings.Default.Save();

                if (passwordBox1.Password.Length > 5 && passwordBox1.Password == passwordBox2.Password)
                {
                    SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
                    MainWindow.NotSecureKey = sha.ComputeHash(Encoding.Unicode.GetBytes(passwordBox1.Password));
                }
                else if (passwordBox1.Password.Length > 0 || passwordBox2.Password.Length > 0 || passwordBox1.Password == passwordBox2.Password)
                {
                    passwordBox1.BorderBrush = Brushes.Red;
                    passwordBox2.BorderBrush = Brushes.Red;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox1.BorderBrush = Brushes.Azure;
            passwordBox2.BorderBrush = Brushes.Azure;
        }

        private void passwordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox1.BorderBrush = Brushes.Azure;
            passwordBox2.BorderBrush = Brushes.Azure;
        }
    }
}
