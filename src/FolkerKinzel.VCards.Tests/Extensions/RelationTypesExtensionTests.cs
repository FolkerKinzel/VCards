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
public class RelationTypesExtensionTests
{
    [TestMethod()]
    public void RelationTypesTest()
    {
        RelationTypes? tp = null;

        Assert.IsFalse(tp.IsSet(RelationTypes.Agent));

        tp = tp.Set(RelationTypes.Agent);
        Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(RelationTypes.Agent);
        Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(RelationTypes.Contact);
        Assert.IsTrue(tp.IsSet(RelationTypes.Contact));
        Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(RelationTypes.Contact);
        Assert.IsFalse(tp.IsSet(RelationTypes.Contact));
        Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(RelationTypes.Contact);
        Assert.IsFalse(tp.IsSet(RelationTypes.Contact));
        Assert.IsTrue(tp.IsSet(RelationTypes.Agent));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(RelationTypes.Agent);
        Assert.IsFalse(tp.IsSet(RelationTypes.Agent));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(RelationTypes.Agent);

        Assert.IsFalse(tp.IsSet(RelationTypes.Agent));
        Assert.IsFalse(tp.HasValue);
    }

}