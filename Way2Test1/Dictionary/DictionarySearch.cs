using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Way2Test1.Properties;

namespace Way2Test1.Dictionary {


    public class DictionarySearch {

        private WebServiceAccessList WebServiceAccessed;

        public int WebServiceAccessCount { 
            get {
                return WebServiceAccessed.Count;
            } 
        }

        public DictionarySearch() {
            WebServiceAccessed = new WebServiceAccessList();
            WebServiceAccessed.Load();
        }


        public string Search(long index) {

            string keyword = null;
            string url = String.Format(Resources.DictionaryURL, index);
            WebRequest request = WebRequest.Create(url);
            
            try {
                Stream stream = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                keyword = reader.ReadLine();

                reader.Close();
                stream.Close();
            } catch(WebException ex) {
                string message = ex.Message;
                if (message.IndexOf("(406)") == -1 && message.IndexOf("(400)") == -1)
                    System.Console.WriteLine(ex.Message);                    
            }

            WebServiceAccessed.Add(DateTime.Now);
            WebServiceAccessed.Save();

            return keyword;
        }


    }
}
