using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Way2Software1.Engine;

namespace Way2Software1Tester.Tests {

    /// <summary>
    /// Realiza testes na classe de pesquisa de palavras no dicionário web da Way2
    /// </summary>
    [TestClass]
    public class Way2DictionaryTests {

        /// <summary>
        /// Testa o método de busca de palavra pelo índice, no dicionário.
        /// Será testado buscar um índice normal.
        /// Será testado buscar um índice menor que zero.
        /// Será testado buscar um índice próximo do limite de tamanho do int.
        /// Será testado buscar um índice próximo do limite de tamanho do long.
        /// </summary>
        [TestMethod]
        public void TestSearch() {

            Way2Dictionary dictionary = new Way2Dictionary();

            //Teste normal
            string word = dictionary.Search(5);
            Assert.IsNotNull(word, "Test 1");

            //Teste com índice negativo
            word = dictionary.Search(-1);
            Assert.IsNull(word, "Test 2");

            //Teste com índice próximo do limite do int
            word = dictionary.Search(int.MaxValue - 1000);
            Assert.IsNull(word, "Test 3");

            //Teste com índice próximo do limite do long
            word = dictionary.Search(long.MaxValue - 1000);
            Assert.IsNull(word, "Test 4");
        }



    }
}
