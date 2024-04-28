
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class RelExtensionTests
{
    [TestMethod()]
    public void RelationTypesTest()
    {
        Rel? tp = null;

        Assert.IsFalse(tp.IsSet(Rel.Agent));

        tp = tp.Set(Rel.Agent);
        Assert.IsTrue(tp.IsSet(Rel.Agent));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(Rel.Agent);
        Assert.IsTrue(tp.IsSet(Rel.Agent));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(Rel.Contact);
        Assert.IsTrue(tp.IsSet(Rel.Contact));
        Assert.IsTrue(tp.IsSet(Rel.Agent));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(Rel.Contact);
        Assert.IsFalse(tp.IsSet(Rel.Contact));
        Assert.IsTrue(tp.IsSet(Rel.Agent));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(Rel.Contact);
        Assert.IsFalse(tp.IsSet(Rel.Contact));
        Assert.IsTrue(tp.IsSet(Rel.Agent));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(Rel.Agent);
        Assert.IsFalse(tp.IsSet(Rel.Agent));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(Rel.Agent);

        Assert.IsFalse(tp.IsSet(Rel.Agent));
        Assert.IsFalse(tp.HasValue);
    }

}