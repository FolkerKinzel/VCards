using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FolkerKinzel.VCards.Models.Tests;

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
        var serializer = new Vcf_4_0Serializer(writer, VcfOptions.Default);
        prop2.AppendValue(serializer);

        Assert.AreEqual(0, serializer.Builder.Length);
    }
}
