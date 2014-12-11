using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Software1.Engine {

    /// <summary>
    /// 
    /// </summary>
    public class BissectionIterator {

        private Func<long, string> DictionarySearch;
        private Func<string, long[]> LocalDictionarySearch = null;
        private Func<int> LocalDictionaryClear = null;
        private Func<long, string, int>  LocalDictionaryAdd = null;
        private long Iterations = 0;
        private long first_invalid_found = long.MaxValue;

        const long FirstIterationIndex = 1024;

        public BissectionIterator(Func<long, string> dictionarySearch) {
            this.DictionarySearch = dictionarySearch;           
        }

        public void SetLocalDictionaryMethods(Func<string, long[]> searchFunction, Func<int> clearFunction, Func<long, string, int> addFunction) {
            LocalDictionarySearch = searchFunction;
            LocalDictionaryClear = clearFunction;
            LocalDictionaryAdd = addFunction;
        }

        public long[] FindIndexByKeyword(string keyword) {

            first_invalid_found = long.MaxValue;
            Iterations = 0;
            long[] local_indexes = new long[] { FirstIterationIndex };
            if (LocalDictionarySearch != null)
                local_indexes = LocalDictionarySearch(keyword);

            long index = FindByWordIndexes(keyword.ToUpper(), local_indexes);

            return new long[] { index, Iterations };
        }

        private long FindByWordIndexes(string keyword, long[] first_indexes) {
            
            //Caso o vetor de índices iniciais for vazio, utiliza o índice inicial padrão
            if (first_indexes.Length == 0)
                return FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);

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
                if (LocalDictionaryClear != null)
                    LocalDictionaryClear();
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
                if (LocalDictionaryClear != null)
                    LocalDictionaryClear();
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


        private long FindByLastIndex(string keyword, long idx) {

            string word = Search(idx);
            if (word == null)
                return FindByLastIndex(keyword, idx / 2);
            else if (keyword.CompareTo(word) == 0)
                return idx;
            else if (keyword.CompareTo(word) > 0)
                return FindByLastIndex(keyword, idx, idx * 2);
            else if (IsZero(keyword))
                return 0;
            else if (idx > 0)
                return FindBetweenTwoIndexes(keyword, 0, idx);
            else
                return -1;
            
        }

        private bool IsZero(string keyword) {
            string word = Search(0);
            return keyword.CompareTo(word) == 0;
        }


        private long FindByLastIndex(string keyword, long idx_start, long idx_end) {

            string word = null;
            if (idx_end > first_invalid_found)
                idx_end = first_invalid_found;
            else
                word = Search(idx_end);
            if (word == null && idx_end == idx_start + 1)
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

        private long FindBetweenTwoIndexes(string keyword, long idx_start, long idx_end) {

            if (idx_end <= idx_start + 1)
                return -1;

            long idx_middle = idx_start + (idx_end - idx_start) / 2;
            string word = Search(idx_middle);
            if (keyword.CompareTo(word) == 0)
                return idx_middle;
            else if (keyword.CompareTo(word) > 0)
                return FindBetweenTwoIndexes(keyword, idx_middle, idx_end);
            else
                return FindBetweenTwoIndexes(keyword, idx_start, idx_middle);

        }

        private string Search(long index) {
            string word = DictionarySearch(index);
            if (word == null)
                first_invalid_found = first_invalid_found > index ? index : first_invalid_found;
            else if (LocalDictionaryAdd != null)
                LocalDictionaryAdd(index, word);            
            Iterations++;
            return word != null ? word.ToUpper() : word;
        }
    }
}
