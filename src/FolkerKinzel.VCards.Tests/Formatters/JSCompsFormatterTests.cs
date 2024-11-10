using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Formatters.Tests;

[TestClass]
public class JSCompsFormatterTests
{
    [TestMethod]
    public void JSCompsFormatterTest1()
    {
        var prop = new NameProperty(NameBuilder.Create().AddGiven("Folker").Build());
        prop.Parameters.ComponentOrder = "a";

        Assert.IsFalse(JSCompsFormatter.TryFormat(prop, out _));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void JSCompsFormatterTest2()
        => _ = JSCompsFormatter.TryFormat(null!, out _);

    [TestMethod]
    public void JSCompsFormatterTest3()
    {
        var prop = new NameProperty(NameBuilder.Create().AddGiven("Folker").Build());
        prop.Parameters.ComponentOrder = ";1;s,\\;";

        Assert.IsTrue(JSCompsFormatter.TryFormat(prop, out string? formatted));
        Assert.AreEqual("Folker;", formatted);
    }
}
