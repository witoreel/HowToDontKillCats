﻿using System;
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
        private List<DateTime> registers {
            get {
                if (_registers == null)
                    _registers = new List<DateTime>();
                return _registers;
            }
            set {
                _registers = value;
            }
        }

        public void Add(DateTime item) {
            registers.Add(item);
        }

        public void Clear() {
            registers.Clear();
        }

        public int Count {
            get {
                return registers.Count();
            }
        }

        void AfterLoad() {
            List<DateTime> d = new List<DateTime>();
                if (Registers != null)
                    for (int i = 0; i < Registers.Length; i++)
                        d.Add(Registers[i]);
                registers = d;
        }

        void BeforeSave() {
            if (registers == null)
                Registers = new DateTime[0];

            DateTime[] d = new DateTime[registers.Count];
            for (int i = 0; i < registers.Count; i++)
                d[i] = registers[i];
            Registers = d;
        }

        public void Load() {
            FileInfo file = new FileInfo(FileName);            
            if (File.Exists(FileName)) {
                Stream stream = File.OpenRead(FileName);
                try {
                    SoapFormatter deserializer = new SoapFormatter();
                    WebServiceAccessList accessList = (WebServiceAccessList)deserializer.Deserialize(stream);                    
                    stream.Close();

                    this.Registers = accessList.Registers;
                    AfterLoad();
                } catch {
                    stream.Close();
                    file.Delete();
                }
            }
            
        }

        public void Save() {
            Stream stream = File.Create(FileName);
            SoapFormatter serializer = new SoapFormatter();
            BeforeSave();
            serializer.Serialize(stream, this);
            stream.Close();
        }

    }
}
