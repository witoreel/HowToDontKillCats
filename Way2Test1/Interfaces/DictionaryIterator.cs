using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Software1.Interfaces {

    /// <summary>
    /// Interface responsável por iterar um dicionário, retornando o índice de uma palavra e a quantidade de iterações
    /// </summary>
    public interface DictionaryIterator {

        /// <summary>
        /// Realiza a busca por iterações de uma palavra em um dicionário, retornando um vetor com duas posições
        /// contendo as informações do índice da palavra e do número de iterações, respectivamente
        /// </summary>
        /// <param name="word">Palavra a ser buscada</param>
        /// <returns>Vetor com o resultado</returns>
        long[] Search(string word);

    }
}
