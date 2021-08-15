using System.Text;
using FolkerKinzel.VCards.Intls.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests.Intls.Converters.Tests
{
    [TestClass]
    public class VCdTextEncodingConverterTests
    {
        private const int UTF8 = 65001;

        [TestMethod]
        public void ParseTest1()
        {
            Encoding enc = TextEncodingConverter.GetEncoding("blödelidödel");

            Assert.AreEqual(UTF8, enc.CodePage);
        }


        [TestMethod]
        public void ParseTest2()
        {
            Encoding enc = TextEncodingConverter.GetEncoding("ISO-8859-1");

            Assert.AreEqual(28591, enc.CodePage);
        }
    }
}
