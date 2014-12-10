using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Test1.Dictionary;
using Way2Test1.Engine;
using Way2Test1.Interface;

namespace Way2Test1 {

    class Program {

        static ConsoleInterface Interface;
        static DictionarySearch Dictionary;
        static BissectionIterator Iterator;

        static void Main(string[] args) {

            Dictionary = new DictionarySearch();
            Iterator = new BissectionIterator(DicSearch);
            Interface = new ConsoleInterface(Iterator.FindIndexByKeyword);
            System.Console.WriteLine(Dictionary.WebServiceAccessCount + " gatinhos mortos");
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
