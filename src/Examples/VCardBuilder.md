## Efficient building and editing of VCard objects using Fluent APIs
The `VCardBuilder` class provides a fluent API for building and editing VCard objects. Since 
`Name` and `Address` are also complicated classes, the classes `NameBuilder` and `AddressBuilder`
also exist to create instances of these classes using a Fluent API.

The properties of the `VCardBuilder` class have the same names as those of the VCard class. Each of these 
properties gets a struct that provides methods to edit the corresponding VCard property. 
Each of these methods return this struct so that the calls can be chained.

The overloads of the `VCardBuilder.Create` method initialize a VCardBuilder instance, which creates a new 
VCard object or edits an existing one. The `VCardBuilder.VCard` property gets the VCard 
object that the VCardBuilder created or manipulated.

VCardBuilder doesn't throw exceptions and makes the most from the input arguments. If strong 
input validation is required, the model classes can be instantiated separately and used with 
VCardBuilder.

See an example of how to use these Fluent APIs:
```csharp
VCard vCard = VCardBuilder
  .Create()
  .NameViews.Add(NameBuilder
      .Create()
      .AddPrefix("Prof.")
      .AddPrefix("Dr.")
      .AddGiven("Käthe")
      .AddGiven2("Alexandra")
      .AddGiven2("Caroline")
      .AddSurname("Müller-Risinowsky")
      .AddGeneration("II.")
      .Build(),
       parameters: p => p.Language = "de-DE",
       group: vc => vc.NewGroup())
  .NameViews.ToDisplayNames(NameFormatter.Default)
  .GenderViews.Add(Gender.Female)
  .GramGenders.Add(Gram.Feminine, parameters: p => p.Language = "de")
  .Organizations.Add("Millers Company", ["C#", "Webdesign"])
  .Titles.Add("CEO")
  .Photos.AddFile(photoFilePath)
  .Phones.Add("tel:+49-321-1234567",
               parameters: p =>
               {
                   p.DataType = Data.Uri;
                   p.PropertyClass = PCl.Work;
                   p.PhoneType = Tel.Cell | Tel.Text | Tel.Msg | Tel.BBS | Tel.Voice;
               }
             )
  .Phones.Add("tel:+49-123-9876543",
               parameters: p =>
               {
                   p.DataType = Data.Uri;
                   p.PropertyClass = PCl.Home;
                   p.PhoneType = Tel.Voice | Tel.BBS;
               }
             )
  .Phones.SetIndexes()
  .Addresses.Add(AddressBuilder
      .Create()
      .AddStreetName("Friedrichstraße")
      .AddStreetNumber("22")
      .AddLocality("Berlin")
      .AddPostalCode("10117")
      .AddCountry("Germany")
      .Build(),
       parameters: p =>
       {
           p.PropertyClass = PCl.Work;
           p.AddressType = Adr.Dom | Adr.Intl | Adr.Postal | Adr.Parcel|Adr.Billing|Adr.Delivery;
           p.TimeZone = TimeZoneID.TryCreate("Europe/Berlin");
           p.GeoPosition = GeoCoordinate.TryCreate(52.51182050685474, 13.389581454284256, 10);
           // Specifying the country or the ParameterSection.CountryCode property helps to format
           // automatically appended address labels correctly:
           p.CountryCode = "de-DE";
       },
       // Applying a group name to the AddressProperty helps to automatically preserve its Label,
       // TimeZone and GeoCoordinate when writing a vCard 2.1 or vCard 3.0.
       group: vc => vc.NewGroup()
                )
  // Append automatically formatted address labels:
  .Addresses.AttachLabels(AddressFormatter.Default)
  .EMails.Add("kaethe_mueller@internet.com", parameters: p => p.PropertyClass = PCl.Work)
  .EMails.Add("mailto:kaethe_at_home@internet.com",
               parameters: p =>
               {
                   p.DataType = Data.Uri;
                   p.PropertyClass = PCl.Home;
               }
             )
  .EMails.SetPreferences()
  .Messengers.Add("https://wd.me/0123456789",
                  parameters: p => p.ServiceType = "WhatsDown")
  .SocialMediaProfiles.Add("https://y.com/Semaphore",
                            parameters: p =>
                            {
                                p.UserName = "Semaphore";
                                p.ServiceType = "Y";
                            }
                          )
  .BirthDayViews.Add(1984, 3, 28)
  .Relations.Add("Paul Müller-Risinowsky",
                 Rel.Spouse | Rel.CoResident | Rel.Colleague
                )
  .AnniversaryViews.Add(2006, 7, 14)
  .Notes.Add("Very experienced in Blazor.",
              parameters: p =>
              {
                  p.Created = DateTimeOffset.Now;
                  p.Author = new Uri("https://www.microsoft.com/");
                  p.AuthorName = "Satya Nadella";
              }
            )
  .VCard;
```