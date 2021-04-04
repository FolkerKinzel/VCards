using System;
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
            adr1.Parameters.GeoPosition = new Models.PropertyParts.GeoCoordinate(12.98, 7.86);
            adr1.Parameters.TimeZone = TimeZoneInfo.Local;
            adr1.Parameters.AltID = "Address";
            adr1.Parameters.Calendar = "GREGORIAN";
            adr1.Parameters.ContentLocation = VCdContentLocation.Inline;
            adr1.Parameters.Index = 1;
            adr1.Parameters.Language = "de";
            adr1.Parameters.SortAs = sortAs;
            adr1.Parameters.PropertyIDs = new PropertyID[] { new PropertyID(3, 5), new PropertyID(2) };

            var logo1 = new DataProperty(new Uri("https://folker-kinzel.de/logo.jpg"));
            logo1.Parameters.MediaType = "image/jpeg";

            var photo1 = new DataProperty(new Uri("https://folker-kinzel.de/photo.png"));
            logo1.Parameters.MediaType = "image/png";

            var sound1 = new DataProperty(new Uri("https://folker-kinzel.de/audio.mp3"));
            logo1.Parameters.MediaType = "audio/mpeg";


            var email1 = new TextProperty("email@folker.com");
            email1.Parameters.EmailType = EmailType.SMTP;

            var name1 = new NameProperty("Kinzel", "Folker");
            name1.Parameters.SortAs = new string[] { "Kinzel", "Folker" };

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

                DeathDateViews = new DateTimeTextProperty("Later"),

                DeathPlaceViews = new TextProperty("Somewhere"),

                BirthPlaceViews = new TextProperty("Dessau"),

                ProdID = new TextProperty("Testcode"),

                Addresses = adr1,

                CalendarAddresses = new TextProperty("Calender address"),

                CalendarUserAddresses = new TextProperty("Calendar user address"),

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

                GeoCoordinates = new GeoProperty(new Models.PropertyParts.GeoCoordinate(23.456, 49.654)),

                NickNames = new StringCollectionProperty(new string[] {"Genius", "The Brain"}),

                Kind = new KindProperty(VCdKind.Organization),

                Mailer = new TextProperty("The Mailer"),

                Languages = new TextProperty("de"),

                Notes = new TextProperty("Kommentar")


            };
        }
    }
}

