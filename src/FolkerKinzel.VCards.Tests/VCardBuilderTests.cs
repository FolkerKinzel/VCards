using System.Xml.Linq;
using FolkerKinzel.VCards.BuilderParts;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Tests;

[TestClass()]
public class VCardBuilderTests
{
    [TestMethod()]
    public void CreateTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var vc = new VCard();
        var builder = VCardBuilder.Edit(vc);
        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(VCardBuilder));
        Assert.AreSame(vc, builder.VCard);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateTest4() => _ = VCardBuilder.Edit(null!);

    [TestMethod()]
    public void SetAccessTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Access.Set(Access.Private)
                               .VCard;
        Assert.IsNotNull (vc.Access);
        Assert.AreEqual(Access.Private, vc.Access.Value);
    }

    [TestMethod()]
    public void SetAccessTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Access.Set(Access.Private, vc => "group")
                               .VCard;
        Assert.IsNotNull(vc.Access);
        Assert.AreEqual(Access.Private, vc.Access.Value);
        Assert.AreEqual("group", vc.Access.Group);

        VCardBuilder.Edit(vc).Access.Clear();

        Assert.IsNull(vc.Access);
    }

    [TestMethod()]
    public void AddAddressTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .Addresses.Add("Elm Street", null, null, null,
                            autoLabel: false,
                            parameters: p => p.AddressType = Adr.Intl,
                            group: vc => "gr1"
                            )
            .Addresses.Add("Schlossallee", null, null, null,
                         pref: true, parameters: p => p.AddressType = Adr.Dom)
            .Addresses.Add("3", null, null, null)
            .Addresses.Add("4", null, null, null)
            .VCard;

        vc.Addresses = vc.Addresses.ConcatWith(null);

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
        VCardBuilder.Edit(vc).Addresses.Remove(x => x.Value.Street[0] == "3");
        Assert.IsFalse(vc.Addresses!.Any(x => x?.Value.Street[0] == "3"));
        VCardBuilder.Edit(vc)
                         .Addresses.Clear();
        Assert.IsNull(vc.Addresses);
    }

    [TestMethod()]
    public void AddAddressTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveAddressTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        _ = VCardBuilder.Create()
                               .Addresses.Remove((Func<AddressProperty?, bool>)null!)
                               .VCard;
    }

    [TestMethod()]
    public void AddAnniversaryViewTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        VCardBuilder.Edit(vc).BirthDayViews.Remove(p => p.Parameters.Language == "de-DE");

        Assert.IsNotNull(vc.BirthDayViews);
        Assert.AreEqual(6, vc.BirthDayViews.Count());

        VCardBuilder.Edit(vc).BirthDayViews.Clear();
        Assert.IsNull(vc.BirthDayViews);
    }

    [TestMethod()]
    public void AddBirthDayViewTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

    [TestMethod()]
    public void AddBirthPlaceViewTest()
    {
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .BirthPlaceViews.Add("1")
            .BirthPlaceViews.Add("one", parameters: p => p.Language = "en", group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.AreEqual(2, vc.BirthPlaceViews.Count());

        VCardBuilder.Edit(vc).BirthPlaceViews.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.BirthPlaceViews);
        Assert.AreEqual(1, vc.BirthPlaceViews.Count());

        VCardBuilder.Edit(vc).BirthPlaceViews.Clear();

        Assert.IsNull(vc.BirthPlaceViews);
    }

    [TestMethod()]
    public void AddCalendarAddressTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        VCardBuilder.Edit(vc).CalendarAddresses.Remove(p => p.Value == "2");

        Assert.IsNotNull(vc.CalendarAddresses);
        Assert.AreEqual("1", vc.CalendarAddresses.First()!.Value);
        Assert.AreEqual(1, vc.CalendarAddresses.Count());

        VCardBuilder.Edit(vc).CalendarAddresses.Clear();
        Assert.IsNull(vc.CalendarAddresses);
    }

    [TestMethod()]
    public void AddCalendarUserAddressTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .Categories.Add("qwertz")
            .Categories.Add("1234", pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Categories);
        Assert.AreEqual(2, vc.Categories.Count());
        Assert.AreEqual("g", vc.Categories.First()?.Group);

        VCardBuilder
            .Edit(vc)
            .Categories.Remove(p => p.Parameters.Preference == 1);

        Assert.IsNotNull(vc.Categories);
        Assert.AreEqual(1, vc.Categories.Count());

        VCardBuilder
            .Edit(vc)
            .Categories.Clear();

        Assert.IsNull(vc.Categories);
    }

    [TestMethod]
    public void AddCategoryTest2()
    {
        VCard.SyncTestReset();

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .DirectoryName.Set("1")
                               .VCard;
        Assert.IsNotNull(vc.DirectoryName);
        Assert.AreEqual("1", vc.DirectoryName.Value);
    }

    [TestMethod()]
    public void SetDirectoryNameTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .DirectoryName.Set("1", p => p.Context = "VCARD", vc => "group")
                               .VCard;
        Assert.IsNotNull(vc.DirectoryName);
        Assert.AreEqual("1", vc.DirectoryName.Value);
        Assert.AreEqual("group", vc.DirectoryName.Group);
        Assert.AreEqual("VCARD", vc.DirectoryName.Parameters.Context);

        VCardBuilder.Edit(vc).DirectoryName.Clear();

        Assert.IsNull(vc.DirectoryName);
    }

    [TestMethod()]
    public void AddDisplayNameTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .GenderViews.Add(Sex.NonOrNotApplicable)
            .GenderViews.Add(Sex.NonOrNotApplicable, "AI", parameters: p => p.Language = "en", group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.GenderViews);
        Assert.AreEqual(2, vc.GenderViews.Count());

        VCardBuilder
            .Edit(vc)
            .GenderViews.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.GenderViews);
        Assert.AreEqual(1, vc.GenderViews.Count());

        VCardBuilder
            .Edit(vc)
            .GenderViews.Clear();

        Assert.IsNull(vc.GenderViews);
    }

    [TestMethod()]
    public void AddGeoCoordinateTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .GeoCoordinates.Add(null)
            .VCard;

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(1, vc.GeoCoordinates.Count());

        VCardBuilder
            .Edit(vc)
            .GeoCoordinates.Remove(p => true);

        Assert.IsNotNull(vc.GeoCoordinates);
        Assert.AreEqual(0, vc.GeoCoordinates.Count());

        VCardBuilder
            .Edit(vc)
            .GeoCoordinates.Clear();

        Assert.IsNull(vc.GeoCoordinates);
    }

    [TestMethod()]
    public void AddGeoCoordinateTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .Hobbies.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Hobbies);
        Assert.AreEqual(1, vc.Hobbies.Count());
    }

    [TestMethod()]
    public void AddMessengerTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .Keys.AddText("qwertz")
            .Keys.AddText("1234", pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Keys);
        Assert.AreEqual(2, vc.Keys.Count());
        Assert.AreEqual("g", vc.Keys.First()?.Group);

        VCardBuilder
            .Edit(vc)
            .Keys.Remove(p => p.Parameters.Preference == 1);

        Assert.IsNotNull(vc.Keys);
        Assert.AreEqual(1, vc.Keys.Count());

        VCardBuilder
            .Edit(vc)
            .Keys.Clear();

        Assert.IsNull(vc.Keys);
    }

    [TestMethod()]
    public void SetIDTest1()
    {
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .ID.Set(Guid.NewGuid())
            .VCard;

        Assert.IsNotNull(vc.ID);

        VCardBuilder
            .Edit(vc)
            .ID.Clear();

        Assert.IsNull(vc.ID);
    }

    [TestMethod()]
    public void SetIDTest2()
    {
        VCard.SyncTestReset();

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
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .ID.Set()
            .VCard;

        Assert.IsNotNull(vc.ID);

        VCardBuilder
            .Edit(vc)
            .ID.Clear();

        Assert.IsNull(vc.ID);
    }

    [TestMethod()]
    public void SetIDTest4()
    {
        VCard.SyncTestReset();

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

    [TestMethod()]
    public void SetKindTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Kind.Set(Kind.Group)
                               .VCard;
        Assert.IsNotNull(vc.Kind);
        Assert.AreEqual(Kind.Group, vc.Kind.Value);
    }

    [TestMethod()]
    public void SetKindTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Kind.Set(Kind.Group, p => p.NonStandard = [ new KeyValuePair<string, string>("X-PARA", "bla")], vc => "group")
                               .VCard;
        Assert.IsNotNull(vc.Kind);
        Assert.IsNotNull(vc.Kind.Parameters.NonStandard);
        Assert.AreEqual(Kind.Group, vc.Kind.Value);
        Assert.AreEqual("group", vc.Kind.Group);

        VCardBuilder.Edit(vc).Kind.Clear();

        Assert.IsNull(vc.Kind);
    }

    [TestMethod()]
    public void AddLanguageTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Mailer.Set("1")
                               .VCard;
        Assert.IsNotNull(vc.Mailer);
        Assert.AreEqual("1", vc.Mailer.Value);
    }

    [TestMethod()]
    public void AddMemberTest()
    {
        VCard.SyncTestReset();
        Assert.IsInstanceOfType(VCardBuilder.Create().Members, typeof(RelationBuilder));
    }

    [TestMethod()]
    public void AddNameViewTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        vc.NameViews = vc.NameViews.ConcatWith(null);

        NameProperty prop1 = vc.NameViews!.First()!;
        NameProperty prop2 = vc.NameViews!.ElementAt(1)!;

        Assert.IsNotNull(vc.NameViews?.FirstOrDefault());

        Assert.AreEqual("Miller", prop1.Value.FamilyNames[0]);
        Assert.AreEqual("en", prop1.Parameters.Language);
        Assert.AreEqual("gr1", prop1.Group);

        Assert.AreEqual("de", prop2.Parameters.Language);
        
        vc = VCardBuilder.Edit(vc).NameViews.Remove(x => x.Parameters.Language == "de").VCard;
        Assert.IsFalse(vc.NameViews!.Any(x => x?.Parameters.Language == "de"));
        vc = VCardBuilder.Edit(vc)
                         .NameViews.Clear()
                         .VCard;
        Assert.IsNull(vc.NameViews);
    }

    [TestMethod()]
    public void AddNameViewTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        vc.NameViews = vc.NameViews.ConcatWith(null);

        NameProperty prop1 = vc.NameViews!.First()!;
        NameProperty prop2 = vc.NameViews!.ElementAt(1)!;

        Assert.IsNotNull(vc.NameViews?.FirstOrDefault());

        Assert.AreEqual("Miller", prop1.Value.FamilyNames[0]);
        Assert.AreEqual("en", prop1.Parameters.Language);
        Assert.AreEqual("gr1", prop1.Group);

        Assert.AreEqual("de", prop2.Parameters.Language);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveNameViewTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        _ = VCardBuilder
            .Create()
            .NameViews.Remove((Func<NameProperty?, bool>)null!)
            .VCard;
    }


    [TestMethod()]
    public void AddNickNameTest()
    {
        VCard.SyncTestReset();
        Assert.IsInstanceOfType(VCardBuilder.Create().NickNames, typeof(StringCollectionBuilder));
    }

    [TestMethod()]
    public void AddNonStandardTest()
    {
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .NonStandards.Add("X-TEST", "first")
            .NonStandards.Add("X-TEST", "second", group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.NonStandards);
        Assert.AreEqual(2, vc.NonStandards.Count());

        VCardBuilder
            .Edit(vc)
            .NonStandards.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.NonStandards);
        Assert.AreEqual(1, vc.NonStandards.Count());

        VCardBuilder
            .Edit(vc)
            .NonStandards.Clear();

        Assert.IsNull(vc.NonStandards);
    }

    [TestMethod()]
    public void AddNoteTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .Notes.Add("1")
            .VCard;

        Assert.IsNotNull(vc.Notes);
        Assert.AreEqual(1, vc.Notes.Count());
    }

    [TestMethod()]
    public void AddOrganizationTest()
    {
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .Organizations.Add("Org1")
            .Organizations.Add("Org2", ["sub"], group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Organizations);
        Assert.AreEqual(2, vc.Organizations.Count());

        VCardBuilder
            .Edit(vc)
            .Organizations.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.Organizations);
        Assert.AreEqual(1, vc.Organizations.Count());

        VCardBuilder
            .Edit(vc)
            .Organizations.Clear();

        Assert.IsNull(vc.Organizations);
    }

    [TestMethod()]
    public void AddOrgDirectoryTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .ProductID.Set("1")
                               .VCard;
        Assert.IsNotNull(vc.ProductID);
        Assert.AreEqual("1", vc.ProductID.Value);
    }

    [TestMethod()]
    public void SetProfileTest()
    {
        VCard.SyncTestReset();

        VCard vc = VCardBuilder.Create()
                               .Profile.Set(vc => "group")
                               .VCard;

        Assert.IsNotNull(vc.Profile);
        Assert.AreEqual("group", vc.Profile.Group);

        VCardBuilder
            .Edit(vc)
            .Profile.Set();

        Assert.IsNull(vc.Profile.Group);
        VCardBuilder.Edit(vc).Profile.Clear();
        Assert.IsNull(vc.Profile);
    }

    [TestMethod()]
    public void AddRelationTest1()
    {
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .Relations.Add(Guid.NewGuid())
            .Relations.Add(Guid.NewGuid(), pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual("g", vc.Relations.First()?.Group);

        VCardBuilder
            .Edit(vc)
            .Relations.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(1, vc.Relations.Count());

        VCardBuilder
            .Edit(vc)
            .Relations.Clear();

        Assert.IsNull(vc.Relations);
    }

    [TestMethod()]
    public void AddRelationTest2()
    {
        VCard.SyncTestReset();

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
        VCard.SyncTestReset();

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
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .Relations.Add(VCardBuilder.Create().DisplayNames.Add("Susi").VCard, Rel.Friend)
            .Relations.Add(VCardBuilder.Create().DisplayNames.Add("Horst").VCard, Rel.Neighbor, pref: true, group: vc => "g")
            .VCard;

        Assert.IsNotNull(vc.Relations);
        Assert.AreEqual(2, vc.Relations.Count());
        Assert.AreEqual("g", vc.Relations.First()?.Group);
    }

    [TestMethod()]
    public void AddRoleTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();

        VCard vc = VCardBuilder
            .Create()
            .TimeStamp.Set(DateTimeOffset.UtcNow)
            .VCard;

        Assert.IsNotNull(vc.TimeStamp);

        VCardBuilder
            .Edit(vc)
            .TimeStamp.Clear();

        Assert.IsNull(vc.TimeStamp);
    }

    [TestMethod()]
    public void SetTimeStampTest2()
    {
        VCard.SyncTestReset();

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
            .Edit(vc)
            .TimeStamp.Clear();

        Assert.IsNull(vc.TimeStamp);
    }

    [TestMethod()]
    public void AddTimeZoneTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Add(null)
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(1, vc.TimeZones.Count());

        VCardBuilder
            .Edit(vc)
            .TimeZones.Remove(p => true);

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(0, vc.TimeZones.Count());

        VCardBuilder
            .Edit(vc)
            .TimeZones.Clear();

        Assert.IsNull(vc.TimeZones);
    }

    [TestMethod()]
    public void AddTimeZoneTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder
            .Create()
            .TimeZones.Add(null, group: v => "g1")
            .VCard;

        Assert.IsNotNull(vc.TimeZones);
        Assert.AreEqual(1, vc.TimeZones.Count());
        Assert.AreEqual("g1", vc.TimeZones.First()!.Group);
    }

    [TestMethod()]
    public void AddTitleTest()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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
        VCard.SyncTestReset();

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
            .Edit(vc)
            .Xmls.Remove(p => p.Group == "g");

        Assert.IsNotNull(vc.Xmls);
        Assert.AreEqual(1, vc.Xmls.Count());

        VCardBuilder
            .Edit(vc)
            .Xmls.Clear();

        Assert.IsNull(vc.Xmls);
    }
}