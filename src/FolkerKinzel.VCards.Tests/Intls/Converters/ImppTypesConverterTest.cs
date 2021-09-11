using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ImppTypesConverterTest
    {
        [TestMethod()]
        public void ParseTest()
        {
            foreach (ImppTypes kind in (ImppTypes[])Enum.GetValues(typeof(ImppTypes)))
            {
                ImppTypes? kind2 = ImppTypesConverter.Parse(kind.ToString().ToUpperInvariant());

                Assert.AreEqual(kind, kind2);

                Assert.IsNull(ImppTypesConverter.Parse("NICHT_VORHANDEN"));
            }
        }

        
    }
    

}
