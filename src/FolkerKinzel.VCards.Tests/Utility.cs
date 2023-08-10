using System.Xml.Linq;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Tests;

internal static class Utility
{
    internal static VCard CreateVCard()
    {
        var sortAs = new string[] { "Entenhausen", "Elmstreet" };

        var tel1 = new TextProperty("123");
        tel1.Parameters.TelephoneType = default(TelTypes);
        tel1.Parameters.TelephoneType = TelTypes.Voice | TelTypes.Cell | TelTypes.Text | TelTypes.Msg;

        tel1.Parameters.Preference = -50;
        tel1.Parameters.Preference = 500;
        tel1.Parameters.Preference = 1;
        tel1.Parameters.PropertyClass = default(PropertyClassTypes);
        tel1.Parameters.PropertyClass = PropertyClassTypes.Home | PropertyClassTypes.Work;


        var hobby1 = new TextProperty("Swimming");
        hobby1.Parameters.InterestLevel = InterestLevel.Medium;

        var expertise1 = new TextProperty("C#");
        expertise1.Parameters.ExpertiseLevel = ExpertiseLevel.Average;

        var adr1 = new AddressProperty("Elmstraße 13", "Entenhausen", null, postalCode: "01234");
        adr1.Parameters.Label = "  ";
        adr1.Parameters.Label = "Elmstreet 13; bei Müller" + Environment.NewLine + "01234 Entenhausen";
        adr1.Parameters.GeoPosition = new Models.GeoCoordinate(12.98, 7.86);
        adr1.Parameters.TimeZone = new Models.TimeZoneID(TimeZoneInfo.Local.Id);
        adr1.Parameters.AltID = "Address";
        adr1.Parameters.Calendar = "GREGORIAN";
        adr1.Parameters.ContentLocation = ContentLocation.Inline;
        adr1.Parameters.Index = 0;
        adr1.Parameters.Language = "de";
        adr1.Parameters.SortAs = sortAs;
        var pidMap = new PropertyIDMapping(5, new Uri("http://folkerkinzel.de"));
        adr1.Parameters.PropertyIDs = new PropertyID[] { new PropertyID(3, pidMap), new PropertyID(2) };
        adr1.Parameters.AddressType = AddressTypes.Dom | AddressTypes.Intl | AddressTypes.Parcel | AddressTypes.Postal;

        var logo1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/logo.jpg"));
        logo1.Parameters.MediaType = "image/jpeg";

        var photo1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/photo.png"));
        logo1.Parameters.MediaType = "image/png";

        var sound1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/audio.mp3"));
        logo1.Parameters.MediaType = "audio/mpeg";

        var key1 = DataProperty.FromUri(new Uri("https://folker-kinzel.de/pgp"));
        logo1.Parameters.MediaType = "application/pgp-keys";


        var email1 = new TextProperty("email@folker.com");
        email1.Parameters.EmailType = "  ";
        email1.Parameters.EmailType = EmailType.SMTP;

        var name1 = new NameProperty("Künzel", "Folker");
        name1.Parameters.SortAs = new string[] { "Kinzel", "Folker" };

        var name2 = new NameProperty("Кинцэл", "Фолкер");
        name2.Parameters.SortAs = new string[] { "Kinzel", "Folker" };
        name2.Parameters.Language = "ru-RU";
        name2.Parameters.AltID = "  ";

        var names = new NameProperty[] {name1, name2}; 


        var impp1 = new TextProperty("aim:uri.com");
        impp1.Parameters.InstantMessengerType = ImppTypes.Personal;

        var impp2 = new TextProperty("gg:uri.com");
        impp1.Parameters.InstantMessengerType = ImppTypes.Business;

        var impp3 = new TextProperty("gtalk:uri.com");
        impp1.Parameters.InstantMessengerType = ImppTypes.Mobile;

        var impp4 = new TextProperty("com.google.hangouts:uri.com");
        var impp5 = new TextProperty("icq:uri.com");
        var impp6 = new TextProperty("icq:uri.com");
        var impp7 = new TextProperty("xmpp:uri.com");
        var impp8 = new TextProperty("msnim:uri.com");
        var impp9 = new TextProperty("sip:uri.com");
        var impp10 = new TextProperty("skype:uri.com");
        var impp11 = new TextProperty("twitter:uri.com");
        var impp12 = new TextProperty("ymsgr:uri.com");

        var rel1 = new RelationTextProperty("Agent", RelationTypes.Agent);
        var rel2 = new RelationTextProperty("Spouse");
        rel2.Parameters.RelationType = default(RelationTypes);
        rel2.Parameters.RelationType = RelationTypes.Spouse | RelationTypes.CoResident;


        var nonStandard1 = new NonStandardProperty("X-NON-STANDARD", "The value");
        nonStandard1.Parameters.NonStandardParameters
            = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("X-NONSTD", "para-value") };

