using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace Way2Test1.Dictionary {

    [Serializable()]
    public class WebServiceAccessList{

        [field: NonSerialized()]
        const string FileName = @"..\..\WebServiceAccessList.xml";

        [field: NonSerialized()]
        private List<DateTime> _registers;

        public DateTime[] Registers { get; set; }

        public void Add(DateTime item) {
            if (_registers == null)
                _registers = new List<DateTime>();
            _registers.Add(item);
        }

        public void Clear() {
            _registers.Clear();
        }

        void AfterLoad() {
            List<DateTime> d = new List<DateTime>();
                if (Registers != null)
                    for (int i = 0; i < Registers.Length; i++)
                        d.Add(Registers[i]);
                _registers = d;
        }

        void BeforeSave() {
            if (_registers == null)
                Registers = new DateTime[0];

            DateTime[] d = new DateTime[_registers.Count];
            for (int i = 0; i < _registers.Count; i++)
                d[i] = _registers[i];
            Registers = d;
        }

        public static WebServiceAccessList Load() {
            FileInfo file = new FileInfo(FileName);            
            if (File.Exists(FileName)) {
                Stream stream = File.OpenRead(FileName);
                try {
                    SoapFormatter deserializer = new SoapFormatter();
                    WebServiceAccessList accessList = (WebServiceAccessList)deserializer.Deserialize(stream);
                    accessList.AfterLoad();
                    stream.Close();
                    return accessList;
                } catch {
                    stream.Close();
                    file.Delete();
                }
            }
            return new WebServiceAccessList();            
        }

        public static void Save(WebServiceAccessList accessList) {
            Stream stream = File.Create(FileName);
            SoapFormatter serializer = new SoapFormatter();
            accessList.BeforeSave();
            serializer.Serialize(stream, accessList);
            stream.Close();
        }

    }
}
