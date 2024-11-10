using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Tests;

internal static class Utility
{
    internal static VCard CreateVCard()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(new Uri("http://folkerkinzel.de"));

        string[] sortAs = ["Entenhausen", "Elmstreet"];

        var tel1 = new TextProperty("123");
        tel1.Parameters.PhoneType = default(Tel);
        tel1.Parameters.PhoneType = Tel.Voice | Tel.Cell | Tel.Text | Tel.Msg;

        tel1.Parameters.Preference = -50;
        tel1.Parameters.Preference = 500;
        tel1.Parameters.Preference = 1;
        tel1.Parameters.PropertyClass = default(PCl);
        tel1.Parameters.PropertyClass = PCl.Home | PCl.Work;


        var hobby1 = new TextProperty("Swimming");
        hobby1.Parameters.Interest = Interest.Medium;

        var expertise1 = new TextProperty("C#");
        expertise1.Parameters.Expertise = Expertise.Average;

        var adr1 = new AddressProperty(
            AddressBuilder
            .Create()
            .AddStreet("Elmstraße 13")
            .AddStreetName("Elmstraße")
            .AddStreetNumber("13")
            .AddLocality("Entenhausen")
            .AddPostalCode("01234")
            .AddCountry("Germany"));
        adr1.Parameters.Label = "  ";
        adr1.Parameters.Label = "Elmstreet 13; bei Müller" + Environment.NewLine + "01234 Entenhausen";
        adr1.Parameters.GeoPosition = new GeoCoordinate(12.98, 7.86);
        adr1.Parameters.TimeZone = TimeZoneID.Parse(TimeZoneInfo.Local.Id);
        adr1.Parameters.AltID = "Address";
        adr1.Parameters.Calendar = "GREGORIAN";
        adr1.Parameters.ContentLocation = Loc.Inline;
        adr1.Parameters.Index = 0;
        adr1.Parameters.Language = "de";
        adr1.Parameters.SortAs = sortAs;
        var pidMap = new AppID(7, "http://www.contoso.com/");
        adr1.Parameters.PropertyIDs = [new(3, pidMap), new(2, null)];
        adr1.Parameters.AddressType = Adr.Dom | Adr.Intl | Adr.Parcel | Adr.Postal;

        var adr2 = new AddressProperty(
            AddressBuilder
            .Create()
            .AddStreet("Elm Street 13")
            .AddStreetName("Elm Street")
            .AddStreetNumber("13")
            .AddLocality("New York")
            .AddPostalCode("01234")
            .AddCountry("USA"));

        var logo1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/logo.jpg"), "image/jpeg");
        //logo1.Parameters.MediaType = "image/jpeg";

        var photo1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/photo.png"), "image/png");
        //photo1.Parameters.MediaType = "image/png";

        var sound1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/audio.mp3"), "audio/mpeg");
        //sound1.Parameters.MediaType = "audio/mpeg";

        var key1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/pgp"), "application/pgp-keys");
        var key2 = DataProperty.FromText("ThePassword");
        //key1.Parameters.MediaType = "application/pgp-keys";


        var email1 = new TextProperty("email@folker.com");
        email1.Parameters.EMailType = "  ";
        email1.Parameters.EMailType = EMail.SMTP;

        var name1 = new NameProperty(NameBuilder.Create().AddFamilyName("Künzel").AddGivenName("Folker"));
        name1.Parameters.SortAs = ["Kinzel", "Folker"];

        var name2 = new NameProperty(NameBuilder.Create().AddFamilyName("Кинцэл").AddGivenName("Фолкер"));
        name2.Parameters.SortAs = ["Kinzel", "Folker"];
        name2.Parameters.Language = "ru-RU";
        name2.Parameters.AltID = "  ";

        var names = new NameProperty[] { name1, name2 };


        var impp1 = new TextProperty("aim:uri.com");
        impp1.Parameters.InstantMessengerType = Impp.Personal;

        var impp2 = new TextProperty("gg:uri.com");
        impp1.Parameters.InstantMessengerType = Impp.Business;

        var impp3 = new TextProperty("gtalk:uri.com");
        impp1.Parameters.InstantMessengerType = Impp.Mobile;

