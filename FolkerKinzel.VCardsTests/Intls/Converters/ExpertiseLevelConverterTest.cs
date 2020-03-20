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
    public class ExpertiseLevelConverterTest
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (var kind in (ExpertiseLevel[])Enum.GetValues(typeof(ExpertiseLevel)))
            {
                var kind2 = ExpertiseLevelConverter.Parse(kind.ToString());

                Assert.AreEqual(kind, kind2);

                var kind3 = Enum.Parse(typeof(ExpertiseLevel), ((ExpertiseLevel?)kind).ToVCardString() ?? "", true);

                Assert.AreEqual(kind, kind3);

                // Test auf null
                //Assert.AreEqual(null, ExpertiseLevelConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(null, ((ExpertiseLevel?)4711).ToVCardString());
            }
        }
    }
}