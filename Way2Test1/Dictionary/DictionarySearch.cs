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


        public string Search(int index) {

            string url = String.Format(Resources.DictionaryURL, index);
            WebRequest request = WebRequest.Create(url);
            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            string keyword = reader.ReadLine();

            reader.Close();
            stream.Close();

            WebServiceAccessed.Add(DateTime.Now);
            WebServiceAccessed.Save();

            return keyword;
        }


    }
}
