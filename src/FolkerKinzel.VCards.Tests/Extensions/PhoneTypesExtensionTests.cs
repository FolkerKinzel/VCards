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
public class PhoneTypesExtensionTests
{
    [TestMethod()]
    public void PhoneTypesTest()
    {
        PhoneTypes? tp = null;

        Assert.IsFalse(tp.IsSet(PhoneTypes.Cell));

        tp = tp.Set(PhoneTypes.Cell);
        Assert.IsTrue(tp.IsSet(PhoneTypes.Cell));
        Assert.IsTrue(tp.HasValue);

        // Set doppelt aufrufen
        tp = tp.Set(PhoneTypes.Cell);
        Assert.IsTrue(tp.IsSet(PhoneTypes.Cell));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Set(PhoneTypes.Voice);
        Assert.IsTrue(tp.IsSet(PhoneTypes.Voice));
        Assert.IsTrue(tp.IsSet(PhoneTypes.Cell));
        Assert.IsTrue(tp.HasValue);

        tp = tp.Unset(PhoneTypes.Voice);
        Assert.IsFalse(tp.IsSet(PhoneTypes.Voice));
        Assert.IsTrue(tp.IsSet(PhoneTypes.Cell));
        Assert.IsTrue(tp.HasValue);

        // Unset doppelt aufrufen:
        tp = tp.Unset(PhoneTypes.Voice);
        Assert.IsFalse(tp.IsSet(PhoneTypes.Voice));
        Assert.IsTrue(tp.IsSet(PhoneTypes.Cell));
        Assert.IsTrue(tp.HasValue);

        // letztes Flag löschen
        tp = tp.Unset(PhoneTypes.Cell);
        Assert.IsFalse(tp.IsSet(PhoneTypes.Cell));
        Assert.IsFalse(tp.HasValue);

        // Unset auf null aufrufen:
        tp = tp.Unset(PhoneTypes.Cell);

        Assert.IsFalse(tp.IsSet(PhoneTypes.Cell));
        Assert.IsFalse(tp.HasValue);
    }
}