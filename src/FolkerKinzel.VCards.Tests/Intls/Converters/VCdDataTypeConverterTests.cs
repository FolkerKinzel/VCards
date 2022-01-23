using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class VCdDataTypeConverterTests
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (VCdDataType kind in (VCdDataType[])Enum.GetValues(typeof(VCdDataType)))
            {
                string? s = VCdDataTypeConverter.ToVcfString(kind);
                VCdDataType? kind2 = VCdDataTypeConverter.Parse(s);
                Assert.AreEqual(kind, kind2);
            }

            // Test auf null
            Assert.AreEqual(null, VCdDataTypeConverter.Parse(null));

            // Test auf nicht definiert
            Assert.AreEqual(null, ((VCdDataType?)4711).ToVcfString());
        }
    }
}