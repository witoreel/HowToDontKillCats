using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Way2Software1.Tools;

namespace Way2Software1Tester.Tests {

    /// <summary>
    /// Testa a classe utilizada para realizar formatações de variáveis de texto.
    /// </summary>
    [TestClass]
    public class StringToolsTests {

        /// <summary>
        /// Testa o método de remoção de acentos.
        /// Tenta inserir um parâmetro nulo.
        /// Testa uma lista de palavras acentuadas.
        /// </summary>
        [TestMethod]
        public void TestRemoveAccents() {

            //Inserir um parâmetro nulo
            string ans = StringTools.RemoveAccents(null);
            Assert.AreEqual(null, ans, "Test 1");

            //Testa uma lista de palavras
            string[] words = new string[] { "café", "xícara", "tração", "carroça" };
            string[] words_unsigned = new string[] { "cafe", "xicara", "tracao", "carroca" };
            for (int i = 0; i < words.Length; i++) {
                ans = StringTools.RemoveAccents(words[i]);
                Assert.AreEqual(words_unsigned[i], ans, "Test 2."+i);
            }


        }



    }
}
