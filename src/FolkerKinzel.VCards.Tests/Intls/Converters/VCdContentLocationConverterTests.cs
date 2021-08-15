using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class VCdContentLocationConverterTests
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (VCdContentLocation kind in (VCdContentLocation[])Enum.GetValues(typeof(VCdContentLocation)))
            {
                string cIdString = kind.ToString().ToUpperInvariant();

                cIdString = cIdString == "CONTENTID" ? "CID" : cIdString;

                VCdContentLocation kind2 = VCdContentLocationConverter.Parse(cIdString);

                Assert.AreEqual(kind, kind2);

                cIdString = kind.ToVCardString();
                cIdString = cIdString == "CID" ? "ContentId" : cIdString;

                object kind3 = Enum.Parse(typeof(VCdContentLocation), cIdString, true);

                Assert.AreEqual(kind, kind3);

                // Test auf null
                Assert.AreEqual(VCdContentLocation.Inline, VCdContentLocationConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(VCdContentLocation.Inline.ToVCardString(), ((VCdContentLocation)4711).ToVCardString());
            }
        }
    }
}