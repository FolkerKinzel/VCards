using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Extensions.Tests;

[TestClass()]
public class PropertyClassTypesExtensionTests
{
    [TestMethod()]
    public void PropertyClassTypesTest()
    {
        PropertyClassTypes? tp = null;

        Assert.IsFalse(tp.IsSet(PropertyClassTypes.Home));

        tp = tp.Set(PropertyClassTypes.Home);
        Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(PropertyClassTypes.Home);
        Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(PropertyClassTypes.Work);
        Assert.IsTrue(tp.IsSet(PropertyClassTypes.Work));
        Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(PropertyClassTypes.Work);
        Assert.IsFalse(tp.IsSet(PropertyClassTypes.Work));
        Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(PropertyClassTypes.Work);
        Assert.IsFalse(tp.IsSet(PropertyClassTypes.Work));
        Assert.IsTrue(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(PropertyClassTypes.Home);
        Assert.IsFalse(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(PropertyClassTypes.Home);

        Assert.IsFalse(tp.IsSet(PropertyClassTypes.Home));
        Assert.IsFalse(tp.HasValue);
    }
}