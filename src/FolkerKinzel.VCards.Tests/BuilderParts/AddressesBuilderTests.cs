using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.BuilderParts.Tests;

[TestClass]
public class AddressesBuilderTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPreferencesTest1() => new AddressesBuilder().SetPreferences();

    [TestMethod]
    public void SetPreferencesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Addresses.Add((Address?)null)
            .Addresses.Add(AddressBuilder.Create().AddLocality("New York").Build())
            .Addresses.SetPreferences();

        VCard vc = builder.VCard;

        Assert.IsNotNull(vc.Addresses);
        Assert.AreEqual(2, vc.Addresses.Count());
        Assert.AreEqual(100, vc.Addresses.First()!.Parameters.Preference);
        Assert.AreEqual(1, vc.Addresses.ElementAt(1)!.Parameters.Preference);

        builder.Addresses.SetPreferences(skipEmptyItems: false);
        Assert.AreEqual(1, vc.Addresses.First()!.Parameters.Preference);
        Assert.AreEqual(2, vc.Addresses.ElementAt(1)!.Parameters.Preference);

        builder.Addresses.UnsetPreferences();
        Assert.IsTrue(vc.Addresses.All(x => x!.Parameters.Preference == 100));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetPreferencesTest1() => new AddressesBuilder().UnsetPreferences();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetIndexesTest1() => new AddressesBuilder().SetIndexes();

    [TestMethod]
    public void SetIndexesTest2()
    {
        VCardBuilder builder = VCardBuilder
            .Create()
            .Addresses.Add((Address?)null)
            .Addresses.Add(AddressBuilder.Create().AddLocality("New York").Build())
            .Addresses.SetIndexes();

        VCard vc = builder.VCard;

        IEnumerable<AddressProperty?>? property = vc.Addresses;

        Assert.IsNotNull(property);
        Assert.AreEqual(2, property.Count());
        Assert.AreEqual(null, property.First()!.Parameters.Index);
        Assert.AreEqual(1, property.ElementAt(1)!.Parameters.Index);

        builder.Addresses.SetIndexes(skipEmptyItems: false);
        Assert.AreEqual(1, property.First()!.Parameters.Index);
        Assert.AreEqual(2, property.ElementAt(1)!.Parameters.Index);

        builder.Addresses.UnsetIndexes();
        Assert.IsTrue(property.All(x => x!.Parameters.Index == null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UnsetIndexesTest1() => new AddressesBuilder().UnsetIndexes();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest1() => new AddressesBuilder().Edit(p => p);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest2() => VCardBuilder.Create().Addresses.Edit(null!);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EditTest3() => new AddressesBuilder().Edit((p, d) => p, true);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EditTest4() => VCardBuilder.Create().Addresses.Edit(null!, true);

    [TestMethod]
    public void EditTest5()
    {
        VCard vc = VCardBuilder
            .Create()
            .Addresses.Edit((props, bl) => new AddressProperty(AddressBuilder.Create().Build()), true)
            .VCard;

        Assert.IsNotNull(vc.Addresses);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest1() => new AddressesBuilder().Add((Address?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest2() => new AddressesBuilder().Add((Address?)null);

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddTest3() => new AddressesBuilder().Add(AddressBuilder.Create().Build());

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ClearTest1() => new AddressesBuilder().Clear();

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveTest1() => new AddressesBuilder().Remove(p => true);

    [TestMethod]
    public void EqualsTest1()
    {
        Assert.IsFalse(new AddressesBuilder().Equals((AddressesBuilder?)null));

        var builder = new AddressesBuilder();
        Assert.AreEqual(builder.GetHashCode(), ((object)builder).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new AddressesBuilder().ToString());

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AttachLabelsTest1() => new AddressesBuilder().AttachLabels(AddressFormatter.Default);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AttachLabelsTest2() => VCardBuilder.Create().Addresses.AttachLabels(null!);

    [TestMethod]
    public void AttachLabelsTest3()
    {
        VCardBuilder
            .Create().Addresses.AttachLabels(AddressFormatter.Default);
    }

    [TestMethod]
    public void AttachLabelsTest4()
    {
        VCard vc = VCardBuilder
            .Create()
            .Addresses.Add(VCards.AddressBuilder.Create().AddLocality("London").Build())
            .Addresses.Add(VCards.AddressBuilder.Create().AddLocality("New York").Build(), p => p.Label = "Borna")
            .Addresses.Edit(props => props.Append(null))
            .Addresses.AttachLabels(AddressFormatter.Default)
            .VCard;

        IEnumerable<AddressProperty?>? adr = vc.Addresses;
        Assert.IsNotNull(adr);
        Assert.AreEqual(3, adr.Count());
        Assert.IsTrue(adr.Any(x => StringComparer.Ordinal.Equals("London", x?.Value.Locality.First()) &&
                                   (x.Parameters.Label?.Contains("London", StringComparison.Ordinal) ?? false)));
        Assert.IsTrue(adr.Any(x => StringComparer.Ordinal.Equals("New York", x?.Value.Locality.First()) &&
                                   (x.Parameters.Label?.Contains("Borna", StringComparison.Ordinal) ?? false)));
    }
}
