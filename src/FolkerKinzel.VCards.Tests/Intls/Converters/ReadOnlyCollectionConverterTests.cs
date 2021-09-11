using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ReadOnlyCollectionConverterTests
    {
        [TestMethod]
        public void ToReadOnlyCollectionTest1()
        {
            Assert.IsNotNull(ReadOnlyCollectionConverter.ToReadOnlyCollection(new string[] { "Hello" }));
        }

        [TestMethod]
        public void ToReadOnlyCollectionTest3()
        {
            Assert.IsNotNull(ReadOnlyCollectionConverter.ToReadOnlyCollection(new ReadOnlyCollection<string>(new string[] { "Hello" })));
        }

        [TestMethod]
        public void ToReadOnlyCollectionTest2()
        {
            Assert.IsNotNull(ReadOnlyCollectionConverter.ToReadOnlyCollection(Yielder()));
        }


        private static IEnumerable<string> Yielder()
        {
            yield return "Hello";
        }

    }

    
}
