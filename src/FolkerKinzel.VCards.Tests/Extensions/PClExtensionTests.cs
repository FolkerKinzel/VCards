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
public class PClExtensionTests
{
    [TestMethod()]
    public void PropertyClassTypesTest()
    {
        PCl? tp = null;

        Assert.IsFalse(tp.IsSet(PCl.Home));

        tp = tp.Set(PCl.Home);
        Assert.IsTrue(tp.IsSet(PCl.Home));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(PCl.Home);
        Assert.IsTrue(tp.IsSet(PCl.Home));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(PCl.Work);
        Assert.IsTrue(tp.IsSet(PCl.Work));
        Assert.IsTrue(tp.IsSet(PCl.Home));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(PCl.Work);
        Assert.IsFalse(tp.IsSet(PCl.Work));
        Assert.IsTrue(tp.IsSet(PCl.Home));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(PCl.Work);
        Assert.IsFalse(tp.IsSet(PCl.Work));
        Assert.IsTrue(tp.IsSet(PCl.Home));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(PCl.Home);
        Assert.IsFalse(tp.IsSet(PCl.Home));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(PCl.Home);

        Assert.IsFalse(tp.IsSet(PCl.Home));
        Assert.IsFalse(tp.HasValue);
    }
}