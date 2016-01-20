using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpEnigma.Tests
{
    [TestClass()]
    public class CharactersAssistantTests
    {
        private TestContext context;

        public TestContext TestContext
        {
            get { return context; }
            set { context = value; }
        }

        [TestMethod()]
        [DataSource(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Robert\Documents\Visual Studio 2015\Projects\CSharpEnigma\TestData\CSharpEnigmaTestData.accdb",
            "NextCharacterTestData")]
        public void nextCharacterTest()
        {
            Alphabet current = (Alphabet)Enum.Parse(typeof(Alphabet), TestContext.DataRow["CurrentCharacter"].ToString());
            Alphabet expectedNext = (Alphabet)Enum.Parse(typeof(Alphabet), TestContext.DataRow["NextCharacter"].ToString());
            Alphabet actualNext = CharactersAssistant.NextCharacter(current);
            Assert.AreEqual(expectedNext, actualNext);
        }

        [TestMethod()]
        [DataSource(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Robert\Documents\Visual Studio 2015\Projects\CSharpEnigma\TestData\CSharpEnigmaTestData.accdb",
            "PreviousCharacterTestData")]
        public void previousCharacterTest()
        {
            Alphabet current = (Alphabet)Enum.Parse(typeof(Alphabet), TestContext.DataRow["CurrentCharacter"].ToString());
            Alphabet expectedPrevious = (Alphabet)Enum.Parse(typeof(Alphabet), TestContext.DataRow["PreviousCharacter"].ToString());
            Alphabet actualPrevious = CharactersAssistant.PreviousCharacter(current);
            Assert.AreEqual(expectedPrevious, actualPrevious);
        }
    }
}