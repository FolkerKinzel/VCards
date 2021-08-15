using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable.Tests
{
    [TestClass]
    public class QuotedPrintableConverterTests
    {
        [TestMethod]
        public void DecodeStringTest()
        {
            string quoted = $"1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand={Environment.NewLine} Firma";

            string? s = QuotedPrintableConverter.Decode(quoted);

            Assert.IsNotNull(s);
            StringAssert.Contains(s, "Firmenstraße");
            StringAssert.EndsWith(s, " Firma");
        }
    }
}
