using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class Rfc9554Tests
{
    [TestMethod]
    public void CreatedTest1()
    {
        var vc = new VCard();
        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        Assert.IsTrue(v4.Contains("CREATED", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains("CREATED", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains("CREATED", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains("CREATED", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        Assert.IsNotNull(vc.Created);
        Assert.IsFalse(vc.Created.IsEmpty);
    }
    

    [TestMethod]
    public void CreatedTest2()
    {
        var vc = new VCard(setCreated: false);

        string v4 = vc.ToVcfString(VCdVersion.V4_0);
        Assert.IsFalse(v4.Contains("CREATED", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void GramGendersTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .GramGenders.Add(Gram.Neuter, p => p.Language = "de-DE")
            .GramGenders.Add(Gram.Common, p => p.Language = "en-US")
            .GramGenders.SetPreferences()
            .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        Assert.IsTrue(v4.Contains("GRAMGENDER", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains("GRAMGENDER", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains("GRAMGENDER", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains("GRAMGENDER", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        Assert.IsNotNull(vc.GramGenders);
        Assert.AreEqual(2, vc.GramGenders.Count());
        Assert.IsTrue(vc.GramGenders.All(x => x?.Parameters.Language is not null && x.Parameters.Preference < 100));
    }

    [TestMethod]
    public void LanguageTest1()
    {
        var vc = new VCard()
        {
            Language = new TextProperty("de-DE")
        };


        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        Assert.IsTrue(v4.Contains("LANGUAGE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains("LANGUAGE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains("LANGUAGE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains("LANGUAGE", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        Assert.IsNotNull(vc.Language);
        Assert.IsFalse(vc.Language.IsEmpty);
    }

    [TestMethod]
    public void PronounsTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .Pronouns.Add("Ihr/Ihre", p => p.Language = "de-DE")
            .Pronouns.Add("Your/Yours", p => p.Language = "en-US")
            .Pronouns.SetPreferences()
            .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        Assert.IsTrue(v4.Contains("PRONOUNS", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains("PRONOUNS", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains("PRONOUNS", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains("PRONOUNS", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        Assert.IsNotNull(vc.Pronouns);
        Assert.AreEqual(2, vc.Pronouns.Count());
        Assert.IsTrue(vc.Pronouns.All(x => x?.Parameters.Language is not null && x.Parameters.Preference < 100));
    }

    [TestMethod]
    public void SocialProfilesTest1()
    {
        VCard vc = VCardBuilder
            .Create()
            .SocialMediaProfiles.Add("https:www.x.com/y", p => { p.ServiceType = "X"; p.UserName = "Y"; })
            .SocialMediaProfiles.Add("https:www.x.com/z", p => { p.ServiceType = "X"; p.UserName = "Z"; p.DataType = Data.Uri; })
            .SocialMediaProfiles.Add("Y", p => { p.ServiceType = "X"; p.UserName = "Y"; p.DataType = Data.Text; })
            .SocialMediaProfiles.SetPreferences()
            .SocialMediaProfiles.Edit(p => p.Append(null))
            .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        string v4Pure = vc.ToVcfString(VCdVersion.V4_0, options: Opts.None);
        string v3Pure = vc.ToVcfString(VCdVersion.V3_0, options: Opts.None);
        string v2Pure = vc.ToVcfString(VCdVersion.V2_1, options: Opts.None);

        Assert.IsTrue(v4.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v4WithoutRfc9554.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4WithoutRfc9554.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4WithoutRfc9554.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v4Pure.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v3.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v3.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v3.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v3Pure.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v2.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v2.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";SERVICE_TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v2.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v2Pure.Contains("\nSOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2Pure.Contains("\nX-SOCIALPROFILE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2Pure.Contains(";SERVICE_TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2Pure.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2Pure.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        Assert.IsNotNull(vc.SocialMediaProfiles);
        Assert.AreEqual(3, vc.SocialMediaProfiles.Count());
        Assert.IsTrue(vc.SocialMediaProfiles.All(x => x?.Parameters.ServiceType is not null && x.Parameters.Preference < 100));
        Assert.IsTrue(vc.SocialMediaProfiles.Any(x => x?.Parameters.DataType == Data.Text));
        Assert.IsTrue(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Y")));
        Assert.IsTrue(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Z")));

        vc = Vcf.Parse(v4WithoutRfc9554)[0];

        Assert.IsNotNull(vc.SocialMediaProfiles);
        Assert.AreEqual(3, vc.SocialMediaProfiles.Count());
        Assert.IsTrue(vc.SocialMediaProfiles.All(x => x?.Parameters.ServiceType is not null && x.Parameters.Preference < 100));
        Assert.IsTrue(vc.SocialMediaProfiles.Any(x => x?.Parameters.DataType == Data.Text));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Y")));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Z")));

        vc = Vcf.Parse(v3)[0];

        Assert.IsNotNull(vc.SocialMediaProfiles);
        Assert.AreEqual(3, vc.SocialMediaProfiles.Count());
        Assert.IsTrue(vc.SocialMediaProfiles.All(x => x?.Parameters.ServiceType is not null));
        Assert.IsTrue(vc.SocialMediaProfiles.Any(x => x?.Parameters.DataType == Data.Text));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Y")));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Z")));

        vc = Vcf.Parse(v2)[0];

        Assert.IsNotNull(vc.SocialMediaProfiles);
        Assert.AreEqual(2, vc.SocialMediaProfiles.Count());
        Assert.IsTrue(vc.SocialMediaProfiles.All(x => x?.Parameters.ServiceType is not null));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => x?.Parameters.DataType == Data.Text));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Y")));
        Assert.IsFalse(vc.SocialMediaProfiles.Any(x => StringComparer.Ordinal.Equals(x?.Parameters.UserName, "Z")));
    }

    [TestMethod]
    public void ParametersTest1()
    {
        const string propID = "propID";
        const string authorName = "Susi";
        var author = new Uri("https://wwww.susi.com/");

        VCard vc = VCardBuilder
            .Create(false, false)
            .DisplayNames.Add("Folker", p => p.Derived = true)
            .Notes.Add("note",
            p =>
            {
                p.Author = author;
                p.AuthorName = authorName;
                p.Created = DateTimeOffset.UtcNow;
                p.PropertyID = propID;
            })
            .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        string v4Conserved = v4;

        v4 = v4.ReplaceWhiteSpaceWith("");
        v4WithoutRfc9554 = v4WithoutRfc9554.ReplaceWhiteSpaceWith("");
        v3 = v3.ReplaceWhiteSpaceWith("");
        v2 = v2.ReplaceWhiteSpaceWith("");

        Assert.IsTrue(v4.Contains(";AUTHOR=", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";AUTHOR-NAME=", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";CREATED=", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";DERIVED", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";PROP-ID", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v4WithoutRfc9554.Contains(";AUTHOR=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";AUTHOR-NAME=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";CREATED=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";DERIVED", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";PROP-ID", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v3.Contains(";AUTHOR=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";AUTHOR-NAME=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";CREATED=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";DERIVED", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";PROP-ID", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v2.Contains(";AUTHOR=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";AUTHOR-NAME=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";CREATED=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";DERIVED", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";PROP-ID", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4Conserved)[0];

        ParameterSection? par = vc.DisplayNames?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.IsTrue(par.Derived);


        par = vc.Notes?.First()?.Parameters;
        Assert.IsNotNull(par);

        Assert.AreEqual(author, par.Author);
        Assert.AreEqual(authorName, par.AuthorName);
        Assert.IsTrue(par.Created.HasValue);
        Assert.AreEqual(propID, par.PropertyID);
    }

    [TestMethod]
    public void ImppTest1()
    {
        const string serviceType = "WhatsDown";
        const string userName = "Susi";
        VCard vc = VCardBuilder
           .Create(false, false)
           .Messengers.Add("123",
           p =>
           {
               p.ServiceType = serviceType;
               p.UserName = userName;
           })
           .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        string v4Pure = vc.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Unset(Opts.WriteXExtensions).Unset(Opts.WriteRfc9554Extensions));
        string v3Pure = vc.ToVcfString(VCdVersion.V3_0, options: Opts.Default.Unset(Opts.WriteXExtensions).Unset(Opts.WriteRfc9554Extensions));

        Assert.IsTrue(v4.Contains("\nIMPP", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(v4WithoutRfc9554.Contains("\nIMPP", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4WithoutRfc9554.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(v4Pure.Contains("\nIMPP", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4Pure.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(v3.Contains("\nIMPP", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v3.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(v3Pure.Contains("\nIMPP", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains(";SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3Pure.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v2.Contains("\nIMPP", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";SERVICE_TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";X-SERVICE-TYPE", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";USERNAME", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        ParameterSection? par = vc.Messengers?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.AreEqual(serviceType, par.ServiceType);
        Assert.AreEqual(userName, par.UserName);

        vc = Vcf.Parse(v4WithoutRfc9554)[0];

        par = vc.Messengers?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.AreEqual(serviceType, par.ServiceType);
        Assert.IsNull(par.UserName);

        vc = Vcf.Parse(v4Pure)[0];

        par = vc.Messengers?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.IsNull(par.ServiceType);
        Assert.IsNull(par.UserName);

        vc = Vcf.Parse(v3)[0];

        par = vc.Messengers?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.AreEqual(serviceType, par.ServiceType);
        Assert.IsNull(par.UserName);

        vc = Vcf.Parse(v3Pure)[0];

        par = vc.Messengers?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.IsNull(par.ServiceType);
        Assert.IsNull(par.UserName);
    }

    [TestMethod]
    public void PhoneticAndScriptTest1()
    {
        VCard vc = VCardBuilder
           .Create(false, false)
           .NameViews.Add(NameBuilder.Create().AddSurname("Kinzel").Build(),
           p =>
           {
               p.Phonetic = Phonetic.Ipa;
               p.Script = "Latn";
           })
           .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        Assert.IsTrue(v4.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v4WithoutRfc9554.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v3.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v2.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        ParameterSection? par = vc.NameViews?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.IsTrue(par.Phonetic.HasValue);
        Assert.IsNotNull(par.Script);
    }

    [TestMethod]
    public void PhoneticAndScriptTest2()
    {
        VCard vc = VCardBuilder
           .Create(false, false)
           .Addresses.Add(AddressBuilder.Create().AddLocality("Berlin").Build(),
           p =>
           {
               p.Phonetic = Phonetic.Ipa;
               p.Script = "Latn";
           })
           .VCard;

        Serialize(vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2);

        Assert.IsTrue(v4.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(v4.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v4WithoutRfc9554.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v4WithoutRfc9554.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v3.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v3.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        Assert.IsFalse(v2.Contains(";PHONETIC=", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(v2.Contains(";SCRIPT=", StringComparison.OrdinalIgnoreCase));

        vc = Vcf.Parse(v4)[0];

        ParameterSection? par = vc.Addresses?.First()?.Parameters;
        Assert.IsNotNull(par);
        Assert.IsTrue(par.Phonetic.HasValue);
        Assert.IsNotNull(par.Script);
    }

    private static void Serialize(VCard vc, out string v4, out string v4WithoutRfc9554, out string v3, out string v2)
    {
        v4 = vc.ToVcfString(VCdVersion.V4_0);
        v4WithoutRfc9554 = vc.ToVcfString(VCdVersion.V4_0, options: Opts.Default.Unset(Opts.WriteRfc9554Extensions));
        v3 = vc.ToVcfString(VCdVersion.V3_0);
        v2 = vc.ToVcfString(VCdVersion.V2_1);
    }
}

