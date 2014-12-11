using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Software1.Engine;
using Way2Software1.Interface;

namespace Way2Software1 {

    class Program {

        static ConsoleInterface Interface;
        static BissectionIterator Iterator;
        static LocalDictionary LocalDictionary;

        static void Main(string[] args) {

            LocalDictionary = new LocalDictionary();
            Iterator = new BissectionIterator(DictionarySearch.SearchAtWebService);
            Iterator.SetLocalDictionaryMethods(LocalDictionary.FindIndexes, LocalDictionary.Clear, LocalDictionary.Add);
            Interface = new ConsoleInterface(Iterator.FindIndexByKeyword);
            Interface.Show();


        }

        static int[] SearchKeyWorkOnServer(string keyword) {

            return new int[]{1, 1};
        }


        static string DicSearch(long idx) {
            string[] list = new string[] { "abacaxi", "arvore", "bola", "casa", "dado", "elefante", "mesa", "tabuleiro", "vidro" };
            return idx < list.Length ? list[idx].ToUpper() : null;            
        }
    }

}
