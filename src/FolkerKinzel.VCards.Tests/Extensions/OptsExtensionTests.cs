using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class OptsExtensionTests
{
    [TestMethod()]
    public void VcfOptionsHelperTest()
    {
        VcfOpts tp = VcfOpts.None;

        Assert.IsFalse(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsTrue(tp.IsSet(VcfOpts.None));

        tp = tp.Set(VcfOpts.WriteGroups);
        Assert.IsTrue(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsFalse(tp.IsSet(VcfOpts.None));


        // Set doppelt aufrufen
        tp = tp.Set(VcfOpts.WriteGroups);
        Assert.IsTrue(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsFalse(tp.IsSet(VcfOpts.None));


        tp = tp.Set(VcfOpts.WriteEmptyProperties);
        Assert.IsTrue(tp.IsSet(VcfOpts.WriteEmptyProperties));
        Assert.IsTrue(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsFalse(tp.IsSet(VcfOpts.None));


        tp = tp.Unset(VcfOpts.WriteEmptyProperties);
        Assert.IsFalse(tp.IsSet(VcfOpts.WriteEmptyProperties));
        Assert.IsTrue(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsFalse(tp.IsSet(VcfOpts.None));


        // Unset doppelt aufrufen:
        tp = tp.Unset(VcfOpts.WriteEmptyProperties);
        Assert.IsFalse(tp.IsSet(VcfOpts.WriteEmptyProperties));
        Assert.IsTrue(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsFalse(tp.IsSet(VcfOpts.None));


        // letztes Flag löschen
        tp = tp.Unset(VcfOpts.WriteGroups);
        Assert.IsFalse(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsTrue(tp.IsSet(VcfOpts.None));


        // Unset auf None aufrufen:
        tp = tp.Unset(VcfOpts.WriteGroups);

        Assert.IsFalse(tp.IsSet(VcfOpts.WriteGroups));
        Assert.IsTrue(tp.IsSet(VcfOpts.None));
    }
}