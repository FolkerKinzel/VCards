using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class AddrExtensionTests
{
    [TestMethod()]
    public void AddressTypesTest()
    {
        Adr? tp = null;

        Assert.IsFalse(tp.IsSet(Adr.Dom));

        tp = tp.Set(Adr.Dom);
        Assert.IsTrue(tp.IsSet(Adr.Dom));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(Adr.Dom);
        Assert.IsTrue(tp.IsSet(Adr.Dom));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(Adr.Postal);
        Assert.IsTrue(tp.IsSet(Adr.Postal));
        Assert.IsTrue(tp.IsSet(Adr.Dom));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(Adr.Postal);
        Assert.IsFalse(tp.IsSet(Adr.Postal));
        Assert.IsTrue(tp.IsSet(Adr.Dom));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(Adr.Postal);
        Assert.IsFalse(tp.IsSet(Adr.Postal));
        Assert.IsTrue(tp.IsSet(Adr.Dom));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(Adr.Dom);
        Assert.IsFalse(tp.IsSet(Adr.Dom));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(Adr.Dom);

        Assert.IsFalse(tp.IsSet(Adr.Dom));
        Assert.IsFalse(tp.HasValue);
    }
}