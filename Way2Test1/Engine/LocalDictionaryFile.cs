using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;

namespace Way2Software1.Engine {

    /// <summary>
    /// Classe responsável por armazenar informações de dicionário local. 
    /// As informações são salvas em HD em um arquivo XML na pasta do programa a partir de serialização.
    /// Esse dicionário local é utilizado para indicar melhores índices iniciais para o método de bisseção.
    /// Como o dicionário web pode alterar, os índices iniciais não representam o índice real, sendo apenas 
    /// uma estimativa. Também por esse motivo, o dicionário é validado a cada nova entrada, sendo inteiramente
    /// excluído caso tenha algum indício que o dicionário web sofreu alteração.
    /// </summary>
    [Serializable()]
    public class LocalDictionary {

        /// <summary>
        /// Estrutura responsável por representar uma entrada no dicionário local, possuindo informações
        /// da palavra e do índice associado.
        /// </summary>
        [Serializable()]
        public struct DictionaryEntry {

            /// <summary>
            /// Índice associado à palavra identificado em um momento passado no dicionário web
            /// </summary>
            public long Index { get; set; }

            /// <summary>
            /// Palavra de referência do dicionário
            /// </summary>
            public string Word { get; set; }

            /// <summary>
            /// Define as informações da entrada
            /// </summary>
            /// <param name="index">Índice associado á palavra</param>
            /// <param name="word">Palavra de referência</param>
            public void Set(long index, string word) {
                Index = index;
                Word = word;
            }

            /// <summary>
            /// Sobrecarga do método ToString, apenas para o processo de depuração
            /// </summary>
            /// <returns>Texto de identificação da estrutura</returns>
            public override string ToString() {
                return String.Format("{0} - {1}", Index, Word);
            }
        }

        /// <summary>
        /// Endereço do arquivo XML onde serão salvos os dados deste objeto
        /// </summary>
        [field: NonSerialized()]
        const string FileName = @"..\..\LocalDictionary.xml";

        /// <summary>
        /// Tamanho máximo de entradas permitidas no dicionário local
        /// </summary>
        [field: NonSerialized()]
        const int MaxSize = 1024;

        /// <summary>
        /// Lista utilizada para controlar as entradas do dicionário
        /// </summary>
        [field: NonSerialized()]
        private List<DictionaryEntry> registers = new List<DictionaryEntry>(); 

        /// <summary>
        /// Propriedade utilizada para salvar/carregar a lista de entradas no arquivo XML
        /// </summary>
        public DictionaryEntry[] Registers { get; set; }

        /// <summary>
        /// Realiza a validação do dicionário, a partir da nova entrada.
        /// Caso haja algum indício de que o dicionário web sofreu alterações, este dicionário
        /// será excluido e então será criado novamente.
        /// </summary>
        /// <param name="entry">Nova entrada do dicionário local</param>
        private void ValidateDictionary(DictionaryEntry entry) {

            //Verifica se o registro novo não invalida os antigos, seguindo algumas regras     
            for (int i = 0; i < registers.Count; i++) {
                bool invalid = false;

                //Invalida caso palavra nova for maior/menor que palavra registrada, com índice novo menor/maior que índice registrado
                invalid = invalid || entry.Word.CompareTo(registers[i].Word) > 0 && entry.Index < registers[i].Index;
                invalid = invalid || entry.Word.CompareTo(registers[i].Word) < 0 && entry.Index > registers[i].Index;

                //Invalida caso palavra nova for igual/diferente que palavra registrada, com índice novo diferente/igual que índice registrado
                invalid = invalid || entry.Word.CompareTo(registers[i].Word) == 0 && entry.Index != registers[i].Index;
                invalid = invalid || entry.Word.CompareTo(registers[i].Word) != 0 && entry.Index == registers[i].Index;

                if (invalid) {
                    Clear();
                    break;
                }
            }

        }

