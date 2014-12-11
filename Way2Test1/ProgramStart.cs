using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Software1.Engine;
using Way2Software1.Graphics;

namespace Way2Software1 {

    /// <summary>
    /// Classe utilizada para armazenar o método de inicialização do aplicativo.
    /// </summary>
    class ProgramStart {

        
        /// <summary>
        /// Método de inicialização do aplicativo.
        /// Nele são criada as referências aos objetos criados, passando-os como parâmetro,
        /// e por fim iniciada a rotina de escrita no console.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {

            //Inicializa os dicionários
            XMLLocalDictionary LocalDictionary = new XMLLocalDictionary();
            Way2Dictionary WebDictionary = new Way2Dictionary();

            //Inicializa o iterador e a interface do console
            BissectionIterator Iterator = new BissectionIterator(WebDictionary, LocalDictionary);
            ConsoleInterface Interface = new ConsoleInterface(Iterator);            
            
            Interface.Show();


        }

    }

}
