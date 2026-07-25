using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class LabelIssueTests
{
    [TestMethod]
    public void LabelIssueTest1()
    {
        IReadOnlyList<VCard> vCards = Vcf.Load(TestFiles.LabelIssueVcf, new AnsiFilter());
        Assert.IsNotNull(vCards);
        Assert.HasCount(1, vCards);
        Assert.IsNotNull(vCards[0]);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.HasCount(3, addresses);

        const string street1 = "Business-Straße 19";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street1) &&
                 x.Value.Street.Contains(street1)));

        const string street2 = "Freizeitweg 4";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street2) &&
                 x.Value.Street.Contains(street2)));

        const string street3 = "Sonstgasse 44";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street3) &&
                 x.Value.Street.Contains(street3)));
    }

    [TestMethod]
    public void LabelTest1()
    {
        IReadOnlyList<VCard> vCards = Vcf.Load(TestFiles.LabelTest1Vcf, new AnsiFilter());
        Assert.IsNotNull(vCards);
        Assert.HasCount(1, vCards);
        Assert.IsNotNull(vCards[0]);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.HasCount(3, addresses);

        const string street1 = "Business-Straße 19";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street1) &&
                 x.Value.Street.Contains(street1)));

        const string street2 = "Freizeitweg 4";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street2) &&
                 x.Value.Street.Contains(street2)));

        const string street3 = "Sonstgasse 44";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street3) && x.Value.IsEmpty));

    }


    [TestMethod]
    public void LabelTest2()
    {
        IReadOnlyList<VCard> vCards = Vcf.Load(TestFiles.LabelTest2Vcf, new AnsiFilter());
        Assert.IsNotNull(vCards);
        Assert.HasCount(1, vCards);
        Assert.IsNotNull(vCards[0]);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.HasCount(4, addresses);

        const string street1 = "Business-Straße 19";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street1) &&
                 x.Value.IsEmpty));

        const string street2 = "Freizeitweg 4";
        Assert.IsNotNull(addresses.FirstOrNull(
            x => x!.Parameters.Label!.Contains(street2) &&
                 x.Value.IsEmpty));

        const string street3 = "Sonstgasse 44";
        Assert.IsNotNull(addresses.FirstOrDefault(
            x => x!.Parameters.Label!.Contains(street3) &&
                 x.Value.IsEmpty));

        const string street4 = "Fabrikstraße 1";
        Assert.IsNotNull(addresses.FirstOrDefault(
            x => x!.Parameters.Label!.Contains(street4)));

    }

    [TestMethod]
    public void LabelTest3()
    {
        const string vcf = """
            BEGIN:VCARD
            VERSION:2.1
            ADR;POSTAL:;;1;;;;
            ADR;PARCEL:;;2;;;;
            LABEL;WORK;DOM;PREF:3
            LABEL;HOME;POSTAL:4
            LABEL;POSTAL:1
            LABEL;WORK:5
            END:VCARD
            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(vcf);
        IEnumerable<AddressProperty?>? adr = vcs[0].Addresses;
        Assert.IsNotNull(adr);
        Assert.HasCount(5, adr);
        Assert.Contains(x => x?.Value.Street[0] == "1" && x.Parameters.Label == "1", adr);
    }

    [TestMethod]
    public void LabelTest4()
    {
        const string vcf = """
            BEGIN:VCARD
            VERSION:2.1
            ADR;POSTAL:;;1;;;;
            a.ADR;PARCEL:;;2;;;;
            A.LABEL;WORK;DOM;PREF:3
            B.LABEL;HOME;POSTAL:4
            LABEL;POSTAL:1
            LABEL;WORK:5
            END:VCARD
            """;

        IReadOnlyList<VCard> vcs = Vcf.Parse(vcf);
        IEnumerable<AddressProperty?>? adr = vcs[0].Addresses;
        Assert.IsNotNull(adr);
        Assert.HasCount(4, adr);
        Assert.Contains(x => x?.Value.Street[0] == "1" && x.Parameters.Label == "1", adr);
    }
}
