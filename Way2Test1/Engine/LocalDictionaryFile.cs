using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;

namespace Way2Test1.Engine {

    [Serializable()]
    public class LocalDictionary {

        [Serializable()]
        public struct DictionaryEntry {
            public long Index { get; set; }
            public string Word { get; set; }

            public override string ToString() {
                return String.Format("{0} - {1}", Index, Word);
            }
        }

        public struct WordIndexes{
            public long IdxStart;
            public long IdxEnd;

            public WordIndexes(long idx_start, long idx_end) {
                IdxStart = idx_start;
                IdxEnd = idx_end;                
            }

            public bool Empty { get { return IdxStart == -1 && IdxEnd == -1; } }

            public bool HasOneIndex { get { return IdxStart != -1 && IdxEnd == -1; } }
        }

        [field: NonSerialized()]
        const string FileName = @"..\..\LocalDictionary.xml";

        [field: NonSerialized()]
        const int MaxSize = 1024;

        [field: NonSerialized()]
        private List<DictionaryEntry> _registers;
        private List<DictionaryEntry> registers { 
            get {
                if (_registers == null)
                    _registers = new List<DictionaryEntry>();
                return _registers;
            }
            set {
                _registers = value;
            }
        }

        public DictionaryEntry[] Registers { get; set; }

        public void Add(long index, string word) {
            DictionaryEntry de = new DictionaryEntry();
            de.Word = word;
            de.Index = index;

            int idx = -1;
            for (int i = 0; i < registers.Count; i++) {
                if (word.CompareTo(registers[i].Word) == 0) {
                    if (index != registers[i].Index)
                        Clear();
                    
                    return;
                }
                if (word.CompareTo(registers[i].Word) < 0) {
                    idx = i;
                    break;
                }
            }
            if (idx == -1)
                registers.Add(de);
            else
                registers.Insert(idx, de);

            for (int i = 0; i < registers.Count-1; i++)
                if (registers[i].Index >= registers[i + 1].Index) {
                    Clear();
                    break;

                }
        }

        public void Clear() {
            registers.Clear();
            Save();
        }

        public int Count {
            get {
                return registers.Count();
            }
        }

        void AfterLoad() {
            List<DictionaryEntry> d = new List<DictionaryEntry>();
            if (Registers != null)
                for (int i = 0; i < Registers.Length; i++)
                    d.Add(Registers[i]);
            registers = d;
        }

        void BeforeSave() {
            if (registers == null)
                Registers = new DictionaryEntry[0];

            DictionaryEntry[] d = new DictionaryEntry[registers.Count];
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
                    LocalDictionary accessList = (LocalDictionary)deserializer.Deserialize(stream);
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


        public WordIndexes FindIndexes(string keyword) {

            if (registers.Count == 0)
                return new WordIndexes(-1, -1);

            if (keyword.CompareTo(registers[0].Word) < 0)
                if (registers[0].Index == 0)
                    return new WordIndexes(0, -1);
                else
                    return new WordIndexes(0, registers[0].Index);

            if (keyword.CompareTo(registers[registers.Count-1].Word) > 0 )
                return new WordIndexes(registers[registers.Count-1].Index, -1);

            for (int i = 0; i < registers.Count; i++){
                if (keyword.CompareTo(registers[i].Word) == 0 )
                    return new WordIndexes(registers[i].Index, -1);
                if (i < registers.Count-1)
                    if (keyword.CompareTo(registers[i+1].Word) < 0)
                        return new WordIndexes(registers[i].Index, registers[i+1].Index);
            }

            return new WordIndexes(-1, -1);
        }


    }
}
