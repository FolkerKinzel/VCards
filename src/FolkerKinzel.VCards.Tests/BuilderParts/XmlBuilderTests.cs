using System.Xml.Linq;
using System.Xml.Schema;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class XmlBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new XmlBuilder().SetPreferences();

    [TestMethod]
    public void SetPreferencesTest2()
    {
        XNamespace ns = "http://www.contoso.com";

        VCardBuilder builder = VCardBuilder
            .Create()
            .Xmls.Add(null)
            .Xmls.Add(new XElement(ns + "Key1", "First"))
            .Xmls.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Xmls);
        Assert.AreEqual(2, vc.Xmls.Count());
        Assert.AreEqual(100, vc.Xmls.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Xmls.ElementAt(1)!.Parameters.Preference);

        builder.Xmls.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Xmls.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Xmls.ElementAt(1)!.Parameters.Preference);

        builder.Xmls.UnsetPreferences();
        Assert.IsTrue(vc.Xmls.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new XmlBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new XmlBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        XNamespace ns = "http://www.contoso.com";

        VCardBuilder builder = VCardBuilder
            .Create()
            .Xmls.Add(null)
            .Xmls.Add(new XElement(ns + "Key1", "First"))
            .Xmls.SetIndexes();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Xmls);
        Assert.AreEqual(2, vc.Xmls.Count());
        Assert.AreEqual(null, vc.Xmls.First()!.Parameters.Index);
        Assert.AreEqual(1, vc.Xmls.ElementAt(1)!.Parameters.Index);

        builder.Xmls.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Xmls.First()!.Parameters.Index);
        Assert.AreEqual(2, vc.Xmls.ElementAt(1)!.Parameters.Index);

        builder.Xmls.UnsetIndexes();
        Assert.IsTrue(vc.Xmls.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new XmlBuilder().UnsetIndexes();


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new XmlBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Xmls.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new XmlBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Xmls.Edit(null!, true);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new XmlBuilder().Add(null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new XmlBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new XmlBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new XmlBuilder().Equals((XmlBuilder?)null));

        var builder = new XmlBuilder();
        Assert.AreEqual(builder.GetHashCode(),((object) builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new XmlBuilder().ToString());
}
