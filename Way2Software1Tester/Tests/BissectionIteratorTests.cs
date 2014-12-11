using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Way2Software1.Engine;

namespace Way2Software1Tester.Tests {

    /// <summary>
    /// Testa a classe responsável por iterar o dicionário e retornar o índice da palavra, a partir do método de bisseção.
    /// </summary>
    [TestClass]
    public class BissectionIteratorTests {

        /// <summary>
        /// Testa buscar determinadas palavras dentro do dicionário.
        /// Será testado buscar uma palavra existente.
        /// Será testado buscar uma palavra não existente.
        /// Será testado buscar uma palavra não existente.
        /// Será testado buscar uma palavra existente, porem com algum tipo de acentuação.
        /// Será testado buscar uma palavra existente, com maiusculas e minusculas.
        /// Será testado buscar uma palavra nula.
        /// </summary>
        [TestMethod]
        public void TestSearch() {

            //Inicializa os dicionários
            Way2Dictionary WebDictionary = new Way2Dictionary();

            //Inicializa o iterador e a interface do console
            BissectionIterator Iterator = new BissectionIterator(WebDictionary, null);
            long[] ans = Iterator.Search("casa");
            Assert.AreNotEqual(-1, ans[0], "Test 1");
                        
            ans = Iterator.Search("asdfghjklç");
            Assert.AreEqual(-1, ans[0], "Test 2");

            ans = Iterator.Search("casá");
            Assert.AreNotEqual(-1, ans[0], "Test 3");

            ans = Iterator.Search("CaSa");
            Assert.AreNotEqual(-1, ans[0], "Test 4");

            ans = Iterator.Search(null);
            Assert.AreEqual(-1, ans[0], "Test 5");

        }

        /// <summary>
        /// Realiza testes da função que itera o dicionário a partir de índices iniciais.
        /// Insere um vetor de índices vazio.
        /// Insere um vetor com um índice menor que a palavra.
        /// Insere um vetor com um índice maior que a palavra.
        /// Insere um vetor com um índice igual que a palavra.
        /// Insere um vetor com dois índices menores que a palavra.
        /// Insere um vetor com dois índices maiores que a palavra.
        /// Insere um vetor com dois índices iguais que a palavra.
        /// Insere um vetor com dois índices, um menor e um maior que a palavra.
        /// Insere um vetor com dois índices iguais, diferentes da palavra.
        /// Insere um vetor com dois índices com ordem invertida.
        /// Insere um vetor com dois índices colados.
        /// Insere um vetor com índice negativo.
        /// </summary>
        [TestMethod]
        public void TestFindByWordIndexes() {

            
            //Inicializa os objetos
            Way2Dictionary WebDictionary = new Way2Dictionary();
            BissectionIterator Iterator = new BissectionIterator(WebDictionary, null);

            //Referência: índice da palavra casa = 7390
            long index_ref = 7390;

            //Insere índices vazios
            long ans = Iterator.FindByWordIndexes("casa", new long[0]);
            Assert.AreEqual(index_ref, ans, "Test 1");

            //Insere um índice menor, maior e igual que o da palavra
            ans = Iterator.FindByWordIndexes("casa", new long[]{ 6000 });
            Assert.AreEqual(index_ref, ans, "Test 2");
            ans = Iterator.FindByWordIndexes("casa", new long[] { 8000 });
            Assert.AreEqual(index_ref, ans, "Test 3");
            ans = Iterator.FindByWordIndexes("casa", new long[] { index_ref });
            Assert.AreEqual(index_ref, ans, "Test 4");

            //Insere dois índices maiores, dois menores, um maior e um menor e dois iguais
            ans = Iterator.FindByWordIndexes("casa", new long[] { 8000, 10000 });
            Assert.AreEqual(index_ref, ans, "Test 5");
            ans = Iterator.FindByWordIndexes("casa", new long[] { 4000, 6000 });
            Assert.AreEqual(index_ref, ans, "Test 6");
            ans = Iterator.FindByWordIndexes("casa", new long[] { 4000, 8000 });
            Assert.AreEqual(index_ref, ans, "Test 7");
            ans = Iterator.FindByWordIndexes("casa", new long[] { index_ref, index_ref });
            Assert.AreEqual(index_ref, ans, "Test 8");

            //Insere dois índices iguais, diferentes do da palavra
            ans = Iterator.FindByWordIndexes("casa", new long[] { 10000, 10000 });
            Assert.AreEqual(index_ref, ans, "Test 9");

            //Insere dois índices com ordem invertida
            ans = Iterator.FindByWordIndexes("casa", new long[] { 10000, 8000 });
            Assert.AreEqual(index_ref, ans, "Test 10");
            
            //Insere dois índices negativo
            ans = Iterator.FindByWordIndexes("casa", new long[] { -1 });
            Assert.AreEqual(index_ref, ans, "Test 11");
            ans = Iterator.FindByWordIndexes("casa", new long[] { -1, 10 });
            Assert.AreEqual(index_ref, ans, "Test 12");
            ans = Iterator.FindByWordIndexes("casa", new long[] { -1, -5 });
            Assert.AreEqual(index_ref, ans, "Test 13");

        }



    }
}
