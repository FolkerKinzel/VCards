using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Extensions;
using Mod = FolkerKinzel.VCards.Models;

namespace Examples;

public static class EmbeddedVCardExample
{
    public static void FromVCardExample()
    {
        // This will work as expected:
        var vc1 = new VCard
        {
            DisplayNames = new Mod::TextProperty("Donald Duck")
        };

        var prop1 = Mod::RelationProperty.FromVCard(vc1);
        Console.WriteLine("prop1, DisplayName: {0}", GetDisplayName(prop1));

        // This won't work because RelationProperty.FromVCard(..) clones
        // its input:
        var vc2 = new VCard();
        var prop2 = Mod::RelationProperty.FromVCard(vc2);
        vc2.DisplayNames = new Mod::TextProperty("Dagobert Duck");
        Console.WriteLine("prop2, DisplayName: {0}", GetDisplayName(prop2));

        // In order to fix this you need to get the VCard reference from the
        // RelationProperty:
        var vc3 = new VCard();
        var prop3 = Mod::RelationProperty.FromVCard(vc3);
        vc3 = prop3.Value!.VCard; // Get the reference!
        vc3!.DisplayNames = new Mod::TextProperty("Dagobert Duck");
        Console.WriteLine("prop3, DisplayName: {0}", GetDisplayName(prop3));

        static string GetDisplayName(Mod::RelationProperty prop)
            => prop.Value!.VCard!.DisplayNames.PrefOrNull()?.Value ?? "<null>";
    }
}

/*
Console Output:

prop1, DisplayName: Donald Duck
prop2, DisplayName: <null>
prop3, DisplayName: Dagobert Duck
*/