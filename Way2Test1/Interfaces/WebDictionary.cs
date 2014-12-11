using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Software1.Interfaces {

    /// <summary>
    /// Interface responsável por possibilitar a pesquisa de informações em um dicionário web
    /// </summary>
    public interface WebDictionary {

        /// <summary>
        /// Método que irá realizar a pesquisa de uma palavra no dicionário web, a partir de seu índice
        /// </summary>
        /// <param name="index">Índice a ser pesquisado no dicionário</param>
        /// <returns>Palavra associada ao índice</returns>
        string Search(long index);

    }
}
