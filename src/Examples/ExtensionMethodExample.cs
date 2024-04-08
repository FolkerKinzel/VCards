using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace Examples;

public class ExtensionMethodExample
{
    public static void Example()
    {
        VCard vc = InitializeTestVCard();

        Console.WriteLine("The content of vc is:\n");
        Console.WriteLine(vc);
        Console.WriteLine("\n=====================================\n");

        // (The extension methods will undertake all the necerssary
        // null checking.)

        // Group properties by their group name. (Group names are
        // case insensitive.)
        Console.WriteLine("\nProperty values with the group name g1:");

        IGrouping<string?, KeyValuePair<Prop, VCardProperty>> g1Group =
            vc.Groups.First(x => x.Key == "g1");

        foreach (KeyValuePair<Prop, VCardProperty> kvp in g1Group)
        {
            Console.WriteLine("  {0}: {1}", kvp.Key, kvp.Value);
        };

        Console.WriteLine("\nDisplayNames ordered by Preference:");
        foreach (TextProperty dn in vc.DisplayNames.OrderByPref())
        {
            Console.WriteLine("  {0}", dn);
        }

        Console.WriteLine("\nThe most preferred DisplayName is: {0}", vc.DisplayNames.PrefOrNull());

        Console.WriteLine("\nDisplayNames ordered by Index:");
        foreach (TextProperty dn in vc.DisplayNames.OrderByIndex())
        {
            Console.WriteLine("  {0}", dn);
        }

        Console.WriteLine("\nThe DisplayName with the lowest Index is: {0}", vc.DisplayNames.FirstOrNull());

        // Group VCardProperties by their AltID parameter:
        Console.WriteLine("\nThe following display names mean the same in different languages:");

        IGrouping<string?, TextProperty> altIDQuery =
            vc.DisplayNames.GroupByAltID()
                           .First(x => x.Key == "@1");

        foreach (TextProperty prop in altIDQuery)
        {
            Console.WriteLine("  {0}", prop);
        }

        // Serialize the VCard as VCF. (Most of the methods of the Vcf class are also
        // available as extension methods.)
        Console.WriteLine("\nvc as vCard 3.0:\n");
        Console.WriteLine(vc.ToVcfString(options: Opts.Default.Unset(Opts.UpdateTimeStamp)));
    }

    private static VCard InitializeTestVCard()
    {
        return VCardBuilder
            .Create(setID: false)
            .DisplayNames.Add("vCard zum Testen",
                               parameters:
                               p =>
                               {
                                   p.Language = "de";
                                   p.AltID = "@1";
                                   p.Index = 2;
                               }
                             )
            .DisplayNames.Add("vCard for testing",
                               pref: true,
                               parameters:
                               p =>
                               {
                                   p.Language = "en";
                                   p.AltID = "@1";
                               }
                             )
            .DisplayNames.Add("Something else",
                               parameters: p => p.Index = 1
                             )
            // The properties of the VCard class that contain collections allow
            // null references within these collections. Extension methods undertake
            // the necessary null checking when reading these properties:
            .DisplayNames.Edit(dplayNames => dplayNames.ConcatWith(null))
            .Phones.Add("1234",
                        // The ParameterSection.PropertyClass property is of Type
                        // Nullable<PCl>. Use extension methods to edit such properties
                        // safely.
                        parameters: p => p.PropertyClass = p.PropertyClass.Set(PCl.Home),
                        group: vc => "g1"
                        )
            .TimeZones.Add("Europe/Berlin", group: vc => "G1")
            .VCard;
    }
}

/*
Console Output:

The content of vc is:

Version: 2.1

[Preference: 1]
[Language: en]
[AltID: @1]
DisplayNames: vCard for testing

[Preference: 2]
[Language: de]
[AltID: @1]
[Index: 2]
DisplayNames: vCard zum Testen

[Index: 1]
DisplayNames: Something else

[Group: G1]
TimeZones: Europe/Berlin

[PropertyClass: Home]
[Group: g1]
Phones: 1234

=====================================


Property values with the group name g1:
  Phones: 1234
  TimeZones: Europe/Berlin

DisplayNames ordered by Preference:
  vCard for testing
  vCard zum Testen
  Something else

The most preferred DisplayName is: vCard for testing

DisplayNames ordered by Index:
  Something else
  vCard zum Testen
  vCard for testing

The DisplayName with the lowest Index is: Something else

The following display names mean the same in different languages:
  vCard for testing
  vCard zum Testen

vc as vCard 3.0:

BEGIN:VCARD
VERSION:3.0
G1.TZ:+01:00
FN;LANGUAGE=en:vCard for testing
N:?;;;;
g1.TEL;TYPE=HOME:1234
END:VCARD
 */
