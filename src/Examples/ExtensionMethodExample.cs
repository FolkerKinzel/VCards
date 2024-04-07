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

        // (Note that the extension methods undertake all the necerssary
        // null checking.)

        // Group properties by their group name. (Note that group names
        // are case insensitive.)
        Console.WriteLine("\nProperty values with the group name g1:");

        IGrouping<string?, KeyValuePair<Prop, VCardProperty>> groupQuery =
            vc.AsEnumerable()
              .GroupByVCardGroup()
              .First(x => x.Key == "g1");

        foreach (KeyValuePair<Prop, VCardProperty> kvp in groupQuery)
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
        Console.WriteLine(vc.ToVcfString());
    }

    private static VCard InitializeTestVCard()
    {
        return VCardBuilder
            .Create()
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
