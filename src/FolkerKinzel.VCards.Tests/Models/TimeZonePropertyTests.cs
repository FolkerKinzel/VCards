﻿using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class TimeZonePropertyTests
{
    private const string GROUP = "MyGroup";

    [TestMethod()]
    public void TimeZonePropertyTest1()
    {
        var tz = TimeZoneID.Parse(TimeZoneInfo.GetSystemTimeZones()[7].Id);
        var prop = new TimeZoneProperty(tz, GROUP);

        Assert.AreEqual(tz, prop.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod()]
    public void TimeZonePropertyTest2()
    {
        var tz = TimeZoneID.Parse(TimeZoneInfo.GetSystemTimeZones()[4].Id);
        var prop = new TimeZoneProperty(tz, GROUP);

        var vcard = new VCard
        {
            TimeZones = prop
        };

        string s = vcard.ToVcfString();

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.TimeZones);

        prop = vcard.TimeZones!.First();
        Assert.IsFalse(prop!.IsEmpty);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsTrue(tz.TryGetUtcOffset(out TimeSpan utc1));
        Assert.IsTrue(prop.Value!.TryGetUtcOffset(out TimeSpan utc2));
        Assert.AreEqual(utc1, utc2);
    }

    [TestMethod]
    public void TimeZonePropertyTest3()
    {
        VcfRow row = VcfRow.Parse("TZ:    ", new VcfDeserializationInfo())!;
        var prop = new TimeZoneProperty(row,VCdVersion.V3_0);

        Assert.IsTrue(prop.IsEmpty);

        using var writer = new StringWriter();
        var serializer = new Vcf_3_0Serializer(writer, VcfOptions.Default, null);

        prop.AppendValue(serializer);
        Assert.AreEqual(0, serializer.Builder.Length);
    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new TimeZoneProperty(TimeZoneID.Parse("+01"));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
