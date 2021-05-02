using System;
using System.Collections.Generic;
using System.Text;
using FolkerKinzel.VCards.Intls.Deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests
{
    [TestClass]
    public class ParameterSectionTests
    {
        [DataTestMethod]
        [DataRow("Date")]
        [DataRow("DATE")]
        [DataRow("\"Date\"")]
        [DataRow("\'Date\'")]
        public void CleanParameterValueTest(string value)
        {
            var info = new VcfDeserializationInfo();
            var para = new ParameterSection("BDAY", new Dictionary<string, string>() { { "VALUE", value } }, info);

            Assert.AreEqual(para.DataType, Enums.VCdDataType.Date);
        }
    }

    
}
