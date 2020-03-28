using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class VCdSexConverterTest
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (var sex in (VCdSex[])Enum.GetValues(typeof(VCdSex)))
            {
                var sex2 = VCdSexConverter.Parse(sex.ToString().Substring(0,1));

                Assert.AreEqual(sex, sex2);

                var sex3 = VCdSexConverter.Parse(VCdSexConverter.ToVCardString(sex));

                Assert.AreEqual(sex, sex3);

                // Test auf null
                Assert.AreEqual(null, VCdSexConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(null, ((VCdSex?)4711).ToVCardString());
            }
        }


    }
}