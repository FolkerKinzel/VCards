﻿using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class NameViewsBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new NameViewsBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .NameViews.Add((Name?)null)
            .NameViews.Add(NameBuilder.Create().AddSurname("Miller").Build())
            .NameViews.SetIndexes();

        VCard vc = builder.VCard;

        IEnumerable<NameProperty?>? property = vc.NameViews;

        Assert.IsNotNull(property);
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.NameViews.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.NameViews.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new NameViewsBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new NameViewsBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().NameViews.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new NameViewsBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().NameViews.Edit(null!, true);

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Edit((props, bl) => new NameProperty(NameBuilder.Create().Build()), true)
            .VCard;

        Assert.IsNotNull(vc.NameViews);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest3() => new NameViewsBuilder().Add(FolkerKinzel.VCards.NameBuilder.Create().Build());

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new NameViewsBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new NameViewsBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new NameViewsBuilder().Equals((NameViewsBuilder?)null));

        var builder = new NameViewsBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new NameViewsBuilder().ToString());

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ToDisplayNamesTest1() => new NameViewsBuilder().ToDisplayNames(NameFormatter.Default);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ToDisplayNamesTest2() => VCardBuilder.Create().NameViews.ToDisplayNames(null!);

    [TestMethod]
    public void ToDisplayNamesTest3()
    {
        VCardBuilder
            .Create().NameViews.ToDisplayNames(NameFormatter.Default);
    }

    [TestMethod]
    public void ToDisplayNamesTest4()
    {
        VCard vc = VCardBuilder
            .Create()
            .DisplayNames.Add("Sven")
            .DisplayNames.Edit(props => props.Append(null))
            .NameViews.Add(NameBuilder.Create().AddGiven("Ulf").Build())
            .NameViews.Add((Name?)null, p => { p.Language = "de-DE"; p.Preference = 1; }, v => "1")
            .NameViews.Add(NameBuilder.Create().AddGiven("Detlef").Build(), p => { p.Language = "de-DE"; p.Preference = 3; }, v => "3")
            .NameViews.Add(NameBuilder.Create().AddGiven("Folker").Build(), p => { p.Language = "de-DE"; p.Preference = 2; }, v => "2")
            .NameViews.Add(NameBuilder.Create().AddGiven("Susi").Build(), p => { p.Language = "en-US"; p.Preference = 4; }, v => "4")
            .NameViews.Edit(props => props.Append(null))
            .NameViews.ToDisplayNames(NameFormatter.Default)
            .VCard;

        StringComparer comp = StringComparer.Ordinal;

        IEnumerable<TextProperty?>? dn = vc.DisplayNames;
        Assert.IsNotNull(dn);
        Assert.AreEqual(4, dn.Count());
        CollectionAssert.Contains(dn.ToArray(), null);
        Assert.IsTrue(dn.Any(x => comp.Equals("Sven", x?.Value)));
        Assert.IsTrue(dn.Any(x => comp.Equals("Folker", x?.Value) && comp.Equals("2", x.Group) && comp.Equals("de-DE", x.Parameters.Language) && x.Parameters.Derived));
        Assert.IsTrue(dn.Any(x => comp.Equals("Susi", x?.Value) && comp.Equals("4", x.Group) && comp.Equals("en-US", x.Parameters.Language) && x.Parameters.Derived));
    }
}
