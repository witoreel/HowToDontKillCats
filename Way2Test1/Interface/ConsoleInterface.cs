using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way2Software1.Properties;

namespace Way2Software1.Interface {
    
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
        private Func<string, long[]> SearchHandler;

        /// <summary>
        /// Construtor padrão.
        /// Recebe como parâmetro o método utilizado para realizar a busca da palavra chave no dicionário.
        /// Esta função aceita como parâmetro um valor string, que é a palavra chave.
        /// Ela deve retornar um vetor de int com duas posições: 
        /// Na primeira deve conter o índice da palavra no dicionário, ou -1 caso ela não exista;
        /// Na segunda deve conter a quantidade de iterações (buscas no webservice) realizadas no processo.
        /// </summary>
        /// <param name="searchHandler">Função responsável por executar o processo de busca da palavra chave</param>
        public ConsoleInterface(Func<string, long[]> searchHandler) {
            this.SearchHandler = searchHandler;
        }

        /// <summary>
        /// Método responsável por inicializar a exibição das mensagens no Console.
        /// Nele são definidas todas possibilidades de acesso do usuário ao software.
        /// </summary>
        public void Show() {

            //Desenha o cabeçalho da aplicação
            System.Console.WriteLine(Resources.ConsoleHeader);

            //Inicia o laço responsável por permitir ao usuário realizar diversas pesquisas sem ter de reiniciar o programa
            bool keepSearching;
            do{
                keepSearching = false;

                System.Console.WriteLine();
                System.Console.WriteLine(Resources.ConsoleSeparator);                          

                //Inicia o laço responsável por pedir ao usuário a palavra a ser buscada novamente, caso seja inserido inicialmente um valor inválido
                bool keepAskingWord;
                string keyword;
                do{
                    keepAskingWord = false;
                    System.Console.Write(Resources.ConsoleAskKeyword + " ");      
                    keyword = System.Console.ReadLine();

                    //Valida a palavra inserida, verificando se não é vazia
                    if (keyword == null || keyword.Trim().Length == 0) {
                        keepAskingWord = true;
                        System.Console.WriteLine();
                    }
                }while(keepAskingWord);
                
                //Realiza a pesquisa da palavra no dicionário e coleta os resultados
                //É esperado um vetor de long com duas posições, sendo a primeira o índice da palavra no dicionário
                //e a segunda o número de iterações realizadas
                long[] answer = SearchHandler(keyword);
                long index = answer[0];
                long iterations = answer[1];

                //Processa as mensagens a serem exibidas e as exibe
                string indexMsg = index < 0 ? Resources.ConsoleAnswerNotFound : Resources.ConsoleAnswerFound;
                string iterationsMsg = iterations == 1 ? Resources.ConsoleAnswerIterationSingle : Resources.ConsoleAnswerIterationPlural;
                System.Console.WriteLine("\t"+String.Format(indexMsg, index));
                System.Console.WriteLine("\t" + String.Format(iterationsMsg, iterations));
                System.Console.WriteLine();

                //Laço responsável por perguntar ao usuário se ele deseja pesquisar novamente uma palavra
                //Caso o usuário insira um caracter diferente de 's', 'S', 'n', 'N', continua perguntando
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

            //Exibe uma mensagem informando que o sistema irá fechar e espera um segundo para isso
            System.Console.Write(Resources.ConsoleExit);
            Thread.Sleep(1000);

        }


    }

}
