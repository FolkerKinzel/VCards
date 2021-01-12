using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Deserializers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;



namespace FolkerKinzel.VCards.Intls.Deserializers.Tests
{
    [TestClass()]
    public class DigitParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            for (int i = 0; i < 10; i++)
            {
                string s = i.ToString();

                Assert.AreEqual(i, DigitParser.Parse(s[0]));
            }
        }
    }
}