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
        VCard vc = builder.Build();
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
        VCard vc = builder.Build();
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
        var builder = VCardBuilder.Create(vc);
        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(VCardBuilder));
        Assert.AreSame(vc, builder.Build());
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateTest4() => _ = VCardBuilder.Create(null!);

    [TestMethod()]
    public void SetAccessTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Access.Set(Access.Private)
                               .Build();
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
                               .Build();
        Assert.IsNotNull(vc.Access);
        Assert.AreEqual(Access.Private, vc.Access.Value);
        Assert.AreEqual("group", vc.Access.Group);

        VCardBuilder.Create(vc).Access.Clear();

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
            .Build();

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
        vc = VCardBuilder.Create(vc).Addresses.Remove(x => x.Value.Street[0] == "3").Build();
        Assert.IsFalse(vc.Addresses!.Any(x => x?.Value.Street[0] == "3"));
        vc = VCardBuilder.Create(vc)
                         .Addresses.Clear()
                         .Build();
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
            .Build();

        AddressProperty prop2 = vc.Addresses!.First()!;

        Assert.AreEqual("Elm Street", prop2.Value.Street[0]);
        Assert.AreEqual(100, prop2.Parameters.Preference);
        Assert.AreEqual(Adr.Intl, prop2.Parameters.AddressType);
        Assert.IsNull(prop2.Parameters.Label);
        Assert.AreEqual("gr1", prop2.Group);

    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveAddressTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        _ = VCardBuilder.Create()
                               .Addresses.Remove((Func<AddressProperty?, bool>)null!)
                               .Build();
    }

    [TestMethod()]
    public void AddAnniversaryViewTest()
    {

    }

    [TestMethod()]
    public void AddBirthDayViewTest()
    {

    }

    [TestMethod()]
    public void AddBirthPlaceViewTest()
    {

    }

    [TestMethod()]
    public void AddCalendarAddressTest()
    {

    }

    [TestMethod()]
    public void AddCalendarUserAddressTest()
    {

    }

    [TestMethod()]
    public void AddCategoryTest()
    {

    }

    [TestMethod()]
    public void AddDeathDateViewTest()
    {

    }

    [TestMethod()]
    public void AddDeathPlaceViewTest()
    {

    }

    [TestMethod()]
    public void SetDirectoryNameTest()
    {

    }

    [TestMethod()]
    public void AddDisplayNameTest()
    {

    }

    [TestMethod()]
    public void AddEMailTest()
    {

    }

    [TestMethod()]
    public void AddExpertiseTest()
    {

    }

    [TestMethod()]
    public void AddFreeOrBusyUrlTest()
    {

    }

    [TestMethod()]
    public void AddGenderViewTest()
    {

    }

    [TestMethod()]
    public void AddGeoCoordinateTest()
    {

    }

    [TestMethod()]
    public void AddHobbyTest()
    {

    }

    [TestMethod()]
    public void AddMessengerTest()
    {

    }

    [TestMethod()]
    public void AddInterestTest()
    {

    }

    [TestMethod()]
    public void AddKeyTest()
    {

    }

    [TestMethod()]
    public void SetKindTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        VCard vc = VCardBuilder.Create()
                               .Kind.Set(Kind.Group)
                               .Build();
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
                               .Build();
        Assert.IsNotNull(vc.Kind);
        Assert.IsNotNull(vc.Kind.Parameters.NonStandard);
        Assert.AreEqual(Kind.Group, vc.Kind.Value);
        Assert.AreEqual("group", vc.Kind.Group);

        VCardBuilder.Create(vc).Kind.Clear();

        Assert.IsNull(vc.Kind);
    }

    [TestMethod()]
    public void AddLanguageTest()
    {

    }

    [TestMethod()]
    public void AddLogoTest()
    {

    }

    [TestMethod()]
    public void SetMailerTest()
    {

    }

    [TestMethod()]
    public void AddMemberTest()
    {

    }

    [TestMethod()]
    public void AddNameViewTest()
    {

    }

    [TestMethod()]
    public void AddNickNameTest()
    {

    }

    [TestMethod()]
    public void AddNonStandardTest()
    {

    }

    [TestMethod()]
    public void AddNoteTest()
    {

    }

    [TestMethod()]
    public void AddOrganizationTest()
    {

    }

    [TestMethod()]
    public void AddOrgDirectoryTest()
    {

    }

    [TestMethod()]
    public void AddPhoneTest()
    {

    }

    [TestMethod()]
    public void AddPhotoTest()
    {

    }

    [TestMethod()]
    public void SetProdIDTest()
    {

    }

    [TestMethod()]
    public void SetProfileTest()
    {

    }

    [TestMethod()]
    public void AddRelationTest()
    {

    }

    [TestMethod()]
    public void AddRoleTest()
    {

    }

    [TestMethod()]
    public void AddSoundTest()
    {

    }

    [TestMethod()]
    public void AddSourceTest()
    {

    }

    [TestMethod()]
    public void SetTimeStampTest()
    {

    }

    [TestMethod()]
    public void AddTimeZoneTest()
    {

    }

    [TestMethod()]
    public void AddTitleTest()
    {

    }

    [TestMethod()]
    public void SetUniqueIdentifierTest()
    {

    }

    [TestMethod()]
    public void AddUrlTest()
    {

    }

    [TestMethod()]
    public void AddXmlTest()
    {

    }
}