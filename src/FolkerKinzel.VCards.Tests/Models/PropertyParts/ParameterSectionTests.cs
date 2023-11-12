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
    public void SetPropertyIDTest1()
    {
        var vc = new VCard();
        var arr = new TextProperty[] { new TextProperty("Donald"), new TextProperty("Duck") };
        vc.DisplayNames = arr;

        arr[0].Parameters.SetPropertyID(vc.DisplayNames, vc);
        Assert.AreEqual(1, arr[0].Parameters.PropertyIDs.First()!.ID);

        arr[1].Parameters.SetPropertyID(vc.DisplayNames, vc);
        Assert.AreEqual(2, arr[1].Parameters.PropertyIDs.First()!.ID);

        var list = new List<PropertyID?>(arr[1].Parameters.PropertyIDs!);
        list.Insert(0, null);
        arr[1].Parameters.PropertyIDs = list;

        arr[1].Parameters.SetPropertyID(vc.DisplayNames, vc);
        Assert.AreEqual(2, arr[1].Parameters.PropertyIDs.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetPropertyIDTest2()
    {
        var vc = new VCard();
        var arr = new TextProperty[] { new TextProperty("Donald"), new TextProperty("Duck") };
        vc.DisplayNames = arr;

        arr[0].Parameters.SetPropertyID(vc.DisplayNames, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetPropertyIDTest3()
    {
        var vc = new VCard();
        var arr = new TextProperty[] { new TextProperty("Donald"), new TextProperty("Duck") };
        vc.DisplayNames = arr;

        arr[0].Parameters.SetPropertyID(null!, vc);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPropertyIDTest4()
    {
        var vc = new VCard();
        var arr = new TextProperty?[] { null, new TextProperty("Donald"), new TextProperty("Duck") };

        arr[1]!.Parameters.SetPropertyID(arr, vc);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPropertyIDTest5()
    {
        var vc = new VCard();
        vc.DisplayNames = new TextProperty("Duck");

        new TextProperty("Donald").Parameters.SetPropertyID(vc.DisplayNames, vc);
    }
}
