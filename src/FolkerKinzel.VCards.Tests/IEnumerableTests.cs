﻿using System.Collections;
using System.Xml.Linq;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class IEnumerableTests
{
    [TestMethod]
    public void IEnumerableTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        XNamespace f = "f";

        var vc = new VCard
        {
            Xmls = new TextProperty(new XElement(f + "Test").ToString())
        };

        IEnumerable enumerable = vc.Xmls;

        foreach (object? item in enumerable)
        {
            Assert.IsNotNull(item);
        }

        Assert.IsNotNull(vc.Xmls.FirstOrDefault());
    }


    [TestMethod]
    public void IEnumerableTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var vc = new VCard
        {
            BirthPlaceViews = new TextProperty("Lummerland")
        };

        IEnumerable enumerable = vc.BirthPlaceViews;

        foreach (object? item in enumerable)
        {
            Assert.IsNotNull(item);
        }

        Assert.IsNotNull(vc.BirthPlaceViews.FirstOrDefault());
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnötige Zuweisung eines Werts.", Justification = "<Ausstehend>")]
    public void IEnumerableTest3()
    {
        var vc = new VCard
        {
            BirthDayViews = new DateAndOrTimeProperty(DateTime.Now)
        };

        IEnumerable enumerable = vc.BirthDayViews;

        foreach (object? item in enumerable)
        {
            Assert.IsNotNull(item);
        }

        IEnumerable<VCardProperty?> props = vc.BirthDayViews;
        Assert.IsNotNull(vc.BirthDayViews.FirstOrDefault());
    }
}
