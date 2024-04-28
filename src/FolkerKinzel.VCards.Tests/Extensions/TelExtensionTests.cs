
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class TelExtensionTests
{
    [TestMethod()]
    public void PhoneTypesTest()
    {
        Tel? tp = null;

        Assert.IsFalse(tp.IsSet(Tel.Cell));

        tp = tp.Set(Tel.Cell);
        Assert.IsTrue(tp.IsSet(Tel.Cell));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(Tel.Cell);
        Assert.IsTrue(tp.IsSet(Tel.Cell));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(Tel.Voice);
        Assert.IsTrue(tp.IsSet(Tel.Voice));
        Assert.IsTrue(tp.IsSet(Tel.Cell));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(Tel.Voice);
        Assert.IsFalse(tp.IsSet(Tel.Voice));
        Assert.IsTrue(tp.IsSet(Tel.Cell));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(Tel.Voice);
        Assert.IsFalse(tp.IsSet(Tel.Voice));
        Assert.IsTrue(tp.IsSet(Tel.Cell));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(Tel.Cell);
        Assert.IsFalse(tp.IsSet(Tel.Cell));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(Tel.Cell);

        Assert.IsFalse(tp.IsSet(Tel.Cell));
        Assert.IsFalse(tp.HasValue);
    }
}