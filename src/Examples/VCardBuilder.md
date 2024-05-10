## Efficient building and editing of VCard objects using VCardBuilder
The `VCardBuilder` class provides a fluent API for building and editing VCard objects.

The properties of the VCardBuilder class have the same names as those of the VCard class. Each of these 
properties gets a struct that provides methods to edit the corresponding VCard property. 
Each of these methods return the VCardBuilder instance so that the calls can be chained.

The `VCardBuilder.Create` method overloads initialize a VCardBuilder, which creates a new 
VCard instance or edits an existing one. The `VCardBuilder.VCard` property gets the VCard 
object that the VCardBuilder created or manipulated.

See an example how `VCardBuilder` can be used:
```csharp
VCard vCard = VCardBuilder
    .Create()
    .NameViews.Add(familyNames: ["Müller-Risinowsky"],
                   givenNames: ["Käthe"],
                   additionalNames: ["Alexandra", "Caroline"],
                   prefixes: ["Prof.", "Dr."],
                   displayName: (displayNames, name) => displayNames.Add(name.ToDisplayName())
                  )
    .GenderViews.Add(Sex.Female)
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
    // Unless specified, an address label is automatically applied to the AddressProperty object.
    // Specifying the country helps to format this label correctly.
    // Applying a group name to the AddressProperty helps to automatically preserve its Label,
    // TimeZone and GeoCoordinate when writing a vCard 2.1 or vCard 3.0.
    .Addresses.Add("Friedrichstraße 22", "Berlin", null, "10117", "Germany",
                    parameters: p =>
                    {
                        p.PropertyClass = PCl.Work;
                        p.AddressType = Adr.Dom | Adr.Intl | Adr.Postal | Adr.Parcel;
                        p.TimeZone = TimeZoneID.Parse("Europe/Berlin");
                        p.GeoPosition = new GeoCoordinate(52.51182050685474, 13.389581454284256);
                    },
                    group: vc => vc.NewGroup()
                  )
    .EMails.Add("kaethe_mueller@internet.com", parameters: p => p.PropertyClass = PCl.Work)
    .EMails.Add("mailto:kaethe_at_home@internet.com",
                 parameters: p =>
                 {
                     p.DataType = Data.Uri;
                     p.PropertyClass = PCl.Home;
                 }
               )
    .EMails.SetPreferences()
    .BirthDayViews.Add(1984, 3, 28)
    .Relations.Add("Paul Müller-Risinowsky",
                   Rel.Spouse | Rel.CoResident | Rel.Colleague
                  )
    .AnniversaryViews.Add(2006, 7, 14)
    .VCard;
```