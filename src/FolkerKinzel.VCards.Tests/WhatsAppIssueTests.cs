namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class WhatsAppIssueTests
{
    [TestMethod]
    public void WhatsAppIssueTest1()
    {
        IList<VCard> list = VCard.LoadVcf(TestFiles.WhatsAppIssueVcf);
        Assert.AreNotEqual(0, list.Count);

        IEnumerable<Models.TextProperty?>? phoneNumbers = list[0].Phones;
        Assert.IsNotNull(phoneNumbers);

        Models.TextProperty? whatsAppNumber = phoneNumbers!.ElementAtOrDefault(1);
        Assert.IsNotNull(whatsAppNumber);

        KeyValuePair<string, string>? parameter = whatsAppNumber!.Parameters.NonStandard?.FirstOrDefault();

        Assert.IsTrue(parameter.HasValue);
        Assert.AreEqual("TYPE", parameter!.Value.Key);
        Assert.AreEqual("WhatsApp", parameter!.Value.Value);
    }
}