        var nonStandard2 = new NonStandardProperty("X-WAKEUP", "07:00:00");
        nonStandard2.Parameters.DataType = VCdDataType.Time;

        var nonStandard = new NonStandardProperty[] { nonStandard1, nonStandard2 };

        var xName = XName.Get("{TheNs}TheLocal");
        var xEl = new XElement(xName, "The content");
        var xml1 = new XmlProperty(xEl);

        var bday = new DateTimeOffsetProperty(new DateTime(1977, 11, 11));
        bday.Parameters.Calendar = "  ";

        var source = new TextProperty("http://neu.de/");
        source.Parameters.Context = " ";
        source.Parameters.Context = "VCARD";

        return new VCard
        {
            NameViews = names,
            PhoneNumbers = tel1,
            Hobbies = hobby1,
            Interests = hobby1,
            Expertises = expertise1,
            BirthDayViews = bday,
            AnniversaryViews = new DateTimeOffsetProperty(new DateTime(2001, 9, 11)),
            Logos = logo1,
            Photos = photo1,
            Sounds = sound1,
            Keys = key1,
            DeathDateViews = new DateTimeTextProperty("Later"),
            DeathPlaceViews = new TextProperty("Somewhere"),
            BirthPlaceViews = new TextProperty("Dessau"),
            ProdID = new TextProperty("Testcode"),
            Addresses = adr1,
            CalendarAddresses = new TextProperty("Calender address"),
            CalendarUserAddresses = new TextProperty("Calendar user address"),
            FreeOrBusyUrls = new TextProperty("Free Busy"),
            Sources = source,
            TimeZones = new TimeZoneProperty(new TimeZoneID(TimeZoneInfo.Local.Id)),
            DisplayNames = new TextProperty("Folker"),
            OrgDirectories = new TextProperty("OrgDirectory"),
            Profile = new ProfileProperty("Group"),
            Categories = new StringCollectionProperty(new string[] { "Person", "Data" }),
            TimeStamp = new TimeStampProperty(),
            EmailAddresses = email1,
            Roles = new TextProperty("Rechte Hand"),
            Titles = new TextProperty("Sündenbock"),
            UniqueIdentifier = new UuidProperty(),
            URLs = new TextProperty("www.folker.com"),
            DirectoryName = new TextProperty("Webseite"),
            Access = new AccessProperty(Access.Confidential),
            GenderViews = new GenderProperty(Gender.NonOrNotApplicable),
            GeoCoordinates = new GeoProperty(new Models.GeoCoordinate(23.456, 49.654)),
            NickNames = new StringCollectionProperty(new string[] { "Genius", "The Brain" }),
            Kind = new KindProperty(VCdKind.Organization),
            Mailer = new TextProperty("The Mailer"),
            Languages = new TextProperty("de"),
            Notes = new TextProperty("Kommentar"),
            PropertyIDMappings = new PropertyIDMappingProperty(new PropertyIDMapping(7, new Uri("http://folkerkinzel.de"))),

            InstantMessengerHandles = new TextProperty[]
            {
                    impp1, impp2, impp3, impp4, impp5, impp6, impp7, impp8, impp9, impp10, impp11, impp12
            },

            Relations = new RelationProperty[] { rel1, rel2 },
            Organizations = new OrganizationProperty("The ÄÖÜ Organization", new string[] { "Department", "Office" }),
            NonStandardProperties = nonStandard,
            XmlProperties = xml1,
        };
    }
}

