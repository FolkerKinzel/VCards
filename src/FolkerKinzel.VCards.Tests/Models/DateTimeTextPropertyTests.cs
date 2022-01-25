namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class DateTimeTextPropertyTests
{
    private const string GROUP = "myGroup";

    [TestMethod()]
    public void DateTimeTextPropertyTest1()
    {
        string now = "Früh morgens";

        var prop = new DateTimeTextProperty(now, GROUP);

        var vcard = new VCard
        {
            BirthDayViews = prop
        };

        string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.BirthDayViews);

        prop = vcard.BirthDayViews!.First() as DateTimeTextProperty;

        Assert.IsNotNull(prop);
        Assert.AreEqual(now, prop!.Value);
        Assert.AreEqual(GROUP, prop.Group);
        Assert.IsFalse(prop.IsEmpty);
        Assert.AreEqual(Enums.VCdDataType.Text, prop.Parameters.DataType);
    }
}
