using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass()]
public class VCardTests
{
    public TestContext? TestContext { get; set; }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LoadTest_fileNameNull() => _ = VCard.LoadVcf(null!);

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void LoadTest_invalidFileName() => _ = VCard.LoadVcf("  ");

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ParseTest_contentNull() => _ = VCard.ParseVcf(null!);

    [TestMethod()]
    public void ParseTest_contentEmpty()
    {
        IList<VCard> list = VCard.ParseVcf("");
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod()]
    public void ParseTest()
    {
        IList<VCard> list = VCard.ParseVcf("BEGIN:VCARD\r\nFN:Folker\r\nEND:VCARD");
        Assert.AreEqual(1, list.Count);

        Assert.IsNotNull(list[0].DisplayNames);

        TextProperty? dispNameProp = list[0].DisplayNames!.FirstOrDefault();
        Assert.IsNotNull(dispNameProp);
        Assert.AreEqual("Folker", dispNameProp?.Value);
    }


    [DataTestMethod()]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SaveTest(VCdVersion version)
    {
        var vcard = new VCard
        {
            DisplayNames = new TextProperty[] { new TextProperty("Folker") }
        };

        string path = Path.Combine(TestContext!.TestRunResultsDirectory, $"SaveTest_{version}.vcf");

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




    [DataTestMethod()]
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



    [DataTestMethod()]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void SerializeTest(VCdVersion version)
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


    [DataTestMethod()]
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

    [DataTestMethod()]
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


    [DataTestMethod()]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void DeserializeTest(VCdVersion version)
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

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeserializeTest_readerNull() => _ = VCard.DeserializeVcf(null!);


    [DataTestMethod()]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    public void ToVcfStringTest(VCdVersion version)
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


    [DataTestMethod()]
    [DataRow(VCdVersion.V2_1)]
    [DataRow(VCdVersion.V3_0)]
    [DataRow(VCdVersion.V4_0)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ToVcfStringTest_vcardListNull(VCdVersion version) => _ = VCard.ToVcfString(null!, version);


    [TestMethod()]
    public void ToStringTest()
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
    public void EqualsTest1()
    {
        object vc1 = new VCard();
        object vc2 = new VCard();

        Assert.IsFalse(vc1.Equals(vc2));
        Assert.AreNotEqual(vc1.GetHashCode(), vc2.GetHashCode());
    }

    [TestMethod]
    public void EqualsTest2()
    {
        var timestamp = DateTimeOffset.UtcNow;
        var uuid = Guid.NewGuid();
        VCard vc1 = new VCard();
        VCard vc2 = new VCard();

        vc1.TimeStamp = new TimeStampProperty(timestamp);
        vc1.UniqueIdentifier = new UuidProperty(uuid);

        vc2.TimeStamp = new TimeStampProperty(timestamp);
        vc2.UniqueIdentifier = new UuidProperty(uuid);

        object o1 = vc1;
        object o2 = vc2;

        Assert.IsTrue(o1.Equals(o2));
        Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());
    }
}
