using FolkerKinzel.VCards.Enums;

using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

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
        var para = new ParameterSection("BDAY", $"VALUE={value}".AsMemory(), info);

        Assert.AreEqual(Data.Date, para.DataType);
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

    [TestMethod]
    public void ToStringTest1()
    {
        var sec = new ParameterSection
        {
            Label = $"""
            First line
            Second line
            """
        };

        string s = sec.ToString();

        Assert.IsTrue(s.GetLinesCount() > 1);
    }

    [TestMethod]
    public void SortAsTest()
    {
        List<string?> list = ["", "  ", " Contoso ", null, "IT"];

        var prop = new OrgProperty("Contoso", ["Computer", "Internet"]);
        prop.Parameters.SortAs = list!;

        IEnumerable<string> result = prop.Parameters.SortAs;

        Assert.IsNotNull(result);
        Assert.IsTrue(!result.Any(x => x is null));
        Assert.IsTrue(result.All(x => StringComparer.Ordinal.Equals(x, x.Trim())));
        Assert.AreEqual(2, result.Count());

        list.Add(null);
        Assert.IsTrue(!result.Any(x => x is null));
        Assert.AreEqual(2, result.Count());

        list.Add(" Web ");
        Assert.IsTrue(!result.Any(x => x is null));
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.All(x => StringComparer.Ordinal.Equals(x, x.Trim())));
    }
}
