using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class VCdContentLocationConverterTest
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (var kind in (VCdContentLocation[])Enum.GetValues(typeof(VCdContentLocation)))
            {
                var cIdString = kind.ToString().ToUpperInvariant();

                cIdString = cIdString == "CONTENTID" ? "CONTENT-ID" : cIdString;

                var kind2 = VCdContentLocationConverter.Parse(cIdString);

                Assert.AreEqual(kind, kind2);

                cIdString = kind.ToVCardString();
                cIdString = cIdString == "CONTENT-ID" ? "ContentId" : cIdString;

                var kind3 = Enum.Parse(typeof(VCdContentLocation), cIdString, true);

                Assert.AreEqual(kind, kind3);

                // Test auf null
                Assert.AreEqual(VCdContentLocation.Inline, VCdContentLocationConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(VCdContentLocation.Inline.ToVCardString(), ((VCdContentLocation)4711).ToVCardString());
            }
        }
    }
}