        var impp4 = new TextProperty("com.google.hangouts:uri.com");
        var impp5 = new TextProperty("icq:uri.com");
        var impp6 = new TextProperty("icq:uri.com");
        var impp7 = new TextProperty("xmpp:uri.com");
        var impp8 = new TextProperty("msnim:uri.com");
        var impp9 = new TextProperty("sip:uri.com");
        var impp10 = new TextProperty("skype:uri.com");
        var impp11 = new TextProperty("twitter:uri.com");
        var impp12 = new TextProperty("ymsgr:uri.com");

        var rel1 = new RelationProperty(Relation.Create(ContactID.Create("Agent")));
        rel1.Parameters.RelationType = Rel.Agent;
        var rel2 = new RelationProperty(Relation.Create(ContactID.Create("Spouse")));
        rel2.Parameters.RelationType = default(Rel);
        rel2.Parameters.RelationType = Rel.Spouse | Rel.CoResident;


        var nonStandard1 = new NonStandardProperty("X-NON-STANDARD", "The value");
        nonStandard1.Parameters.NonStandard
            = [new("X-NONSTD", "para-value")];

        var nonStandard2 = new NonStandardProperty("X-WAKEUP", "07:00:00");
        nonStandard2.Parameters.DataType = Data.Time;

        var nonStandard = new NonStandardProperty[] { nonStandard1, nonStandard2 };

        var xName = XName.Get("{TheNs}TheLocal");
        var xEl = new XElement(xName, "The content");
        var xml1 = new XmlProperty(xEl);

        var bday = DateAndOrTimeProperty.FromDate(1977, 11, 11);
        bday.Parameters.Calendar = "  ";

        var source = new TextProperty("http://neu.de/");
        source.Parameters.Context = " ";
        source.Parameters.Context = "VCARD";

        var org = new Organization("The ÄÖÜ Organization", ["Department", "Office"]);

        var vc = new VCard
        {
            AppIDs = new AppIDProperty(pidMap),
            NameViews = names,
            Phones = tel1,
            Hobbies = hobby1,
            Interests = hobby1,
            Expertises = expertise1,
            BirthDayViews = bday,
            AnniversaryViews = DateAndOrTimeProperty.FromDate(2001, 9, 11),
            Logos = logo1,
            Photos = photo1,
            Sounds = sound1,
            Keys = [key1, key2],
            DeathDateViews = DateAndOrTimeProperty.FromText("Later"),
            DeathPlaceViews = new TextProperty("Somewhere"),
            BirthPlaceViews = new TextProperty("Dessau"),
            ProductID = new TextProperty("Testcode"),
            Addresses = [adr1, adr2],
            CalendarAddresses = new TextProperty("Calender address"),
            CalendarUserAddresses = new TextProperty("Calendar user address"),
            FreeOrBusyUrls = new TextProperty("Free Busy"),
            Sources = source,
            TimeZones = new TimeZoneProperty(TimeZoneID.Parse(TimeZoneInfo.Local.Id)),
            DisplayNames = new TextProperty("Folker"),
            OrgDirectories = new TextProperty("OrgDirectory"),
            Profile = new ProfileProperty(),
            Categories = new StringCollectionProperty(["Person", "Data"]),
            TimeStamp = new TimeStampProperty(),
            EMails = email1,
            Roles = new TextProperty("Rechte Hand"),
            Titles = new TextProperty("Sündenbock"),
            ID = new IDProperty(ContactID.Create()),
            Urls = new TextProperty("www.folker.com"),
            DirectoryName = new TextProperty("Webseite"),
            Access = new AccessProperty(Access.Confidential),
            GenderViews = new GenderProperty(Sex.NonOrNotApplicable),
            GeoCoordinates = new GeoProperty(new GeoCoordinate(23.456, 49.654)),
            NickNames = new StringCollectionProperty(["Genius", "The Brain"]),
            Kind = new KindProperty(Kind.Organization),
            Mailer = new TextProperty("The Mailer"),
            Languages = new TextProperty("de"),
            Notes = new TextProperty("Kommentar"),
            GramGenders = [new GramProperty(Gram.Neuter), new GramProperty(Gram.Common)],

            Messengers =
            [
                    impp1, impp2, impp3, impp4, impp5, impp6, impp7, impp8, impp9, impp10, impp11, impp12
            ],

            Relations = [rel1, rel2],
            Organizations = new OrgProperty(org),
            NonStandards = nonStandard,
            Xmls = xml1,
        };

        return vc;
    }
}

