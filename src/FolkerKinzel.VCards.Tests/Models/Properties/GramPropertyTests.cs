using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Properties.Tests;

[TestClass()]
public class GramPropertyTests
{
    [TestMethod]
    public void IEnumerableTest()
    {
        var tProp = new GramProperty(Gram.Feminine);

        GramProperty value = tProp.AsWeakEnumerable().Cast<GramProperty>().First();

        Assert.AreSame(tProp, value);

    }

    [TestMethod]
    public void IEnumerableTTest()
    {
        var tProp = new GramProperty(Gram.Feminine);

        GramProperty value = tProp.First();

        Assert.AreSame(tProp, value);

    }

    [TestMethod]
    public void TryParseTest1()
        => Assert.IsFalse(GramProperty.TryParse(VcfRow.Parse("GRAMGENDER:blabla".AsMemory(),
                                                             new VcfDeserializationInfo())!,
                          out _));
}