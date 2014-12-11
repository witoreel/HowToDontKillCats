using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Software1.Interfaces;
using Way2Software1.Properties;

namespace Way2Software1.Engine {

    /// <summary>
    /// Classe responsável por armazenar o método de pesquisa no dicionário fornecido por WebService pela Way2.
    /// </summary>
    public class Way2Dictionary : WebDictionary {

        #region ====== Métodos Públicos ======

        /// <summary>
        /// Realiza a busca de uma palavra contida no dicionário, a partir de um índice especificado.
        /// </summary>
        /// <param name="index">Índice a ser buscado no WebService</param>
        /// <returns>Palavra contida no dicionário</returns>
        public string Search(long index) {

            string keyword = null;

            //Formata a url de acesso ao WebService
            string url = String.Format(Resources.DictionaryURL, index);
            WebRequest request = WebRequest.Create(url);
            
            //Realiza uma tentativa de acesso à base, utilizando o tratamento de erro para identificar os casos de estouro de índice,
            //seja por índices negativos como para índices maiores que o dicionário.
            try {
                Stream stream = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                keyword = reader.ReadLine();
                reader.Close();
                stream.Close();
            } catch(WebException ex) {
                string message = ex.Message;

                //Ignora o erro 400, para quando o índice é maior que o dicionário
                //Ignora o erro 406, para quando o número é maior que o suportado pelo WebService
                if (message.IndexOf("(406)") == -1 && message.IndexOf("(400)") == -1) {
                    System.Console.WriteLine("");
                    System.Console.WriteLine(Resources.WebURLError, url);
                    System.Console.WriteLine(Resources.ConsoleAskExit);
                    System.Console.Read();
                    System.Environment.Exit(0);
                }
            }
            
            return keyword;
        }

        #endregion
    }
}
