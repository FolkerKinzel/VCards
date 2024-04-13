using System.Xml.Linq;
using FolkerKinzel.VCards.BuilderParts;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass()]
public class VCardBuilderTests
{
    [TestMethod()]
    public void CreateTest1()
    {
        var builder = VCardBuilder.Create();
        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(VCardBuilder));
        VCard vc = builder.VCard;
        Assert.IsNotNull(vc);
        Assert.IsInstanceOfType(vc, typeof(VCard));
        Assert.IsNotNull(vc.ID);
    }

    [TestMethod()]
    public void CreateTest2()
    {
        var builder = VCardBuilder.Create(setID: false);
        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(VCardBuilder));
        VCard vc = builder.VCard;
        Assert.IsNotNull(vc);
        Assert.IsInstanceOfType(vc, typeof(VCard));
        Assert.IsNull(vc.ID);
    }

    [TestMethod()]
    public void CreateTest3()
    {
        var vc = new VCard();
        var builder = VCardBuilder.Create(vc);
        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(VCardBuilder));
        Assert.AreSame(vc, builder.VCard);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateTest4() => _ = VCardBuilder.Create(null!);

    [TestMethod()]
    public void SetAccessTest1()
    {
        VCard vc = VCardBuilder.Create()
                               .Access.Set(Access.Private)
                               .VCard;
        Assert.IsNotNull (vc.Access);
        Assert.AreEqual(Access.Private, vc.Access.Value);
    }

    [TestMethod()]
    public void SetAccessTest2()
    {
        VCard vc = VCardBuilder.Create()
                               .Access.Set(Access.Private, vc => "group")
                               .VCard;
        Assert.IsNotNull(vc.Access);
        Assert.AreEqual(Access.Private, vc.Access.Value);
        Assert.AreEqual("group", vc.Access.Group);

        VCardBuilder.Create(vc).Access.Clear();

        Assert.IsNull(vc.Access);
    }

    [TestMethod]
    public void EditAccessTest1()
    {
        var builder = VCardBuilder.Create();
        var prop = new AccessProperty(Access.Public);
        builder.Access.Edit(p => { prop = p; return p; });
        Assert.IsNull(prop);
        builder.Access.Edit(p => new AccessProperty(Access.Private))
               .Access.Edit(p => { prop = p; return p; });
        Assert.IsNotNull(prop);

        builder.Access.Edit(x => null);
        Assert.IsNull(builder.VCard.Access);
    }

    [TestMethod()]
    public void AddAddressTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Addresses.Add("Elm Street", null, null, null,
                            autoLabel: false,
                            parameters: p => p.AddressType = Adr.Intl,
                            group: vc => "gr1"
                            )
            .Addresses.Add("3", null, null, null)
            .Addresses.Add("4", null, null, null)
            .Addresses.Add("Schlossallee", null, null, null,
                         pref: true,
                         parameters: p => p.AddressType = Adr.Dom)
            .VCard;

        Assert.IsNotNull(vc.Addresses);
        vc.Addresses = vc.Addresses.Append(null);

        AddressProperty prop1 = vc.Addresses!.First()!;
        AddressProperty prop2 = vc.Addresses!.ElementAt(1)!;

        Assert.AreEqual("Schlossallee", prop1.Value.Street[0]);
        Assert.AreEqual(1, prop1.Parameters.Preference);
        Assert.AreEqual(Adr.Dom, prop1.Parameters.AddressType);
        Assert.IsNotNull(prop1.Parameters.Label);

        Assert.AreEqual("Elm Street", prop2.Value.Street[0]);
        Assert.AreEqual(2, prop2.Parameters.Preference);
        Assert.AreEqual(Adr.Intl, prop2.Parameters.AddressType);
        Assert.IsNull(prop2.Parameters.Label);
        Assert.AreEqual("gr1", prop2.Group);

        Assert.IsTrue(vc.Addresses!.Any(x => x?.Value.Street[0] == "3"));
        VCardBuilder.Create(vc).Addresses.Remove(x => x.Value.Street[0] == "3");
        Assert.IsFalse(vc.Addresses!.Any(x => x?.Value.Street[0] == "3"));
        VCardBuilder.Create(vc)
                         .Addresses.Clear();
        Assert.IsNull(vc.Addresses);
    }

    [TestMethod()]
    public void AddAddressTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .Addresses.Add(Enumerable.Repeat("Elm Street", 1), null, null, null,
                           autoLabel: false,
                           parameters: p => p.AddressType = Adr.Intl,
                           group: vc => "gr1"
                         )
            .Addresses.Add(["2"], null, null, null, pref: true)
            .VCard;

        AddressProperty prop1 = vc.Addresses!.ElementAt(1)!;

        Assert.AreEqual("Elm Street", prop1.Value.Street[0]);
        Assert.AreEqual(2, prop1.Parameters.Preference);
        Assert.AreEqual(Adr.Intl, prop1.Parameters.AddressType);
        Assert.IsNull(prop1.Parameters.Label);
        Assert.AreEqual("gr1", prop1.Group);

        AddressProperty prop2 = vc.Addresses!.First()!;

        Assert.IsNotNull(prop2.Parameters.Label);
    }

    [TestMethod]
    public void EditAddressesTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<AddressProperty?>? prop = null;
        builder.Addresses.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.Addresses = new AddressProperty("Elmstreet", null, null, null).Append(null);
        builder.Addresses.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.Addresses.Edit(x => null);
        Assert.IsNull(builder.VCard.Addresses);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveAddressTest1()
    {
        _ = VCardBuilder.Create()
                        .Addresses.Remove((Func<AddressProperty?, bool>)null!)
                        .VCard;
    }

    [TestMethod()]
    public void AddAnniversaryViewTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .AnniversaryViews.Add(2023, 12, 6)
            .VCard;

        Assert.IsNotNull(vc.AnniversaryViews);
        Assert.AreEqual(1, vc.AnniversaryViews.Count());

    }

    [TestMethod()]
    public void AddBirthDayViewTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add(2023, 12, 6)
            .VCard;

        Assert.IsNotNull(vc.BirthDayViews);
        Assert.AreEqual(1, vc.BirthDayViews.Count());
    }

    [TestMethod()]
    public void AddBirthDayViewTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add(2023, 12, 6, p => p.Index = 1, v => "g")
            .BirthDayViews.Add(12,6)
            .BirthDayViews.Add(new DateTimeOffset(2023, 12, 6, 10, 0, 0, TimeSpan.Zero))
            .BirthDayViews.Add(new DateOnly(2023, 12, 6))
            .BirthDayViews.Add(new TimeOnly(10,0))
            .BirthDayViews.Add("Nicholas")
            .BirthDayViews.Add("Nikolaus", p => p.Language = "de-DE", v => "g")
            .VCard;

        Assert.IsNotNull(vc.BirthDayViews);
        Assert.AreEqual(7, vc.BirthDayViews.Count());

        VCardBuilder.Create(vc).BirthDayViews.Remove(p => p.Parameters.Language == "de-DE");

        Assert.IsNotNull(vc.BirthDayViews);
        Assert.AreEqual(6, vc.BirthDayViews.Count());

        VCardBuilder.Create(vc).BirthDayViews.Clear();
        Assert.IsNull(vc.BirthDayViews);
    }

    [TestMethod()]
    public void AddBirthDayViewTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthDayViews.Add(12, 6, p => p.Index = 1, v => "g")
            .BirthDayViews.Add(new DateTimeOffset(2023, 12, 6, 10, 0, 0, TimeSpan.Zero), p => p.Index = 2, v => "g")
            .BirthDayViews.Add(new DateOnly(2023, 12, 6), p => p.Index = 3, v => "g")
            .BirthDayViews.Add(new TimeOnly(10, 0), p => p.Index = 4, v => "g")
            .VCard;

        Assert.IsNotNull(vc.BirthDayViews);
        Assert.AreEqual(4, vc.BirthDayViews.Count());
        CollectionAssert.AllItemsAreNotNull(vc.BirthDayViews.ToArray());
        Assert.IsTrue(vc.BirthDayViews.All(x => x!.Group == "g"));
        Assert.IsTrue(vc.BirthDayViews.All(x => x!.Parameters.Index.HasValue));
    }

    [TestMethod]
    public void EditBirthDayViewsTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<DateAndOrTimeProperty?>? prop = null;
        builder.BirthDayViews.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.BirthDayViews = DateAndOrTimeProperty.FromDate(12, 24).Append(null);
        builder.BirthDayViews.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.BirthDayViews.Edit(x => null);
        Assert.IsNull(builder.VCard.BirthDayViews);
    }

    [TestMethod()]
    public void AddBirthPlaceViewTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .BirthPlaceViews.Add("1")
            .BirthPlaceViews.Add("one", parameters: p => p.Language = "en", group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.AreEqual(2, vc.BirthPlaceViews.Count());

        VCardBuilder.Create(vc).BirthPlaceViews.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.AreEqual(1, vc.BirthPlaceViews.Count());

        VCardBuilder.Create(vc).BirthPlaceViews.Clear();

        Assert.IsNull(vc.BirthPlaceViews);
    }

    [TestMethod]
    public void EditBirthPlaceViewTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<TextProperty?>? prop = null;
        builder.BirthPlaceViews.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.BirthPlaceViews = new TextProperty("Allentown").Append(null);
        builder.BirthPlaceViews.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.BirthPlaceViews.Edit(x => null);
        Assert.IsNull(builder.VCard.BirthPlaceViews);
    }

    [TestMethod()]
    public void AddCalendarAddressTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .CalendarAddresses.Add("1")
            .CalendarAddresses.Add("2", pref: true, p => p.Index = 1, v => "g")
            .VCard;

        Assert.IsNotNull(vc.CalendarAddresses);

        TextProperty? first = vc.CalendarAddresses.First();
        Assert.IsNotNull(first);
        Assert.AreEqual("2", first.Value);
        Assert.AreEqual("g", first.Group);
        Assert.AreEqual(1, first.Parameters.Index!.Value);

        VCardBuilder.Create(vc).CalendarAddresses.Remove(p => p.Value == "2");

        Assert.IsNotNull(vc.CalendarAddresses);
        Assert.AreEqual("1", vc.CalendarAddresses.First()!.Value);
        Assert.AreEqual(1, vc.CalendarAddresses.Count());

        VCardBuilder.Create(vc).CalendarAddresses.Clear();
        Assert.IsNull(vc.CalendarAddresses);
    }

    [TestMethod()]
    public void AddCalendarUserAddressTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .CalendarUserAddresses.Add("1")
            .VCard;

        Assert.IsNotNull(vc.CalendarUserAddresses);
        Assert.AreEqual(1, vc.CalendarUserAddresses.Count());
    }

    [TestMethod()]
    public void AddCategoryTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Categories.Add("1234", group: vc => "g", pref: true)
            .Categories.Add("qwertz")
            .VCard;

        Assert.IsNotNull(vc.Categories);
        Assert.AreEqual(2, vc.Categories.Count());
        Assert.AreEqual("g", vc.Categories.First()?.Group);

        VCardBuilder
            .Create(vc)
            .Categories.Remove(p => p.Parameters.Preference == 1);

        Assert.IsNotNull(vc.Categories);
        Assert.AreEqual(1, vc.Categories.Count());

        VCardBuilder
            .Create(vc)
            .Categories.Clear();

        Assert.IsNull(vc.Categories);
    }

    [TestMethod]
    public void AddCategoryTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .Categories.Add(["qwertz", "bla"])
            .Categories.Add(["1234"], pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Categories);
        Assert.AreEqual(2, vc.Categories.Count());
        Assert.AreEqual("g", vc.Categories.First()?.Group);
    }

    [TestMethod()]
    public void AddDeathDateViewTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .DeathDateViews.Add(2023, 12, 6)
            .VCard;

        Assert.IsNotNull(vc.DeathDateViews);
        Assert.AreEqual(1, vc.DeathDateViews.Count());
    }

    [TestMethod()]
    public void AddDeathPlaceViewTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .DeathPlaceViews.Add("1")
            .VCard;

        Assert.IsNotNull(vc.DeathPlaceViews);
        Assert.AreEqual(1, vc.DeathPlaceViews.Count());
    }

    [TestMethod()]
    public void SetDirectoryNameTest1()
    {
        VCard vc = VCardBuilder.Create()
                               .DirectoryName.Set("1")
                               .VCard;
        Assert.IsNotNull(vc.DirectoryName);
        Assert.AreEqual("1", vc.DirectoryName.Value);
    }

    [TestMethod()]
    public void SetDirectoryNameTest2()
    {
        VCard vc = VCardBuilder.Create()
                               .DirectoryName.Set("1", p => p.Context = "VCARD", vc => "group")
                               .VCard;
        Assert.IsNotNull(vc.DirectoryName);
        Assert.AreEqual("1", vc.DirectoryName.Value);
        Assert.AreEqual("group", vc.DirectoryName.Group);
        Assert.AreEqual("VCARD", vc.DirectoryName.Parameters.Context);

        VCardBuilder.Create(vc).DirectoryName.Clear();

        Assert.IsNull(vc.DirectoryName);
    }

    [TestMethod]
    public void EditDirectoryNameTest1()
    {
        var builder = VCardBuilder.Create();
        var prop = new TextProperty("My Homepage");
        builder.DirectoryName.Edit(p => prop = p);
        Assert.IsNull(prop);
        builder.DirectoryName.Set("Contoso's Website")
               .DirectoryName.Edit(p => prop = p);
        Assert.IsNotNull(prop);

        builder.DirectoryName.Edit(x => null);
        Assert.IsNull(builder.VCard.DirectoryName);
    }

    [TestMethod()]
    public void AddDisplayNameTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .DisplayNames.Add("1")
            .VCard;

        Assert.IsNotNull(vc.DisplayNames);
        Assert.AreEqual(1, vc.DisplayNames.Count());
    }

    [TestMethod()]
    public void AddEMailTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .EMails.Add("1")
            .VCard;

        Assert.IsNotNull(vc.EMails);
        Assert.AreEqual(1, vc.EMails.Count());
    }

    [TestMethod()]
    public void AddExpertiseTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Expertises.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Expertises);
        Assert.AreEqual(1, vc.Expertises.Count());
    }

    [TestMethod()]
    public void AddFreeOrBusyUrlTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .FreeOrBusyUrls.Add("1")
            .VCard;

        Assert.IsNotNull(vc.FreeOrBusyUrls);
        Assert.AreEqual(1, vc.FreeOrBusyUrls.Count());
    }

    [TestMethod()]
    public void AddGenderViewTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Add(Sex.NonOrNotApplicable)
            .GenderViews.Add(Sex.NonOrNotApplicable, "AI", parameters: p => p.Language = "en", group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.GenderViews);
        Assert.AreEqual(2, vc.GenderViews.Count());

        VCardBuilder
            .Create(vc)
            .GenderViews.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.GenderViews);
        Assert.AreEqual(1, vc.GenderViews.Count());

        VCardBuilder
            .Create(vc)
            .GenderViews.Clear();

        Assert.IsNull(vc.GenderViews);
    }

    [TestMethod]
    public void EditGenderViewTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<GenderProperty?>? prop = null;
        builder.GenderViews.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.GenderViews = new GenderProperty(Sex.Female).Append(null);
        builder.GenderViews.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.GenderViews.Edit(x => null);
        Assert.IsNull(builder.VCard.GenderViews);
    }

    [TestMethod()]
    public void AddGeoCoordinateTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .GeoCoordinates.Add(null)
            .VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(1, vc.GeoCoordinates.Count());

        VCardBuilder
            .Create(vc)
            .GeoCoordinates.Remove(p => true);

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(0, vc.GeoCoordinates.Count());

        VCardBuilder
            .Create(vc)
            .GeoCoordinates.Clear();

        Assert.IsNull(vc.GeoCoordinates);
    }

    [TestMethod]
    public void EditGeoCoordinateTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<GeoProperty?>? prop = null;
        builder.GeoCoordinates.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.GeoCoordinates = new GeoProperty(42,42).Append(null);
        builder.GeoCoordinates.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.GeoCoordinates.Edit(x => null);
        Assert.IsNull(builder.VCard.GeoCoordinates);
    }

    [TestMethod()]
    public void AddGeoCoordinateTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .GeoCoordinates.Add(null, group: v => "g1")
            .VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(1, vc.GeoCoordinates.Count());
        Assert.AreEqual("g1", vc.GeoCoordinates.First()!.Group);
    }

    [TestMethod()]
    public void AddGeoCoordinateTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .GeoCoordinates.Add(25, 25, group: v => "g1")
            .VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(1, vc.GeoCoordinates.Count());
        Assert.AreEqual("g1", vc.GeoCoordinates.First()!.Group);
    }

    [TestMethod()]
    public void AddGeoCoordinateTest4()
    {
        VCard vc = VCardBuilder
            .Create()
            .GeoCoordinates.Add(25, 25)
            .VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(1, vc.GeoCoordinates.Count());
        Assert.AreEqual(null, vc.GeoCoordinates.First()!.Group);
    }

    [TestMethod()]
    public void AddHobbyTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Hobbies.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Hobbies);
        Assert.AreEqual(1, vc.Hobbies.Count());
    }

    [TestMethod()]
    public void SetIDTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .ID.Set(Guid.NewGuid())
            .VCard;

        Assert.IsNotNull(vc.ID);

        VCardBuilder
            .Create(vc)
            .ID.Clear();

        Assert.IsNull(vc.ID);
    }

    [TestMethod()]
    public void SetIDTest2()
    {
        const string key = "X-Test";
        const string val = "bla";

        VCard vc = VCardBuilder
            .Create()
            .ID.Set(Guid.NewGuid(),
                    parameters: p => p.NonStandard = [new KeyValuePair<string, string>(key, val)],
                    group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.ID);
        Assert.AreEqual("g", vc.ID.Group);

        KeyValuePair<string, string> para = vc.ID.Parameters.NonStandard!.First();
        Assert.AreEqual(key, para.Key);
        Assert.AreEqual(val, para.Value);
    }

    [TestMethod()]
    public void SetIDTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .ID.Set()
            .VCard;

        Assert.IsNotNull(vc.ID);

        VCardBuilder
            .Create(vc)
            .ID.Clear();

        Assert.IsNull(vc.ID);
    }

    [TestMethod()]
    public void SetIDTest4()
    {
        const string key = "X-Test";
        const string val = "bla";

        VCard vc = VCardBuilder
            .Create()
            .ID.Set(
                    parameters: p => p.NonStandard = [new KeyValuePair<string, string>(key, val)],
                    group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.ID);
        Assert.AreEqual("g", vc.ID.Group);

        KeyValuePair<string, string> para = vc.ID.Parameters.NonStandard!.First();
        Assert.AreEqual(key, para.Key);
        Assert.AreEqual(val, para.Value);
    }

    [TestMethod]
    public void EditIDTest1()
    {
        var builder = VCardBuilder.Create(setID: false);
        var prop = new IDProperty();
        builder.ID.Edit(p => prop = p);
        Assert.IsNull(prop);
        builder.ID.Set()
               .ID.Edit(p => prop = p);
        Assert.IsNotNull(prop);

        builder.ID.Edit(x => null);
        Assert.IsNull(builder.VCard.ID);
    }


    [TestMethod()]
    public void AddMessengerTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Messengers.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Messengers);
        Assert.AreEqual(1, vc.Messengers.Count());
    }

    [TestMethod()]
    public void AddInterestTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Interests.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Interests);
        Assert.AreEqual(1, vc.Interests.Count());
    }

    [TestMethod()]
    public void AddKeyTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Keys.AddText("1234", group: vc => "g", pref: true)
            .Keys.AddText("qwertz")
            .VCard;

        Assert.IsNotNull(vc.Keys);
        Assert.AreEqual(2, vc.Keys.Count());
        Assert.AreEqual("g", vc.Keys.First()?.Group);

        VCardBuilder
            .Create(vc)
            .Keys.Remove(p => p.Parameters.Preference == 1);

        Assert.IsNotNull(vc.Keys);
        Assert.AreEqual(1, vc.Keys.Count());

        VCardBuilder
            .Create(vc)
            .Keys.Clear();

        Assert.IsNull(vc.Keys);
    }

    [TestMethod]
    public void EditKeysTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<DataProperty?>? prop = null;
        builder.Keys.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.Keys = DataProperty.FromText("Password").Append(null);
        builder.Keys.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.Keys.Edit(x => null);
        Assert.IsNull(builder.VCard.Keys);
    }

    
    [TestMethod()]
    public void SetKindTest1()
    {
        VCard vc = VCardBuilder.Create()
                               .Kind.Set(Kind.Group)
                               .VCard;
        Assert.IsNotNull(vc.Kind);
        Assert.AreEqual(Kind.Group, vc.Kind.Value);
    }

    [TestMethod()]
    public void SetKindTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .Kind.Set(Kind.Group, 
                      p => p.NonStandard = [ new KeyValuePair<string, string>("X-PARA", "bla")], vc => "group")
            .VCard;
        Assert.IsNotNull(vc.Kind);
        Assert.IsNotNull(vc.Kind.Parameters.NonStandard);
        Assert.AreEqual(Kind.Group, vc.Kind.Value);
        Assert.AreEqual("group", vc.Kind.Group);

        VCardBuilder.Create(vc).Kind.Clear();

        Assert.IsNull(vc.Kind);
    }

    [TestMethod]
    public void EditKindTest1()
    {
        var builder = VCardBuilder.Create();
        var prop = new KindProperty(Kind.Individual);
        builder.Kind.Edit(p => prop = p);
        Assert.IsNull(prop);
        builder.Kind.Set(Kind.Group)
               .Kind.Edit(p => prop = p);
        Assert.IsNotNull(prop);

        builder.Kind.Edit(x => null);
        Assert.IsNull(builder.VCard.Kind);
    }

    [TestMethod()]
    public void AddLanguageTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Languages.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Languages);
        Assert.AreEqual(1, vc.Languages.Count());
    }

    [TestMethod()]
    public void AddLogoTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Logos.AddFile(TestFiles.EmptyVcf)
            .Logos.AddFile(TestFiles.EmptyVcf, pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Logos);
        Assert.AreEqual(2, vc.Logos.Count());
        Assert.AreEqual("g", vc.Logos.First()?.Group);
    }

    [TestMethod()]
    public void AddLogoTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .Logos.AddUri(new Uri("https://api.nuget.org/v3-flatcontainer/folkerkinzel.vcards/6.1.0/icon"))
            .Logos.AddUri(new Uri("https://www.nuget.org/Content/gallery/img/logo-header.svg"), pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Logos);
        Assert.AreEqual(2, vc.Logos.Count());
        Assert.AreEqual("g", vc.Logos.First()?.Group);
    }

    [TestMethod()]
    public void SetMailerTest()
    {
        VCard vc = VCardBuilder.Create()
                               .Mailer.Set("1")
                               .VCard;
        Assert.IsNotNull(vc.Mailer);
        Assert.AreEqual("1", vc.Mailer.Value);
    }

    [TestMethod()]
    public void AddMemberTest()
        => Assert.IsInstanceOfType(VCardBuilder.Create().Members, typeof(RelationBuilder));

    [TestMethod()]
    public void AddNameViewTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(["Miller"], ["John"], null, null,
                            displayName: (b, p) => b.Add("John Miller")
,
                            parameters: p => p.Language = "en",
                            group: vc => "gr1")
            .NameViews.Add(["Müller"], ["Johann"], null, null,
                         parameters: p => p.Language = "de")
            .VCard;

        vc.NameViews = vc.NameViews.Append(null);

        NameProperty prop1 = vc.NameViews!.First()!;
        NameProperty prop2 = vc.NameViews!.ElementAt(1)!;

        Assert.IsNotNull(vc.NameViews?.FirstOrDefault());

        Assert.AreEqual("Miller", prop1.Value.FamilyNames[0]);
        Assert.AreEqual("en", prop1.Parameters.Language);
        Assert.AreEqual("gr1", prop1.Group);

        Assert.AreEqual("de", prop2.Parameters.Language);
        
        vc = VCardBuilder.Create(vc).NameViews.Remove(x => x.Parameters.Language == "de").VCard;
        Assert.IsFalse(vc.NameViews!.Any(x => x?.Parameters.Language == "de"));
        vc = VCardBuilder.Create(vc)
                         .NameViews.Clear()
                         .VCard;
        Assert.IsNull(vc.NameViews);
    }

    [TestMethod()]
    public void AddNameViewTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add("Miller", "John", null, null,
                            parameters: p => p.Language = "en",
                            group: vc => "gr1",
                            displayName: (b, p) => b.Add("John Miller")
                            )
            .NameViews.Add("Müller", "Johann", null, null,
                         parameters: p => p.Language = "de")
            .VCard;

        Assert.IsNotNull(vc.NameViews);
        vc.NameViews = vc.NameViews.Append(null);

        NameProperty prop1 = vc.NameViews!.First()!;
        NameProperty prop2 = vc.NameViews!.ElementAt(1)!;

        Assert.IsNotNull(vc.NameViews?.FirstOrDefault());

        Assert.AreEqual("Miller", prop1.Value.FamilyNames[0]);
        Assert.AreEqual("en", prop1.Parameters.Language);
        Assert.AreEqual("gr1", prop1.Group);

        Assert.AreEqual("de", prop2.Parameters.Language);
    }

    [TestMethod]
    public void EditNameViewTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<NameProperty?>? prop = null;
        builder.NameViews.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.NameViews = new NameProperty(null, "Heinz").Append(null);
        builder.NameViews.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.NameViews.Edit(x => null);
        Assert.IsNull(builder.VCard.NameViews);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveNameViewTest1()
    {
        _ = VCardBuilder
            .Create()
            .NameViews.Remove((Func<NameProperty?, bool>)null!)
            .VCard;
    }


    [TestMethod()]
    public void AddNickNameTest()
        => Assert.IsInstanceOfType(VCardBuilder.Create().NickNames, typeof(StringCollectionBuilder));

    [TestMethod]
    public void EditNickNameTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<StringCollectionProperty?>? prop = null;
        builder.NickNames.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.NickNames = new StringCollectionProperty(["Duffy", "Dumpfbacke"]).Append(null);
        builder.NickNames.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.NickNames.Edit(x => null);
        Assert.IsNull(builder.VCard.NickNames);
    }

    [TestMethod()]
    public void AddNonStandardTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .NonStandards.Add("X-TEST", "first")
            .NonStandards.Add("X-TEST", "second", group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.NonStandards);
        Assert.AreEqual(2, vc.NonStandards.Count());

        VCardBuilder
            .Create(vc)
            .NonStandards.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.NonStandards);
        Assert.AreEqual(1, vc.NonStandards.Count());

        VCardBuilder
            .Create(vc)
            .NonStandards.Clear();

        Assert.IsNull(vc.NonStandards);
    }

    [TestMethod]
    public void EditNonStandardTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<NonStandardProperty?>? prop = null;
        builder.NonStandards.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.NonStandards = new NonStandardProperty("X-TEST", "Heinz").Append(null);
        builder.NonStandards.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.NonStandards.Edit(x => null);
        Assert.IsNull(builder.VCard.NonStandards);
    }

    [TestMethod()]
    public void AddNoteTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Notes.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Notes);
        Assert.AreEqual(1, vc.Notes.Count());
    }

    [TestMethod]
    public void EditNoteTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<TextProperty?>? prop = null;
        builder.Notes.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.Notes = new TextProperty("First note.").Append(null);
        builder.Notes.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.Notes.Edit(x => null);
        Assert.IsNull(builder.VCard.Notes);
    }

    [TestMethod()]
    public void AddOrganizationTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Organizations.Add("Org1")
            .Organizations.Add("Org2", ["sub"], group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Organizations);
        Assert.AreEqual(2, vc.Organizations.Count());

        VCardBuilder
            .Create(vc)
            .Organizations.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.Organizations);
        Assert.AreEqual(1, vc.Organizations.Count());

        VCardBuilder
            .Create(vc)
            .Organizations.Clear();

        Assert.IsNull(vc.Organizations);
    }

    [TestMethod]
    public void EditOrganizationTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<OrgProperty?>? prop = null;
        builder.Organizations.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.Organizations = new OrgProperty("Contoso").Append(null);
        builder.Organizations.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.Organizations.Edit(x => null);
        Assert.IsNull(builder.VCard.Organizations);
    }

    [TestMethod()]
    public void AddOrgDirectoryTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .OrgDirectories.Add("1")
            .VCard;

        Assert.IsNotNull(vc.OrgDirectories);
        Assert.AreEqual(1, vc.OrgDirectories.Count());
    }

    [TestMethod()]
    public void AddPhoneTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Phones.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Phones);
        Assert.AreEqual(1, vc.Phones.Count());
    }

    [TestMethod()]
    public void AddPhotoTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Photos.AddBytes([1,2,3,4])
            .Photos.AddBytes([17,4], pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Photos);
        Assert.AreEqual(2, vc.Photos.Count());
        Assert.AreEqual("g", vc.Photos.First()?.Group);
    }

    [TestMethod()]
    public void SetProductIDTest()
    {
        VCard vc = VCardBuilder.Create()
                               .ProductID.Set("1")
                               .VCard;
        Assert.IsNotNull(vc.ProductID);
        Assert.AreEqual("1", vc.ProductID.Value);
    }

    [TestMethod()]
    public void SetProfileTest()
    {
        VCard vc = VCardBuilder.Create()
                               .Profile.Set(vc => "group")
                               .VCard;

        Assert.IsNotNull(vc.Profile);
        Assert.AreEqual("group", vc.Profile.Group);

        VCardBuilder
            .Create(vc)
            .Profile.Set();

        Assert.IsNull(vc.Profile.Group);
        VCardBuilder.Create(vc).Profile.Clear();
        Assert.IsNull(vc.Profile);
    }

    [TestMethod]
    public void EditProfileTest1()
    {
        var builder = VCardBuilder.Create();
        var prop = new ProfileProperty();
        builder.Profile.Edit(p => prop = p);
        Assert.IsNull(prop);
        builder.Profile.Set()
               .Profile.Edit(p => prop = p);
        Assert.IsNotNull(prop);

        builder.Profile.Edit(x => null);
        Assert.IsNull(builder.VCard.Profile);
    }

    [TestMethod()]
    public void AddRelationTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Add(Guid.NewGuid())
            .Relations.Add(Guid.NewGuid(), pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual("g", vc.Relations.First()?.Group);

        VCardBuilder
            .Create(vc)
            .Relations.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(1, vc.Relations.Count());

        VCardBuilder
            .Create(vc)
            .Relations.Clear();

        Assert.IsNull(vc.Relations);
    }

    [TestMethod()]
    public void AddRelationTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Add("Susi", Rel.Friend)
            .Relations.Add("Horst", Rel.Neighbor, pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual("g", vc.Relations.First()?.Group);
    }

    [TestMethod()]
    public void AddRelationTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Add(new Uri("http://www.Susi.de"), Rel.Friend)
            .Relations.Add(new Uri("http://www.Horst.com"), Rel.Neighbor, pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual("g", vc.Relations.First()?.Group);
    }

    [TestMethod()]
    public void AddRelationTest4()
    {
        VCard vc = VCardBuilder
            .Create()
            .Relations.Add(VCardBuilder.Create().DisplayNames.Add("Susi").VCard, Rel.Friend)
            .Relations.Add(VCardBuilder.Create().DisplayNames.Add("Horst").VCard, Rel.Neighbor, pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual("g", vc.Relations.First()?.Group);
    }

    [TestMethod]
    public void EditRelationTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<RelationProperty?>? prop = null;
        builder.Relations.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.Relations = RelationProperty.FromText("Susi", Rel.Friend).Append(null);
        builder.Relations.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.Relations.Edit(x => null);
        Assert.IsNull(builder.VCard.Relations);
    }

    [TestMethod()]
    public void AddRoleTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Roles.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Roles);
        Assert.AreEqual(1, vc.Roles.Count());
    }

    [TestMethod()]
    public void AddSoundTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Sounds.AddBytes([1, 2, 3, 4])
            .Sounds.AddBytes([17, 4], pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Sounds);
        Assert.AreEqual(2, vc.Sounds.Count());
        Assert.AreEqual("g", vc.Sounds.First()?.Group);
    }

    [TestMethod()]
    public void AddSourceTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Sources.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Sources);
        Assert.AreEqual(1, vc.Sources.Count());
    }

    [TestMethod()]
    public void SetTimeStampTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeStamp.Set(DateTimeOffset.UtcNow)
            .VCard;

        Assert.IsNotNull(vc.TimeStamp);

        VCardBuilder
            .Create(vc)
            .TimeStamp.Clear();

        Assert.IsNull(vc.TimeStamp);
    }

    [TestMethod()]
    public void SetTimeStampTest2()
    {
        const string key = "X-Test";
        const string val = "bla";

        VCard vc = VCardBuilder
            .Create()
            .TimeStamp.Set(DateTimeOffset.UtcNow,
                           parameters: p => p.NonStandard = [new KeyValuePair<string, string>(key, val)],
                           group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.TimeStamp);
        Assert.AreEqual("g", vc.TimeStamp.Group);

        KeyValuePair<string, string> para = vc.TimeStamp.Parameters.NonStandard!.First();
        Assert.AreEqual(key, para.Key);
        Assert.AreEqual(val, para.Value);

        VCardBuilder
            .Create(vc)
            .TimeStamp.Clear();

        Assert.IsNull(vc.TimeStamp);
    }

    [TestMethod()]
    public void SetTimeStampTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeStamp.Set()
            .VCard;

        Assert.IsNotNull(vc.TimeStamp);

        VCardBuilder
            .Create(vc)
            .TimeStamp.Clear();

        Assert.IsNull(vc.TimeStamp);
    }

    [TestMethod]
    public void EditTimeStampTest1()
    {
        var builder = VCardBuilder.Create();
        var prop = new TimeStampProperty();
        builder.TimeStamp.Edit(p => prop = p);
        Assert.IsNull(prop);
        builder.TimeStamp.Set()
               .TimeStamp.Edit(p => prop = p);
        Assert.IsNotNull(prop);

        builder.TimeStamp.Edit(x => null);
        Assert.IsNull(builder.VCard.TimeStamp);
    }

    [TestMethod()]
    public void SetTimeStampTest4()
    {
        const string key = "X-Test";
        const string val = "bla";

        VCard vc = VCardBuilder
            .Create()
            .TimeStamp.Set(parameters: p => p.NonStandard = [new KeyValuePair<string, string>(key, val)],
                           group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.TimeStamp);
        Assert.AreEqual("g", vc.TimeStamp.Group);

        KeyValuePair<string, string> para = vc.TimeStamp.Parameters.NonStandard!.First();
        Assert.AreEqual(key, para.Key);
        Assert.AreEqual(val, para.Value);

        VCardBuilder
            .Create(vc)
            .TimeStamp.Clear();

        Assert.IsNull(vc.TimeStamp);
    }

    [TestMethod()]
    public void AddTimeZoneTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Add((TimeZoneID?)null)
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(1, vc.TimeZones.Count());

        VCardBuilder
            .Create(vc)
            .TimeZones.Remove(p => true);

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(0, vc.TimeZones.Count());

        VCardBuilder
            .Create(vc)
            .TimeZones.Clear();

        Assert.IsNull(vc.TimeZones);
    }

    [TestMethod()]
    public void AddTimeZoneTest2()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Add((TimeZoneID?)null, group: v => "g1")
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(1, vc.TimeZones.Count());
        Assert.AreEqual("g1", vc.TimeZones.First()!.Group);
    }

    [TestMethod()]
    public void AddTimeZoneTest3()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Add("Europe/Berlin")
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(1, vc.TimeZones.Count());
    }

    [TestMethod()]
    public void AddTimeZoneTest4()
    {
        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Add("Europe/Berlin", group: v => "g1")
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(1, vc.TimeZones.Count());
        Assert.AreEqual("g1", vc.TimeZones.First()!.Group);
    }

    [TestMethod]
    public void EditTimeZoneTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<TimeZoneProperty?>? prop = null;
        builder.TimeZones.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.TimeZones = new TimeZoneProperty(TimeZoneID.Parse("Europe/Berlin")).Append(null);
        builder.TimeZones.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.TimeZones.Edit(x => null);
        Assert.IsNull(builder.VCard.TimeZones);
    }

    [TestMethod()]
    public void AddTitleTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Titles.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Titles);
        Assert.AreEqual(1, vc.Titles.Count());
    }

    [TestMethod()]
    public void AddUrlTest()
    {
        VCard vc = VCardBuilder
            .Create()
            .Urls.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Urls);
        Assert.AreEqual(1, vc.Urls.Count());
    }

    [TestMethod()]
    public void AddXmlTest()
    {
        XNamespace ns = "http://www.contoso.com";

        VCard vc = VCardBuilder
            .Create()
            .Xmls.Add(new XElement(ns + "Key1", "First"))
            .Xmls.Add(new XElement(ns + "Key2", "Second"), pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Xmls);
        Assert.AreEqual(2, vc.Xmls.Count());
        Assert.AreEqual("Second", XElement.Parse(vc.Xmls.First()!.Value!).Value);

        VCardBuilder
            .Create(vc)
            .Xmls.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.Xmls);
        Assert.AreEqual(1, vc.Xmls.Count());

        VCardBuilder
            .Create(vc)
            .Xmls.Clear();

        Assert.IsNull(vc.Xmls);
    }

    [TestMethod]
    public void EditXmlsTest1()
    {
        var builder = VCardBuilder.Create();
        IEnumerable<XmlProperty?>? prop = null;
        builder.Xmls.Edit(p => prop = p);
        Assert.IsNotNull(prop);
        Assert.IsFalse(prop.Any());
        builder.VCard.Xmls = ((IEnumerable<XmlProperty>)new XmlProperty((XElement?)null)).Append(null);
        builder.Xmls.Edit(p => prop = p);
        Assert.IsTrue(prop.Any());
        CollectionAssert.AllItemsAreNotNull(prop.ToArray());

        builder.Xmls.Edit(x => null);
        Assert.IsNull(builder.VCard.Xmls);
    }
}