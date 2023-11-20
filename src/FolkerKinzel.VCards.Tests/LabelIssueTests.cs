using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class LabelIssueTests
{
    [TestMethod]
    public void LabelIssueTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var filter = new AnsiFilter();
        IList<VCard> vCards = filter.LoadVcf(TestFiles.LabelIssueVcf, out string enc);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.IsNotNull(vCards[0]);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.AreEqual(3, addresses!.Count());

        const string street1 = "Business-Straße 19";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street1) &&
                 x.Value.Street.Contains(street1)));

        const string street2 = "Freizeitweg 4";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street2) &&
                 x.Value.Street.Contains(street2)));

        const string street3 = "Sonstgasse 44";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street3) &&
                 x.Value.Street.Contains(street3)));
    }

    [TestMethod]
    public void LabelTest1()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var filter = new AnsiFilter();
        IList<VCard> vCards = filter.LoadVcf(TestFiles.LabelTest1Vcf, out string enc);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.IsNotNull(vCards[0]);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.AreEqual(3, addresses!.Count());

        const string street1 = "Business-Straße 19";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street1) &&
                 x.Value.Street.Contains(street1)));

        const string street2 = "Freizeitweg 4";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street2) &&
                 x.Value.Street.Contains(street2)));

        const string street3 = "Sonstgasse 44";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street3) && x.Value.IsEmpty));

    }


    [TestMethod]
    public void LabelTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var filter = new AnsiFilter();
        IList<VCard> vCards = filter.LoadVcf(TestFiles.LabelTest2Vcf, out string enc);
        Assert.IsNotNull(vCards);
        Assert.AreEqual(1, vCards.Count);
        Assert.IsNotNull(vCards[0]);
        IEnumerable<AddressProperty?>? addresses = vCards[0].Addresses;
        Assert.IsNotNull(addresses);
        Assert.AreEqual(4, addresses!.Count());

        const string street1 = "Business-Straße 19";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street1) &&
                 x.Value.IsEmpty));

        const string street2 = "Freizeitweg 4";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street2) &&
                 x.Value.IsEmpty));

        const string street3 = "Sonstgasse 44";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street3) &&
                 x.Value.IsEmpty));

        const string street4 = "Fabrikstraße 1";
        Assert.IsNotNull(addresses!.FirstOrDefault(
            x => x!.Parameters!.Label!.Contains(street4)));

    }

    [TestMethod]
    public void LabelTest3()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        IList<VCard> vcs = VCard.ParseVcf(vcf);
        IEnumerable<AddressProperty?>? adr = vcs[0].Addresses;
        Assert.IsNotNull(adr);
        Assert.AreEqual(5, adr.Count());
        Assert.IsTrue(adr.Any(x => x?.Value.Street[0] == "1" && x.Parameters.Label == "1"));
    }

    [TestMethod]
    public void LabelTest4()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

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

        IList<VCard> vcs = VCard.ParseVcf(vcf);
        IEnumerable<AddressProperty?>? adr = vcs[0].Addresses;
        Assert.IsNotNull(adr);
        Assert.AreEqual(4, adr.Count());
        Assert.IsTrue(adr.Any(x => x?.Value.Street[0] == "1" && x.Parameters.Label == "1"));
    }
}
