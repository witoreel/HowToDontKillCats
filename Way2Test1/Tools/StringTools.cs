using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Software1.Tools {

    /// <summary>
    /// Classe responsável por armazenar métodos estáticos de tratamento de variáveis de texto string.
    /// </summary>
    public class StringTools {

        /// <summary>
        /// Listas com relação entre caracteres acentuados e sua versão sem acento
        /// </summary>
        private static string[] SignReference = new string[] { "ãâáàäêéèëîíìïõôóòöûúùüñçÃÂÁÀÄÊÉÈËÎÍÌÏÕÔÓÒÖÛÚÙÜÑÇ", "aaaaaeeeeiiiiooooouuuuncAAAAAEEEEIIIIOOOOOUUUUNC"};


        /// <summary>
        /// Remove todos caracteres acentuados, substituindo-os por sua versão não acentuada.
        /// </summary>
        /// <param name="text">Texto a ser processado</param>
        /// <returns>Texto processado</returns>
        public static string RemoveAccents(string text) {

            if (text == null || text.Length == 0)
                return text;

            string s = text;
            for (int i = 0; i < SignReference[0].Length; i++)
                s = s.Replace(SignReference[0][i], SignReference[1][i]);

            return s;

        }

    }
}
