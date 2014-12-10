using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Test1.Engine {
    public class BissectionIterator {

        private Func<int, string> DictionarySearch;
        private int Iterations = 0;

        const int FirstIterationIndex = 1024;

        public BissectionIterator(Func<int, string> dictionarySearch) {
            this.DictionarySearch = dictionarySearch;
        }


        public int[] FindIndexByKeyword(string keyword) {

            Iterations = 0;
            int index = FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);

            return new int[]{index, Iterations};
        }


        private int FindByLastIndex(string keyword, int idx) {

            string word = Search(idx);
            if (word == null)
                return FindByLastIndex(keyword, idx / 2);
            else if (keyword.CompareTo(word) == 0)
                return idx;
            else if (keyword.CompareTo(word) > 0)
                return FindByLastIndex(keyword, idx, idx * 2);
            else if (IsZero(keyword))
                return 0;
            else 
                return FindBetweenTwoIndexes(keyword, 0, idx);
            
        }

        private bool IsZero(string keyword) {
            string word = Search(0);
            return keyword.CompareTo(word) == 0;
        }


        private int FindByLastIndex(string keyword, int idx_start, int idx_end) {

            string word = Search(idx_end);
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

        private int FindBetweenTwoIndexes(string keyword, int idx_start, int idx_end) {

            if (idx_end == idx_start + 1)
                return -1;

            int idx_middle = idx_start + (idx_end - idx_start) / 2;
            string word = Search(idx_middle);
            if (keyword.CompareTo(word) == 0)
                return idx_middle;
            else if (keyword.CompareTo(word) > 0)
                return FindBetweenTwoIndexes(keyword, idx_middle, idx_end);
            else
                return FindBetweenTwoIndexes(keyword, idx_start, idx_middle);

        }

        private string Search(int index) {
            string word = DictionarySearch(index);
            Iterations++;
            return word != null ? word.ToUpper() : word;
        }
    }
}
