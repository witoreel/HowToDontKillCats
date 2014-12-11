
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Software1.Interfaces {

    /// <summary>
    /// Interface utilizada para acessar informações de um dicionário local
    /// </summary>
    public interface LocalDictionary {

        /// <summary>
        /// Adiciona uma nova entrada ao dicionário local.
        /// </summary>
        /// <param name="index">Índice associado à palavra</param>
        /// <param name="word">Palavra de referência</param>
        void Add(long index, string word);

        /// <summary>
        /// Limpa as informações do dicionário local.
        /// </summary>
        void Clear();

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
        long[] FindIndexes(string word);

    }
}
