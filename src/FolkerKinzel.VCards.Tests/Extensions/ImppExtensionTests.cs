using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class ImppExtensionTests
{
    [TestMethod()]
    public void ImppTypesTest()
    {
        Impp? tp = null;

        Assert.IsFalse(tp.IsSet(Impp.Personal));

        tp = tp.Set(Impp.Personal);
        Assert.IsTrue(tp.IsSet(Impp.Personal));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(Impp.Personal);
        Assert.IsTrue(tp.IsSet(Impp.Personal));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(Impp.Business);
        Assert.IsTrue(tp.IsSet(Impp.Business));
        Assert.IsTrue(tp.IsSet(Impp.Personal));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(Impp.Business);
        Assert.IsFalse(tp.IsSet(Impp.Business));
        Assert.IsTrue(tp.IsSet(Impp.Personal));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(Impp.Business);
        Assert.IsFalse(tp.IsSet(Impp.Business));
        Assert.IsTrue(tp.IsSet(Impp.Personal));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(Impp.Personal);
        Assert.IsFalse(tp.IsSet(Impp.Personal));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(Impp.Personal);

        Assert.IsFalse(tp.IsSet(Impp.Personal));
        Assert.IsFalse(tp.HasValue);
    }
}