using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Test1.Properties;

namespace Way2Test1 {
    
    public class ConsoleInterface {

        private Func<string, int[]> SearchHandler;

        public ConsoleInterface(Func<string, int[]> searchHandler) {
            this.SearchHandler = searchHandler;
        }

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