        /// <summary>
        /// Adiciona uma nova entrada no dicionário local, a partir de uma palavra e de seu indíce associado,
        /// obtidos na última vez que foi acessado o dicionário web.
        /// </summary>
        /// <param name="index">Índice associado à uma palavra</param>
        /// <param name="word">Palavra de referência</param>
        /// <returns>Declarado apenas para poder passar o método como parâmetro</returns>
        public int Add(long index, string word) {
            
            //Não adiciona a entrada caso já tenha atingido o tamanho máximo do dicionário
            if (registers.Count >= MaxSize)
                return 0;

            //Cria uma nova entrada no dicionário
            DictionaryEntry entry = new DictionaryEntry();
            entry.Set(index, word);

            //Valida a integrade do dicionário
            ValidateDictionary(entry);
           
            //Identifica em qual posição da lista deverá ser inserido a nova entrada
            int idx = -1;
            for (int i = 0; i < registers.Count; i++) {               
                if (word.CompareTo(registers[i].Word) < 0) {
                    idx = i;
                    break;
                }
            }

            //Adiciona a entrada na lista
            if (idx == -1)
                registers.Add(entry);
            else
                registers.Insert(idx, entry);
            Save();

            return 0;
        }

        /// <summary>
        /// Exclui todas as entradas do dicionário e salva as informações no HD.
        /// </summary>
        /// <returns>Declarado apenas para poder passar o método como parâmetro</returns>
        public int Clear() {
            registers.Clear();
            Save();
            return 0;
        }

        /// <summary>
        /// Método responsável por alimentar a lista de entradas a partir da propriedade salva no arquivo XML,
        /// executado após carregar as informações do arquivo.
        /// </summary>
        private void AfterLoad() {
            List<DictionaryEntry> d = new List<DictionaryEntry>();
            if (Registers != null)
                for (int i = 0; i < Registers.Length; i++)
                    d.Add(Registers[i]);
            registers = d;
        }

        /// <summary>
        /// Método responsável por alimentar a propriedade a ser salva no arquivo XML,
        /// executado antes de salvar as informações no arquivo.
        /// </summary>
        void BeforeSave() {
            if (registers == null)
                Registers = new DictionaryEntry[0];

            DictionaryEntry[] d = new DictionaryEntry[registers.Count];
            for (int i = 0; i < registers.Count; i++)
                d[i] = registers[i];
            Registers = d;
        }

        /// <summary>
        /// Carrega as informações deste objeto a partir do arquivo XML contido no HD.
        /// </summary>
        public void Load() {

            //Verifica a existência do arquivo, e caso exista, carrega as informações do mesmo a partir
            //de serialização Soap
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

        /// <summary>
        /// Salva as informações deste objeto em um arquivo XML no HD.
        /// </summary>
        public void Save() {

            //Salva o arquivo a partir de serialização Soap
            Stream stream = File.Create(FileName);
            SoapFormatter serializer = new SoapFormatter();
            BeforeSave();
            serializer.Serialize(stream, this);
            stream.Close();
        }

        /// <summary>
        /// Retorna a posição mais provavel de uma determinada palavra no dicionário local.
        /// Caso o dicionário esteja vazio, retorna um vetor nulo.
        /// Caso seja menor que a primeira palavra contida na lista, retorna um vetor com 0 
        /// na primeira posição e o índice do primeira entrada na segunda posição.
        /// Caso seja maior que a última palavra contida na lista, retorna um vetor com apenas 
        /// o índice da última entrada.
        /// Caso esteja entre dois membros da lista, retorna os índices dos respectivos membros.
        /// </summary>
        /// <param name="keyword">Palavra a ser buscada no dicionário</param>
        /// <returns>Retorna um vetor com índices de referência da posição da palavra</returns>
        public long[] FindIndexes(string keyword) {

            //Retorna um vetor vazio caso não tenha entradas na lista
            if (registers.Count == 0)
                return new long[0];

            //Caso a palavra seja menor que a primeira entrada, retorna um vetor com os termos 0 e o primeiro índice
            if (keyword.CompareTo(registers[0].Word) < 0)
                return new long[] { 0, registers[0].Index };

            //Caso a palavra seja maior que a última entrada, retorna um vetor com o primeiro índice e o último índice
            if (keyword.CompareTo(registers[registers.Count-1].Word) > 0 )
                return new long[]{registers[registers.Count-1].Index};

            //Verifica entre quais entradas se encontra a palavra, retornando um vetor com os índices associados a estas entradas
            for (int i = 0; i < registers.Count; i++){
                if (keyword.CompareTo(registers[i].Word) == 0 )
                    return new long[]{registers[i].Index};
                if (i < registers.Count-1)
                    if (keyword.CompareTo(registers[i+1].Word) < 0)
                        return new long[]{registers[i].Index, registers[i+1].Index};
            }

            return new long[0];
        }


    }
}
