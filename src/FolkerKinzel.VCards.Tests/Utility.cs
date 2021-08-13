using System;
using System.Collections.Generic;
using System.Xml.Linq;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Tests
{
    internal static class Utility
    {

        internal static VCard CreateVCard()
        {
            var sortAs = new string[] { "Entenhausen", "Elmstreet" };

            var tel1 = new TextProperty("123");
            tel1.Parameters.TelephoneType = TelTypes.Voice | TelTypes.Cell | TelTypes.Text | TelTypes.Msg;
            tel1.Parameters.Preference = 1;
            tel1.Parameters.PropertyClass = PropertyClassTypes.Home | PropertyClassTypes.Work;


            var hobby1 = new TextProperty("Swimming");
            hobby1.Parameters.InterestLevel = InterestLevel.Medium;

            var expertise1 = new TextProperty("C#");
            expertise1.Parameters.ExpertiseLevel = ExpertiseLevel.Average;

            var adr1 = new AddressProperty("Elmstreet 13", "Entenhausen", "01234");
            adr1.Parameters.Label = "Elmstreet 13; bei Müller" + Environment.NewLine + "01234 Entenhausen";
            adr1.Parameters.GeoPosition = new Models.GeoCoordinate(12.98, 7.86);
            adr1.Parameters.TimeZone = TimeZoneInfo.Local;
            adr1.Parameters.AltID = "Address";
            adr1.Parameters.Calendar = "GREGORIAN";
            adr1.Parameters.ContentLocation = VCdContentLocation.Inline;
            adr1.Parameters.Index = 1;
            adr1.Parameters.Language = "de";
            adr1.Parameters.SortAs = sortAs;
            var pidMap = new PropertyIDMapping(5, new Uri("http://folkerkinzel.de"));
            adr1.Parameters.PropertyIDs = new PropertyID[] { new PropertyID(3, pidMap), new PropertyID(2) };

            var logo1 = new DataProperty(new Uri("https://folker-kinzel.de/logo.jpg"));
            logo1.Parameters.MediaType = "image/jpeg";

            var photo1 = new DataProperty(new Uri("https://folker-kinzel.de/photo.png"));
            logo1.Parameters.MediaType = "image/png";

            var sound1 = new DataProperty(new Uri("https://folker-kinzel.de/audio.mp3"));
            logo1.Parameters.MediaType = "audio/mpeg";

            var key1 = new DataProperty(new Uri("https://folker-kinzel.de/pgp"));
            logo1.Parameters.MediaType = "application/pgp-keys";


            var email1 = new TextProperty("email@folker.com");
            email1.Parameters.EmailType = EmailType.SMTP;

            var name1 = new NameProperty("Kinzel", "Folker");
            name1.Parameters.SortAs = new string[] { "Kinzel", "Folker" };


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
            var rel2 = new RelationTextProperty("Spouse", RelationTypes.Spouse | RelationTypes.CoResident);


            var nonStandard1 = new NonStandardProperty("X-NON-STANDARD", "The value");
            nonStandard1.Parameters.NonStandardParameters
                = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("X-NONSTD", "para-value") };

            var xName = XName.Get("{TheNs}TheLocal");
            var xEl = new XElement(xName, "The content");
            var xml1 = new XmlProperty(xEl);

            return new VCard
            {
                NameViews = name1,

                PhoneNumbers = tel1,

                Hobbies = hobby1,

                Interests = hobby1,

                Expertises = expertise1,

                BirthDayViews = new DateTimeOffsetProperty(new DateTime(1972, 11, 11)),

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

                Sources = new TextProperty("http://neu.de/"),

                TimeZones = new TimeZoneProperty(TimeZoneInfo.Local),

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

                Access = new AccessProperty(VCdAccess.Confidential),

                GenderViews = new GenderProperty(VCdSex.NonOrNotApplicable),

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

                Organizations = new OrganizationProperty("The Organization", new string[] {"Department", "Office"}),

                NonStandardProperties = nonStandard1,

                XmlProperties = xml1,

                
            };
        }
    }
}

