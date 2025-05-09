﻿using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class KindBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new KindBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Kind.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new KindBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Kind.Edit(null!, true);

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Kind.Edit((p, d) => new KindProperty(d), Enums.Kind.Individual)
            .VCard;

        Assert.IsNotNull(vc.Kind);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetTest1() => new KindBuilder().Set(Enums.Kind.Individual);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new KindBuilder().Clear();

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new KindBuilder().Equals((KindBuilder?)null));

        var builder = new KindBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new KindBuilder().ToString());
}
