using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class AddrExtensionTests
{
    [TestMethod()]
    public void AddressTypesTest()
    {
        Addr? tp = null;

        Assert.IsFalse(tp.IsSet(Addr.Dom));

        tp = tp.Set(Addr.Dom);
        Assert.IsTrue(tp.IsSet(Addr.Dom));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(Addr.Dom);
        Assert.IsTrue(tp.IsSet(Addr.Dom));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(Addr.Postal);
        Assert.IsTrue(tp.IsSet(Addr.Postal));
        Assert.IsTrue(tp.IsSet(Addr.Dom));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(Addr.Postal);
        Assert.IsFalse(tp.IsSet(Addr.Postal));
        Assert.IsTrue(tp.IsSet(Addr.Dom));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(Addr.Postal);
        Assert.IsFalse(tp.IsSet(Addr.Postal));
        Assert.IsTrue(tp.IsSet(Addr.Dom));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(Addr.Dom);
        Assert.IsFalse(tp.IsSet(Addr.Dom));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(Addr.Dom);

        Assert.IsFalse(tp.IsSet(Addr.Dom));
        Assert.IsFalse(tp.HasValue);
    }
}