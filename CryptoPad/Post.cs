using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CryptoPad
{

    [Serializable]
    public class Post
    {
        private DateTime m_date;
        private string m_postMessage;

        public Post()
        {
            m_postMessage = string.Empty;
            m_date = DateTime.Now;
        }

        public Post(string message)
        {
            m_postMessage = message;
            m_date = DateTime.Now;
        }

        public DateTime PostDate
        {
            get { return m_date; }
            set {  m_date = value; }
        }
        public string PostDateString
        {
            get { return m_date.ToString("hh:mm:ss dd.MM.yyyy"); }
        }

        public string PostMessage
        {
            get { return m_postMessage; }
            set {  m_postMessage = value; }
        }
    }
}
