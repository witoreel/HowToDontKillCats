using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way2Software1.Interfaces;
using Way2Software1.Tools;

namespace Way2Software1.Engine {

    /// <summary>
    /// Classe responsável por executar a busca de uma palavra chave em um dicionário, utilizando para isso o método da bisseção.
    /// Para otimização do processo, utiliza um dicionário local melhorar a escolha de índice inicial.
    /// </summary>
    public class BissectionIterator : DictionaryIterator {

        #region ====== Campos ======

        /// <summary>
        /// Objeto de dicionário web, utilizado para obter palavras a partir do índice da posição.
        /// </summary>
        private WebDictionary WebDictionary;

        /// <summary>
        /// Objeto de dicionário local, utilizaro para obter um índice de iteração inicial melhorado.
        /// </summary>
        private LocalDictionary LocalDictionary;

        /// <summary>
        /// Número de iterações realizadas no último processo de busca.
        /// </summary>
        private long Iterations = 0;

        /// <summary>
        /// Menor índice identificado como palavra inválida, ou seja, maior que o tamanho do dicionário
        /// </summary>
        private long FirstInvalidFound = long.MaxValue;

        #endregion

        #region ====== Constantes ======

        /// <summary>
        /// Valor de índice inicial de iteração padrão, utilizado quando não há nenhuma informação proveniente
        /// do dicionário local. Para a escolha deste valor, foi considerado o tamanho máximo dos dicionários 
        /// da lingua portuguesa e da inglesa, sendo ele aproximadamente em 1 milhão de palavras, ou seja 2^20.
        /// Logo, foi utilizado o valor de 1024, ou seja 2^10, por ser um intervalo médio exponencial entre os 
        /// possíveis valores mínimos e máximos de dicionários.
        /// </summary>
        const long FirstIterationIndex = 1024;

        #endregion

        #region ====== Construtores ======

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="webDictionary">Objeto de dicionário web</param>
        /// <param name="localDictionary">Objeto de dicionário local</param>
        public BissectionIterator(WebDictionary webDictionary, LocalDictionary localDictionary) {
            this.WebDictionary = webDictionary;
            this.LocalDictionary = localDictionary != null ? localDictionary : new EmptyLocalDictionary();
            if (webDictionary == null)
                throw new Exception("É necessário inserir um dicionário web não nulo!");
        }

        #endregion

        #region ====== Métodos Públicos ======

        /// <summary>
        /// Realiza a busca em um dicionário, a partir do método da bisseção, retornando o índice de uma determinada
        /// palavra chave. Também retorna o número de iterações realizadas.
        /// </summary>
        /// <param name="keyword">Palavra chave a ser buscada</param>
        /// <returns>Retorna um vetor com duas posições, sendo a primeira o índice da palavra no dicionário, e a segunda, o número de iterações</returns>
        public long[] Search(string keyword) {

            if (keyword == null || keyword.Length == 0)
                return new long[] { -1, 0 };

            //Inicializa as variaveis
            keyword = StringTools.RemoveAccents(keyword);
            FirstInvalidFound = long.MaxValue;
            Iterations = 0;

            //Identifica os índices de inicialização
            long[] local_indexes = LocalDictionary.FindIndexes(keyword);
            if (local_indexes.Length == 0)
                local_indexes = new long[] { FirstIterationIndex };            

            //Realiza as iterações
            long index = FindByWordIndexes(keyword.ToUpper(), local_indexes);

            return new long[] { index, Iterations };
        }

        /// <summary>
        /// Inicia o processo de iteração a partir de um vetor de índices iniciais.
        /// Caso o vetor esteja vazio, utilizando assim o valor de índice padrão.
        /// Caso o vetor possua uma posição, utiliza este valor como índice padrão.
        /// Caso o vetor possua duas posições, verifica se a palavra chave está antes, no meio
        /// ou depois dos índices, utilizando essa referência no restante do processo.
        /// </summary>
        /// <param name="keyword">Palavra chave a ser buscada</param>
        /// <param name="first_indexes">Vetor com os índices iniciaisk</param>
        /// <returns>Retorna o índice da palavra no dicionário</returns>
        public long FindByWordIndexes(string keyword, long[] first_indexes) {
            
            //Caso o vetor de índices iniciais for vazio, utiliza o índice inicial padrão
            if (first_indexes.Length == 0)
                return FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);

