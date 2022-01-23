using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class VCdKindConverterTests
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (VCdKind kind in (VCdKind[])Enum.GetValues(typeof(VCdKind)))
            {
                VCdKind kind2 = VCdKindConverter.Parse(kind.ToString());

                Assert.AreEqual(kind, kind2);

                var kind3 = Enum.Parse(typeof(VCdKind), kind.ToVcfString(), true);

                Assert.AreEqual(kind, kind3);
            }

            // Test auf null
            Assert.AreEqual(VCdKind.Individual, VCdKindConverter.Parse(null));

            // Test auf nicht definiert
            Assert.AreEqual(VCdKind.Individual.ToVcfString(), ((VCdKind)4711).ToVcfString());
        }
    }
}