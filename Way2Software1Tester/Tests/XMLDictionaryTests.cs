using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Way2Software1.Engine;

namespace Way2Software1Tester.Tests {

    /// <summary>
    /// Testa a classe responsável por manter dicionários locais em arquivos XML.
    /// </summary>
    [TestClass]
    public class XMLDictionaryTests {

        /// <summary>
        /// Realiza os testes sobre a função de limpar o dicionário.
        /// Para isso, adiciona algums termos ao dicionário, o limpa e por fim verifica seu tamanho.
        /// </summary>
        [TestMethod]
        public void TestClear() {

            string[] words = new string[] { "software", "class", "interface"};
            long[] indexes = new long[] { 1, 5, 7 };

            //Popula um objeto fake
            XMLLocalDictionary dictionary = new XMLLocalDictionary();
            for (int i = 0; i < words.Length; i++)
                dictionary.Add(indexes[i], words[i]);

            //Realiza o teste e as validações
            Assert.AreNotEqual(0, dictionary.Count);
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);

        }

        /// <summary>
        /// Realiza os testes sobre a função de adicionar entradas.
        /// Serão adicionadas entradas com ordem corretas.
        /// Serão adicionadas entradas com ordem incorreta.
        /// Será adicionado uma entrada com índice encavalado.
        /// Será adicionada uma palavra nula.
        /// Será adicionado um índice negativo.
        /// Será adicionado uma palavra já adicionada, com mesmo índice diferente.
        /// Será adicionado uma palavra já adicionada, com índice diferente.
        /// Será adicionado um índice já adicionado.
        /// </summary>
        [TestMethod]
        public void TestAdd() {

            //Adiciona entradas com ordem correta
            string[] words = new string[] { "class", "interface", "software" };
            long[] indexes = new long[] { 1, 5, 7 };
            XMLLocalDictionary dictionary = new XMLLocalDictionary();
            for (int i = 0; i < words.Length; i++)
                dictionary.Add(indexes[i], words[i]);
            Assert.AreEqual(words.Length, dictionary.Count, "Test 1");

            //Adiciona entradas com ordem errada
            dictionary.Clear();
            words = new string[] { "interface", "software", "class" };
            indexes = new long[] { 1, 5, 7 };
            for (int i = 0; i < words.Length; i++)
                dictionary.Add(indexes[i], words[i]);
            Assert.AreEqual(1, dictionary.Count, "Test 2");

            //Realiza a segunda etapa do teste, inserindo a palavra "enumerator" com o índice 8
            //ocasionando um erro, uma vez que já está adicionado o registro "interface" com índice 7
            //Devendo então o dicionário ser limpo
            int last_count = dictionary.Count;
            dictionary.Add(8, "enumerator");
            Assert.AreEqual(last_count+1, dictionary.Count, "Test 3");

            //Realiza um teste adicionadno um valor nulo na palavra, não podendo ser adicionada
            last_count = dictionary.Count;
            dictionary.Add(9, null);
            Assert.AreEqual(last_count, dictionary.Count, "Test 4");

            //Realiza um teste adicionando um valor com índice negativo, não podendo ser adicionado
            last_count = dictionary.Count;
            dictionary.Add(-3, "method");
            Assert.AreEqual(last_count, dictionary.Count, "Test 5");
            
            //Realiza um teste adicionando uma palavra já adicionada
            dictionary.Clear();
            dictionary.Add(8, "enumerator");
            last_count = dictionary.Count;
            dictionary.Add(8, "enumerator");
            Assert.AreEqual(last_count, dictionary.Count, "Test 6");

            //Realiza um teste adicionando uma palavra já adicionada, porém com índice diferente
            dictionary.Add(7, "enumerator");
            Assert.AreEqual(1, dictionary.Count, "Test 7");

            //Realiza um teste adicionando um índice já adicionado, porém com outra palavra
            dictionary.Add(7, "method");
            Assert.AreEqual(1, dictionary.Count, "Test 8");

        }

        /// <summary>
        /// Realiza testes do método de busca de índices a partir de uma palavra chave.
        /// Será passado por parâmetro null.
        /// Será passado por parâmetro um caracter menor que a primeira.
        /// Será passado por parâmetro uma palavra contida no dicionário.
        /// Será passado por parâmetro uma palavra contida entre termos do dicionário.
        /// Será passado por parâmetro um caracter maior que última.
        /// Será passado por parâmetro uma palavra com acento.
        /// </summary>
        [TestMethod]
        public void TestFindIndexes() {

            //Cadastra um pequeno dicionário conhecido
            string[] words = new string[] { "class", "enumerator", "interface", "software" };
            long[] indexes = new long[] { 1, 4, 5, 7 };
            XMLLocalDictionary dictionary = new XMLLocalDictionary();
            for (int i = 0; i < words.Length; i++)
                dictionary.Add(indexes[i], words[i]);

            //Verifica inserir null
            long[] ans = dictionary.FindIndexes(null);
            Assert.AreEqual(0, ans.Length, "Test 1");

            //Verifica inserir uma palavra menor que a primeira
            ans = dictionary.FindIndexes("abstract");
            Assert.AreEqual(2, ans.Length, "Test 2");
            Assert.AreEqual(0, ans[0], "Test 3");
            Assert.AreEqual(1, ans[1], "Test 4");

            //Verifica inserir uma palavra conhecida
            ans = dictionary.FindIndexes("enumerator");
            Assert.AreEqual(1, ans.Length, "Test 5");
            Assert.AreEqual(4, ans[0], "Test 6");

            //Verifica inserir uma palavra entre duas conhecidas
            ans = dictionary.FindIndexes("function");
            Assert.AreEqual(2, ans.Length, "Test 7");
            Assert.AreEqual(4, ans[0], "Test 8");
            Assert.AreEqual(5, ans[1], "Test 9");

            //Verifica inserir uma palavra maior que a última
            ans = dictionary.FindIndexes("unit");
            Assert.AreEqual(1, ans.Length, "Test 10");
            Assert.AreEqual(7, ans[0], "Test 11");

            //Verifica inserir uma palavra com acento
            ans = dictionary.FindIndexes("enumerâtor");
            Assert.AreEqual(1, ans.Length, "Test 12");
            Assert.AreEqual(4, ans[0], "Test 13");
        }

    }
}
