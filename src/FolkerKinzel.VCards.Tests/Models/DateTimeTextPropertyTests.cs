using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Intls.Models;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class DateTimeTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeTextPropertyTest1()
    {
        string now = "Früh morgens";

        var prop = DateAndOrTimeProperty.Create(now, GROUP);

        var vcard = new VCard
        {
            BirthDayViews = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.BirthDayViews);

        DateTimeTextProperty? prop2 = vcard.BirthDayViews!.First() as DateTimeTextProperty;

        Assert.IsNotNull(prop2);
        Assert.AreEqual(now, prop2!.Value);
        Assert.AreEqual(GROUP, prop2.Group);
        Assert.IsFalse(prop2.IsEmpty);
        Assert.AreEqual(Enums.VCdDataType.Text, prop2.Parameters.DataType);
    }

    [TestMethod]
    public void DateTimeTextPropertyTest2()
    {
        var prop = DateAndOrTimeProperty.Create("   ");
        Assert.IsNull(prop.Value);
    }
}