            //Caso o primeiro valor seja negativo, troca pra zero
            if (first_indexes.Length > 0 && first_indexes[0] < 0)
                first_indexes[0] = FirstIterationIndex;

            //Caso o segundo valor seja negativo, troca pra zero
            if (first_indexes.Length > 1 && first_indexes[1] < 0)
                first_indexes[1] = FirstIterationIndex;

            //Caso possua dois valores iguais, processa como se houvesse apenas 1
            if (first_indexes.Length == 2 && first_indexes[0] == first_indexes[1])
                first_indexes = new long[]{first_indexes[0]};
            
            //Caso apresente um valor de índice inicial, utiliza-o como base
            if (first_indexes.Length == 1)
                return FindByLastIndex(keyword.ToUpper(), first_indexes[0]);

            //Case apresente dois valores, verifica se o primeiro for menor, caso contrario o inverte
            if (first_indexes.Length == 2 && first_indexes[0] > first_indexes[1])
                first_indexes = new long[] { first_indexes[1], first_indexes[0] };
            
            //Coleta a palavra associada ao último índice
            string word_end = Search(first_indexes[1]);

            //Verifica se a palavra associada ao último índice é a palavra de busca, retornando-o caso seja
            if (keyword.CompareTo(word_end) == 0)
                return first_indexes[1];

            //Verifica se a palavra associada ao índice é nula, invalidando assim o dicionário e realiza a busca padrão
            if (word_end == null) {
                LocalDictionary.Clear();
                return FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);
            }

            //Verifica se a palavra associada ao último índice é maior que a palavra de busca
            //Caso seja, busca um índice acima do último índice
            if (keyword.CompareTo(word_end) >= 0)
                return FindByLastIndex(keyword.ToUpper(), first_indexes[1]);

            //Coleta a palavra associada ao primeiro índice
            string word_start = Search(first_indexes[0]);

