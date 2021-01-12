using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Intls.Tests
{
    [TestClass]
    public class VcfRowTests
    {
        [DataTestMethod]
        [DataRow("folker.TEL;TYPE=work,voice;VALUE=uri:tel:+49-221-9999123", false, "TEL", "folker", 3)]
        public void ParseTest(string input, bool rowIsNull, string key, string? group, int parametersCount)
        {
            var info = new VCardDeserializationInfo();
            info.Builder.Append(input);
            var row = VcfRow.Parse(info);

            if(rowIsNull)
            {
                Assert.IsNull(row);
                return;
            }

            Assert.IsNotNull(row);
            Assert.IsNotNull(row!.Parameters);
            Assert.AreEqual(parametersCount, row.Parameters.Count);
            Assert.AreEqual(key, row.Key, false);
            Assert.AreEqual(group, row.Group, false);
        }
    }
}
