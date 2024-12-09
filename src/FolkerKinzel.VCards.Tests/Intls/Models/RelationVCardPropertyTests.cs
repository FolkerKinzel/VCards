using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models.Tests;

[TestClass]
public class RelationVCardPropertyTests
{
    [TestMethod]
    public void RelationVCardPropertyTest1()
    {
        var prop = RelationProperty.FromVCard(null);
        Assert.IsTrue(prop.IsEmpty);

        var prop2 = prop.Clone() as RelationProperty;
        Assert.IsNotNull(prop2);
        Assert.IsTrue(prop2!.IsEmpty);
        Assert.AreNotSame(prop, prop2);

        using var writer = new StringWriter();
        var serializer = new Vcf_4_0Serializer(writer, Opts.Default);
        prop2.AppendValue(serializer);

        Assert.AreEqual(0, serializer.Builder.Length);
    }

    

    [TestMethod]
    public void CircularReferenceTest1()
    {
        var vc = new VCard() { DisplayNames = new TextProperty("Donald Duck") };

        vc.Relations = RelationProperty.FromVCard(vc);
        string s = vc.ToString();
        Assert.IsNotNull(s);
    }

    [TestMethod]
    public void CircularReferenceTest2()
    {
        var donald = new VCard() { DisplayNames = new TextProperty("Donald Duck") };
        var dagobert = new VCard
        {
            DisplayNames = new TextProperty("Dagobert Duck"),
            Relations = RelationProperty.FromVCard(donald)
        };
        donald.Relations = RelationProperty.FromVCard(dagobert);

        string s = donald.ToString();
        Assert.IsNotNull(s);
    }
}
