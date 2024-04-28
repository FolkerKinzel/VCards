using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class OptsExtensionTests
{
    [TestMethod()]
    public void VcfOptionsHelperTest()
    {
        Opts tp = Opts.None;

        Assert.IsFalse(tp.IsSet(Opts.WriteGroups));
        Assert.IsTrue(tp.IsSet(Opts.None));

        tp = tp.Set(Opts.WriteGroups);
        Assert.IsTrue(tp.IsSet(Opts.WriteGroups));
        Assert.IsFalse(tp.IsSet(Opts.None));


        // Set doppelt aufrufen
        tp = tp.Set(Opts.WriteGroups);
        Assert.IsTrue(tp.IsSet(Opts.WriteGroups));
        Assert.IsFalse(tp.IsSet(Opts.None));


        tp = tp.Set(Opts.WriteEmptyProperties);
        Assert.IsTrue(tp.IsSet(Opts.WriteEmptyProperties));
        Assert.IsTrue(tp.IsSet(Opts.WriteGroups));
        Assert.IsFalse(tp.IsSet(Opts.None));


        tp = tp.Unset(Opts.WriteEmptyProperties);
        Assert.IsFalse(tp.IsSet(Opts.WriteEmptyProperties));
        Assert.IsTrue(tp.IsSet(Opts.WriteGroups));
        Assert.IsFalse(tp.IsSet(Opts.None));


        // Unset doppelt aufrufen:
        tp = tp.Unset(Opts.WriteEmptyProperties);
        Assert.IsFalse(tp.IsSet(Opts.WriteEmptyProperties));
        Assert.IsTrue(tp.IsSet(Opts.WriteGroups));
        Assert.IsFalse(tp.IsSet(Opts.None));


        // letztes Flag löschen
        tp = tp.Unset(Opts.WriteGroups);
        Assert.IsFalse(tp.IsSet(Opts.WriteGroups));
        Assert.IsTrue(tp.IsSet(Opts.None));


        // Unset auf None aufrufen:
        tp = tp.Unset(Opts.WriteGroups);

        Assert.IsFalse(tp.IsSet(Opts.WriteGroups));
        Assert.IsTrue(tp.IsSet(Opts.None));
    }
}