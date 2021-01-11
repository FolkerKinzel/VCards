using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class InterestLevelConverterTests
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (InterestLevel kind in (InterestLevel[])Enum.GetValues(typeof(InterestLevel)))
            {
                InterestLevel? kind2 = InterestLevelConverter.Parse(kind.ToString());

                Assert.AreEqual(kind, kind2);

                var kind3 = Enum.Parse(typeof(InterestLevel), ((InterestLevel?)kind).ToVCardString() ?? "", true);

                Assert.AreEqual(kind, kind3);

                // Test auf null
                //Assert.AreEqual(null, InterestLevelConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(null, ((InterestLevel?)4711).ToVCardString());
            }
        }
    }
}