            //Verifica se a palavra associada ao índice é nula, invalidando assim o dicionário e realiza a busca padrão
            if (word_start == null) {
                LocalDictionary.Clear();
                return FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);
            }

            //Verifica se a palavra associada ao primeiro índice é a palavra de busca, retornando-o caso seja
            if (keyword.CompareTo(word_start) == 0)
                return first_indexes[0];

            //Verifica se a palavra de busca está entre as palavras coletadas pelos índices
            //Caso estejam, busca um índice entre os índices iniciais
            //Caso contrário, busca um índice entre 0 e o primeiro índice inicial
            if (keyword.CompareTo(word_start) >= 0)
                return FindByLastIndex(keyword.ToUpper(), first_indexes[0], first_indexes[1]);
            else
                return FindByLastIndex(keyword.ToUpper(), 0, first_indexes[0]);
              
        }

        #endregion 

        #region ====== Métodos Privados ======

        /// <summary>
        /// Realiza o processo iterativo tendo como referência apenas um índice.
        /// Caso a palavra chave seja maior que a palavra deste índice, dobra o índice e tenta novamente.
        /// Caso seja menor, continua o processo considerando que a palavra chave está entre 0 e o índice.
        /// </summary>
        /// <param name="keyword">Palavra chave</param>
        /// <param name="idx">Índice de referência</param>
        /// <returns>Retorna o índice da palavra no dicionário</returns>
        private long FindByLastIndex(string keyword, long idx) {

            //Coleta a palavra associada ao índice
            string word = Search(idx);

            //Caso a palavra seja nula, ou seja além do limite do dicionário, reduz o índice pela metade
            //Caso seja igual a palavra chave, retorna o índice
            //Caso seja a palavra chave seja maior que a coletada, dobra o índice e busca novamente
            //Caso seja a palavra chave seja menor que a coletada, busca entre 0 e o índice
            if (word == null)
                return FindByLastIndex(keyword, idx / 2);
            else if (keyword.CompareTo(word) == 0)
                return idx;
            else if (keyword.CompareTo(word) > 0) {
                long idx2 = idx == 0 ? 1 : idx*2;
                return FindByLastIndex(keyword, idx, idx2);
            } else if (IsZero(keyword))
                return 0;
            else if (idx > 0)
                return FindBetweenTwoIndexes(keyword, 0, idx);
            else
                return -1;
            
        }

        /// <summary>
        /// Verifica se a palavra chave está no índice 0.
        /// </summary>
        /// <param name="keyword">Palavra chave</param>
        /// <returns>Verdadeiro, caso a palavra chave esteja no 0</returns>
        private bool IsZero(string keyword) {
            string word = Search(0);
            return keyword.CompareTo(word) == 0;
        }


                
        /// <summary>
        /// Realiza o processo de iteração, verificando se a palavra chave está acima ou abaixo do último
        /// índice. Caso esteja acima, chama novamente o método atualizando os parâmetros para o último índice
        /// e seu dobro, caso esteja abaixo, chama o método de identificação do índice a partir de um intervalo 
        /// conhecido.
        /// </summary>
        /// <param name="keyword">Palavra chave</param>
        /// <param name="idx_start">Índice de início do intervalo</param>
        /// <param name="idx_end">Índice de término do intervalo</param>
        /// <returns>Retorna o índice da palavra no dicionário</returns>
        private long FindByLastIndex(string keyword, long idx_start, long idx_end) {

            //Caso o índice seja maior que o primeiro inválido encontrado, substitui
            //Caso contrário, coleta a palavra associada ao índice
            string word = null;
            if (idx_end > FirstInvalidFound)
                idx_end = FirstInvalidFound;
            else
                word = Search(idx_end);

            //Caso a palavra coletada seja diferente da palavra chave, e os índices estejam colados,
            //retorna como índice não encontrado
            //Caso a palavra seja nula, ou seja além do limite do dicionário, reduz o índice pela metade
            //Caso seja igual a palavra chave, retorna o índice
            //Caso a palavra chave seja maior que a coletada, define o intervalo como o índice final e o dobro do índice final
            //Caso a palavra chave seja menor que a coletada, define o intervalo como o índice inicial e índice final
            if (keyword.CompareTo(word) != 0 && idx_end <= idx_start + 1)
                return -1;
            else if (word == null)
                return FindByLastIndex(keyword, idx_start, idx_start + (idx_end - idx_start)/2);
            else if (keyword.CompareTo(word) == 0)
                return idx_end;
            else if (keyword.CompareTo(word) > 0)
                return FindByLastIndex(keyword, idx_end, idx_end * 2);
            else
                return FindBetweenTwoIndexes(keyword, idx_start, idx_end);

        }

        /// <summary>
        /// Realiza o processo de iteração, considerando que a palavra chave está entre dois índices.
        /// O processo consiste em buscar a informação do índice intermediário aos dois, e verificar
        /// se a palavra chave está acima ou abaixo dele. Em ambos os casos, o universo de possíveis
        /// índices é reduzido a metade, recursivamente, até que se encontre o índice correto ou a 
        /// informação de que a palavra não está contida no dicionário.
        /// </summary>
        /// <param name="keyword">Palavra chave</param>
        /// <param name="idx_start">Índice de início do intervalo</param>
        /// <param name="idx_end">Índice de término do intervalo</param>
        /// <returns>Retorna o índice da palavra no dicionário</returns>
        private long FindBetweenTwoIndexes(string keyword, long idx_start, long idx_end) {

            //Caso os índices estejam próximos, retorna como não encontrado
            if (idx_end <= idx_start + 1)
                return -1;

            //Coleta a palavra associada ao índice intermediário
            long idx_middle = idx_start + (idx_end - idx_start) / 2;
            string word = Search(idx_middle);

            //Caso seja igual a palavra chave, retorna o índice
            //Caso a palavra chave seja maior que a coletada, define o intervalo como o índice médio e o índice final
            //Caso a palavra chave seja menor que a coletada, define o intervalo como o índice inicial e o índice médio
            if (keyword.CompareTo(word) == 0)
                return idx_middle;
            else if (keyword.CompareTo(word) > 0)
                return FindBetweenTwoIndexes(keyword, idx_middle, idx_end);
            else
                return FindBetweenTwoIndexes(keyword, idx_start, idx_middle);

        }

        /// <summary>
        /// Realiza uma busca no servidor web, retornando a palavra relacionada a um determinado índice
        /// </summary>
        /// <param name="index">Índice a ser buscado</param>
        /// <returns>Palavra associada</returns>
        private string Search(long index) {

            //Realiza a pesquisa no dicionário e incrementa o contador de iterações
            string word = WebDictionary.Search(index);
            word = StringTools.RemoveAccents(word);

            if (word == null)
                FirstInvalidFound = FirstInvalidFound > index ? index : FirstInvalidFound;
            else 
                LocalDictionary.Add(index, word);            
            Iterations++;
            return word != null ? word.ToUpper() : word;
        }

        #endregion

    }
}
