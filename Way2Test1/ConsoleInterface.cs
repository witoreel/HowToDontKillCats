using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Test1.Properties;

namespace Way2Test1 {
    
    /// <summary>
    /// Classe responsável por realizar a interface entre o programa e a tela de Console.
    /// Nela são executados todos os métodos Write e Read da classe System.Console, sendo 
    /// a única responsável no programa por isso. Todas as chamadas de funções são realizadas 
    /// através de métodos passados como parâmetro, não havendo nenhum vínculo desta classe
    /// com outras do aplicativo.
    /// </summary>
    public class ConsoleInterface {

        /// <summary>
        /// Método utilizado para realizar a busca da palavra chave no dicionário.
        /// Aceita como parâmetro um valor string, que é a palavra chave.
        /// Deve retornar um vetor de int com duas posições: 
        /// Na primeira deve conter o índice da palavra no dicionário, ou -1 caso ela não exista.
        /// Na segunda deve conter a quantidade de iterações (buscas no webservice) realizadas no processo.
        /// </summary>
        private Func<string, int[]> SearchHandler;

        /// <summary>
        /// Construtor padrão.
        /// Recebe como parâmetro o método utilizado para realizar a busca da palavra chave no dicionário.
        /// Esta função aceita como parâmetro um valor string, que é a palavra chave.
        /// Ela deve retornar um vetor de int com duas posições: 
        /// Na primeira deve conter o índice da palavra no dicionário, ou -1 caso ela não exista;
        /// Na segunda deve conter a quantidade de iterações (buscas no webservice) realizadas no processo.
        /// </summary>
        /// <param name="searchHandler">Função responsável por executar o processo de busca da palavra chave</param>
        public ConsoleInterface(Func<string, int[]> searchHandler) {
            this.SearchHandler = searchHandler;
        }

        /// <summary>
        /// Método responsável por inicializar a exibição das mensagens no Console.
        /// Nele são definidas todas possibilidades de acesso do usuário ao software.
        /// </summary>
        public void Show() {

            System.Console.WriteLine(Resources.ConsoleHeader);

            bool keepSearching = false;
            do{
                System.Console.WriteLine();
                System.Console.WriteLine(Resources.ConsoleSeparator);
                System.Console.Write(Resources.ConsoleAskKeyword+" ");                
                string keyword = System.Console.ReadLine();

                int[] answer = SearchHandler(keyword);
                int index = answer[0];
                int iterations = answer[1];

                string indexMsg = index < 0 ? Resources.ConsoleAnswerNotFound : Resources.ConsoleAnswerFound;
                string iterationsMsg = iterations == 1 ? Resources.ConsoleAnswerIterationSingle : Resources.ConsoleAnswerIterationPlural;

                System.Console.WriteLine("\t"+String.Format(indexMsg, index));
                System.Console.WriteLine("\t" + String.Format(iterationsMsg, iterations));
                System.Console.WriteLine();

                bool keepAskingContinue;
                do{
                    keepAskingContinue = false;
                    System.Console.Write(Resources.ConsoleAskReSearch);
                    ConsoleKeyInfo key = System.Console.ReadKey();
                    switch (key.KeyChar){
                        case 'S':
                            keepSearching = true;
                            break;
                        case 's':
                            keepSearching = true;
                            break;
                        case 'N':
                            keepSearching = false;
                            break;
                        case 'n':
                            keepSearching = false;
                            break;
                        default:
                            keepAskingContinue = true;
                            break;
                    }
                    System.Console.WriteLine();
                }while(keepAskingContinue);
 
            } while (keepSearching);

            System.Console.Write(Resources.ConsoleExit);
            Thread.Sleep(1000);

        }


    }

}
