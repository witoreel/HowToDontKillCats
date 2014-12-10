using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way2Test1.Engine {
    public class BissectionIterator {

        private LocalDictionary LocalDictionary;
        private Func<long, string> DictionarySearch;
        private long Iterations = 0;
        private long first_invalid_found = long.MaxValue;

        const long FirstIterationIndex = 1024;

        public BissectionIterator(Func<long, string> dictionarySearch) {
            this.DictionarySearch = dictionarySearch;
            this.LocalDictionary = new LocalDictionary();
            this.LocalDictionary.Load();
        }

        public long[] FindIndexByKeyword(string keyword) {

            first_invalid_found = long.MaxValue;
            Iterations = 0;
            LocalDictionary.WordIndexes wi = LocalDictionary.FindIndexes(keyword);

            long index;
            if (wi.Empty)
                index = FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);
            else
                index = FindByWordIndexes(keyword.ToUpper(), wi);

            LocalDictionary.Save();

            return new long[] { index, Iterations };
        }

        private long FindByWordIndexes(string keyword, LocalDictionary.WordIndexes wi) {

            if (wi.HasOneIndex)
                return FindByLastIndex(keyword.ToUpper(), wi.IdxStart);
            else {
                string word_end = Search(wi.IdxEnd);
                if (keyword.CompareTo(word_end) == 0)
                    return wi.IdxEnd;
                if (word_end == null) {
                    LocalDictionary.Clear();
                    return FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);
                }
                if (keyword.CompareTo(word_end) >= 0)
                    return FindByLastIndex(keyword.ToUpper(), wi.IdxEnd);
                else {
                    string word_start = Search(wi.IdxStart);
                    if (keyword.CompareTo(word_start) == 0)
                        return wi.IdxStart;
                    if (word_start == null) {
                        LocalDictionary.Clear();
                        return FindByLastIndex(keyword.ToUpper(), FirstIterationIndex);
                    }
                    if (keyword.CompareTo(word_start) >= 0)
                        return FindByLastIndex(keyword.ToUpper(), wi.IdxStart, wi.IdxEnd);
                    else
                        return FindByLastIndex(keyword.ToUpper(), 0, wi.IdxStart);
                }

            }

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

            if (idx_end == idx_start + 1)
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
            else
                LocalDictionary.Add(index, word);            
            Iterations++;
            return word != null ? word.ToUpper() : word;
        }
    }
}
