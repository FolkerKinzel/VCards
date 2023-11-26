using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

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

        Assert.AreEqual(para.DataType, Data.Date);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AssignTest1()
    {
        var sec = new ParameterSection();
        sec.Assign(null!);
    }

    [TestMethod]
    public void AssignTest2()
    {
        var sec = new ParameterSection();
        sec.Assign(sec);
    }

    [TestMethod]
    public void AddressTypeTest1()
    {
        var sec = new ParameterSection
        {
            AddressType = default(Adr)
        };
        Assert.IsNull(sec.AddressType);
    }

    [TestMethod]
    public void IndexTest1()
    {
        var sec = new ParameterSection
        {
            Index = -7
        };
        Assert.AreEqual(1, sec.Index);

        sec.Index = 1;
        Assert.AreEqual(1, sec.Index);

        sec.Index = null;
        Assert.IsNull(sec.Index);
    }
}
