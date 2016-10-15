using System;
using System.Collections.Generic;
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
using System.Security.Cryptography;
using System.IO;
using System.Security;

namespace CryptoPad
{

    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class MyLogin : Window
    {
        public MyLogin()
        {
            InitializeComponent();
            passwordBox1.BorderThickness = new Thickness(3,3,3,3);
            passwordBox1.Focus();
        }
        public bool LoginSuccess;
        public SecureString password;
        public byte[] NotSecurePassword;

        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string pathToFile = Properties.Settings.Default.PathToFile;
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            if (e.Key == Key.Enter)
            {
                SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
                aes.Key = sha.ComputeHash(Encoding.Unicode.GetBytes(passwordBox1.Password));
                try
                {
                    if (MainWindow.VerifyFile(aes.Key, pathToFile))
                    {
                        LoginSuccess = true;
                        NotSecurePassword = aes.Key;
                        Close();
                    }
                }
                catch (FileNotFoundException fnfe)
                {
                    MessageBoxResult mbr = MessageBox.Show("Не найден файл. Создать новый?", "Что-то пошло не так", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (mbr)
                    {
                        case (MessageBoxResult.Yes):
                            NotSecurePassword = aes.Key;
                            using (FileStream fs = new FileStream(pathToFile, FileMode.CreateNew))
                            {
                                byte[] encrypted = MainWindow.EncryptStringToBytes_Aes("check", NotSecurePassword, new byte[16]);
                                fs.Write(encrypted, 0, encrypted.Length);
                            }
                            MainWindow.SignFile(NotSecurePassword, pathToFile);
                            LoginSuccess = true;
                            Close();
                            break;
                        case (MessageBoxResult.No):
                            aes.Dispose();
                            GC.Collect();
                            Close();
                            break;
                        case (MessageBoxResult.Cancel):
                            return;

                    }
                }
                catch (CryptographicException ce)
                {
                    passwordBox1.BorderBrush = Brushes.Red;
                }

            }
            if (e.Key == Key.Escape)
            {
                LoginSuccess = false;
                Close();
            }
        }

        private void passwordBox1_TextInput(object sender, TextCompositionEventArgs e)
        {
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox1.BorderBrush = Brushes.Azure;
        }
    }
}
