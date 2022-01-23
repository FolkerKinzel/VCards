using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class ExpertiseLevelConverterTests
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (ExpertiseLevel kind in (ExpertiseLevel[])Enum.GetValues(typeof(ExpertiseLevel)))
            {
                ExpertiseLevel? kind2 = ExpertiseLevelConverter.Parse(kind.ToString().ToLowerInvariant());

                Assert.AreEqual(kind, kind2);

                var kind3 = Enum.Parse(typeof(ExpertiseLevel), ((ExpertiseLevel?)kind).ToVcfString() ?? "", true);

                Assert.AreEqual(kind, kind3);
            }

            // Test auf null
            //Assert.AreEqual(null, ExpertiseLevelConverter.Parse(null));

            // Test auf nicht definiert
            Assert.AreEqual(null, ((ExpertiseLevel?)4711).ToVcfString());
        }

        [TestMethod]
        public void ParseTest() => Assert.IsNull(ExpertiseLevelConverter.Parse("nichtvorhanden"));

    }
}