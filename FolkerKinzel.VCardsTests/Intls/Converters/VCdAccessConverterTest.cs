using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    
    [TestClass()]
    public class VCdAccessConverterTest
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (var kind in (VCdAccess[])Enum.GetValues(typeof(VCdAccess)))
            {
                var kind2 = VCdAccessConverter.Parse(kind.ToString());

                Assert.AreEqual(kind, kind2);

                var kind3 = Enum.Parse(typeof(VCdAccess), kind.ToVCardString(), true);

                Assert.AreEqual(kind, kind3);

                // Test auf null
                Assert.AreEqual(VCdAccess.Public, VCdAccessConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(VCdAccess.Public.ToVCardString(), ((VCdAccess)4711).ToVCardString());
            }
        }
    }
}