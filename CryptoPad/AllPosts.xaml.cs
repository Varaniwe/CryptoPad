using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace CryptoPad
{

    /// <summary>
    /// Логика взаимодействия для AllPosts.xaml
    /// </summary>
    public partial class AllPosts : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Post> m_postsList;
        public AllPosts()
        {
            InitializeComponent();
            allPostsListView.DataContext = this;

            KeyBinding keyBinding = new KeyBinding(ApplicationCommands.Close, Key.Escape, ModifierKeys.None);
            this.InputBindings.Add(keyBinding);
            this.InputBindings.Add(new KeyBinding(MyCommands.AddPostCommand, Key.Enter, ModifierKeys.Alt));

            CommandBinding commBinding = new CommandBinding(ApplicationCommands.Close, CloseCommandHandler, CloseCommand_CanExecuted);
            this.CommandBindings.Add(commBinding);

            BinaryFormatter bf = new BinaryFormatter();
            List<Post> allPostsBinary;
            using (
                FileStream fs = new FileStream(Properties.Settings.Default.PathToFile + "_allposts",
                    FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {

                    byte[] allPostsData = MainWindow.DecryptBytesFromBytes_Aes(br.ReadBytes((int)fs.Length),
                        MainWindow.NotSecureKey, new byte[16]);

                    if (allPostsData.Length == 0)
                    {
                        allPostsBinary = new List<Post>();
                    }
                    else 
                        using (MemoryStream ms = new MemoryStream(allPostsData))
                        {
                            allPostsBinary = bf.Deserialize(ms) as List<Post>;
                        }
                }
            }

            m_postsList = new ObservableCollection<Post>(allPostsBinary);
        }

        public ObservableCollection<Post> PostsList
        {
            get { return m_postsList; }
        }
        

        private void NewPostTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {           
            Close();            
        }

        private void CloseCommand_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CopyCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            string toClipboard = string.Empty;
            foreach (Post v in allPostsListView.SelectedItems)
            {
                toClipboard += v.PostMessage + Environment.NewLine + v.PostDate + Environment.NewLine + Environment.NewLine;
            }
            Clipboard.SetText(toClipboard);
        }

        private void CopyCommand_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = allPostsListView.SelectedItems.Count > 0;
        }

        
        private void AddPostCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            m_postsList.Add(new Post(newPostTextBox.Text));
            newPostTextBox.Text = string.Empty;
            RaisePropertyChanged(nameof(PostsList));
        }

        private void AddPostCommand_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrWhiteSpace(newPostTextBox.Text);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, PostsList.ToList());
                MainWindow.SaveAndExit(Properties.Settings.Default.PathToFile + "_allposts", ms.ToArray());
            }
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
