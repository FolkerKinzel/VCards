using FolkerKinzel.VCards.Intls.Deserializers;

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


/* Unmerged change from project 'FolkerKinzel.VCards.Tests (net7.0)'
Before:
        Assert.AreEqual(para.DataType, Enums.Data.Date);
After:
        Assert.AreEqual(para.DataType, Data.Date);
*/
        Assert.AreEqual(para.DataType, VCards.Enums.Data.Date);
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

}
