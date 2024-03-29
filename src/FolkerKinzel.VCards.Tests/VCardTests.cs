﻿using System.Diagnostics.CodeAnalysis;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class VCardTests
{
    [NotNull]
    public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext? TestContext { get; set; }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadTest_fileNameNull() => _ = VCard.LoadVcf(null!);

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void LoadTest_invalidFileName() => _ = VCard.LoadVcf("  ");

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ParseTest_contentNull() => _ = VCard.ParseVcf(null!);

    [TestMethod]
    public void ParseTest_contentEmpty()
    {
        IList<VCard> list = VCard.ParseVcf("");
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void ParseTest1()
    {
        IList<VCard> list = VCard.ParseVcf("BEGIN:VCARD\r\nFN:Folker\r\nEND:VCARD");
        Assert.AreEqual(1, list.Count);

        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SaveTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        string path = Path.Combine(TestContext.TestRunResultsDirectory!, $"SaveTest_{version}.vcf");

        vcard.SaveVcf(path, version);

        IList<VCard> list = VCard.LoadVcf(path);

        Assert.AreEqual(1, list.Count);
        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
        Assert.AreEqual(version, list[0].Version);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveTest_fileNameNull()
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        vcard.SaveVcf(null!);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SaveTest_InvalidFileName()
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        vcard.SaveVcf("   ");
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SerializeTest_StreamNull(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        vcard.SerializeVcf(null!, version);
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SerializeTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        using var ms = new MemoryStream();

        vcard.SerializeVcf(ms, version, leaveStreamOpen: true);

        Assert.AreNotEqual(0, ms.Length);

        ms.Position = 0;

        Assert.AreNotEqual(-1, ms.ReadByte());
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SerializeTest_CloseStream(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        using var ms = new MemoryStream();

        vcard.SerializeVcf(ms, version, leaveStreamOpen: false);

        _ = ms.Length;
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
    public void SerializeTest_StreamClosed(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        using var ms = new MemoryStream();
        ms.Close();

        vcard.SerializeVcf(ms, version);
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void DeserializeTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        using var ms = new MemoryStream();

        vcard.SerializeVcf(ms, version, leaveStreamOpen: true);
        ms.Position = 0;

        using var reader = new StreamReader(ms);
        IList<VCard> list = VCard.DeserializeVcf(reader);

        Assert.AreEqual(1, list.Count);
        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
        Assert.AreEqual(version, list[0].Version);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeTest_readerNull() => _ = VCard.DeserializeVcf(null!);


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void ToVcfStringTest1(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        string s = vcard.ToVcfString(version);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.AreEqual(1, list.Count);
        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
        Assert.AreEqual(version, list[0].Version);
    }


    [DataTestMethod]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ToVcfStringTest_vcardListNull1(VCdVersion version) => _ = VCard.ToVcfString(null!, version);


    [TestMethod]
    public void ToStringTest1()
    {
        var textProp = new TextProperty("Test");

        var pidMap1 = new PropertyIDMapping(5, new Uri("http://folkerkinzel.de/file1.htm"));
        var pidMap2 = new PropertyIDMapping(8, new Uri("http://folkerkinzel.de/file2.htm"));
        textProp.Parameters.PropertyIDs = new PropertyID[] { new PropertyID(1, pidMap1), new PropertyID(7), new PropertyID(1, pidMap2) };

        var vc = new VCard()
        {
            DisplayNames = new TextProperty?[]
            {
                    null,
                    textProp
            }
        };

        string s = vc.ToString();

        Assert.IsNotNull(s);
        Assert.IsFalse(string.IsNullOrWhiteSpace(s));
    }


    [TestMethod]
    public void GetEnumeratorTest1()
    {
        var vc = new VCard
        {
            DisplayNames = new TextProperty("Folker")
        };

        System.Collections.IEnumerable enumerable = vc;

        foreach (object? _ in enumerable)
        {
            return;
        }

        Assert.Fail();
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SeralizeVcfTest1()
    {
        using var stream = new MemoryStream();
        VCard.SerializeVcf(stream, null!);
    }

    [TestMethod]
    public void DeserializeTest2()
    {
        var stream = new FailStream(new ArgumentOutOfRangeException());
        using var reader = new StreamReader(stream);
        IList<VCard> vCards = VCard.DeserializeVcf(reader);
        Assert.IsNotNull(vCards);
    }

    [TestMethod]
    public void DeserializeTest3()
    {
        var stream = new FailStream(new OutOfMemoryException());
        using var reader = new StreamReader(stream);
        IList<VCard> vCards = VCard.DeserializeVcf(reader);
        Assert.IsNotNull(vCards);
    }

    [TestMethod]
    public void LoadCropped_2_1Test()
    {
        IList<VCard> vCards = VCard.LoadVcf(TestFiles.Cropped_2_1vcf);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
    }

    [TestMethod]
    public void IsEmptyTest1()
    {
        var vc = new VCard();
        Assert.IsTrue(vc.IsEmpty());

        var adr = new AddressProperty("  ", null, null, postalCode: "", autoLabel: false);
        adr.Parameters.Label = "  ";

        vc.Addresses = new AddressProperty?[] { null, adr };
        Assert.IsTrue(vc.IsEmpty());

        vc.BirthDayViews = DateAndOrTimeProperty.FromText(null);
        Assert.IsTrue(vc.IsEmpty());

        var text = new TextProperty("  ");
        var names = new TextProperty?[] { null, text, new TextProperty(null) };
        vc.DisplayNames = names;
        Assert.IsTrue(vc.IsEmpty());

        adr.Parameters.Label = "Label";
        Assert.IsFalse(vc.IsEmpty());

        adr.Parameters.Label = null;
        Assert.IsTrue(vc.IsEmpty());

        text.Parameters.Label = "Label";
        Assert.IsTrue(vc.IsEmpty());


        names[0] = new TextProperty("Name");
        Assert.IsFalse(vc.IsEmpty());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DereferenceTest1() => _ = FolkerKinzel.VCards.VCard.Dereference(null!).Count();

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ReferenceTest1() => _ = FolkerKinzel.VCards.VCard.Reference(null!);

    [TestMethod]
    public void GroupsTest1()
    {
        var vc = new VCard();

        IEnumerable<string> groups = vc.GroupIDs;
        Assert.IsFalse(groups.Any());

        vc.DisplayNames = new TextProperty("Donald", " 4 1 ");
        vc.TimeStamp = new TimeStampProperty();
        vc.Addresses = new AddressProperty?[] {
                                              null,
                                              new AddressProperty("1", null, null, null, group: " g r 1 "), 
                                              new AddressProperty("2", null, null, null, group: "41") 
                                              };
        vc.GeoCoordinates = new GeoProperty?[]{ 
                                               new GeoProperty(new GeoCoordinate(1, 1), group: "GR1"),
                                               null,
                                               new GeoProperty(new GeoCoordinate(2, 2))
                                             };
        Assert.AreEqual(2, vc.GroupIDs.Count());
        Assert.AreEqual("42", vc.NewGroup());
    }


    //[TestMethod]
    //public void EqualsTest1()
    //{
    //    object vc1 = new VCard();
    //    object vc2 = new VCard();

    //    Assert.IsFalse(vc1.Equals(vc2));
    //    Assert.AreNotEqual(vc1.GetHashCode(), vc2.GetHashCode());
    //}

    //[TestMethod]
    //public void EqualsTest2()
    //{
    //    var timestamp = DateTimeOffset.UtcNow;
    //    var uuid = Guid.NewGuid();
    //    VCard vc1 = new VCard();
    //    VCard vc2 = new VCard();

    //    vc1.TimeStamp = new TimeStampProperty(timestamp);
    //    vc1.UniqueIdentifier = new UuidProperty(uuid);

    //    vc2.TimeStamp = new TimeStampProperty(timestamp);
    //    vc2.UniqueIdentifier = new UuidProperty(uuid);

    //    object o1 = vc1;
    //    object o2 = vc2;

    //    Assert.IsTrue(o1.Equals(o2));
    //    Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());
    //}
}
