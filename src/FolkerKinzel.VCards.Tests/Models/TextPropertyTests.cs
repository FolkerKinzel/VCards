using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class TextPropertyTests
    {
        //[TestMethod()]
        //public void TextPropertyTest()
        //{

        //}

        //[TestMethod()]
        //public void CloneTest()
        //{

        //}

        [TestMethod]
        public void IEnumerableTest()
        {
            var tProp = new TextProperty("Good value");

            TextProperty value = tProp.AsWeakEnumerable().Cast<TextProperty>().First();

            Assert.AreSame(tProp, value);

        }

        [TestMethod]
        public void IEnumerableTTest()
        {
            var tProp = new TextProperty("Good value");

            TextProperty value = tProp.First();

            Assert.AreSame(tProp, value);

        }
    }
}