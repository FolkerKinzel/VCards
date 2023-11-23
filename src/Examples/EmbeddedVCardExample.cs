using FolkerKinzel.VCards;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples;

public static class EmbeddedVCardExample
{
    public static void FromVCardExample()
    {
        // In order to initialize the library, the executing application MUST be registered
        // with the VCard class. To do this, call the static method VCard.RegisterApp with an absolute
        // Uri once when the program starts. (UUID URNs are ideal for this.) This registration
        // is used for the data synchronization mechanism introduced with vCard 4.0 (PID and
        // CLIENTPIDMAP).
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        // This will work as expected:
        var vc1 = new VCard
        {
            DisplayNames = new VC::TextProperty("Donald Duck")
        };
        var prop1 = VC::RelationProperty.FromVCard(vc1);
        Console.WriteLine("prop1, DisplayName: {0}", GetDisplayName(prop1));

        // This won't work because RelationProperty.FromVCard(..) clones
        // its input:
        var vc2 = new VCard();
        var prop2 = VC::RelationProperty.FromVCard(vc2);
        vc2.DisplayNames = new VC::TextProperty("Dagobert Duck");
        Console.WriteLine("prop2, DisplayName: {0}", GetDisplayName(prop2));

        // In order to fix this you need to get the VCard reference from the
        // RelationProperty:
        var vc3 = new VCard();
        var prop3 = VC::RelationProperty.FromVCard(vc3);
        vc3 = prop3.Value!.VCard; // Get the reference!
        vc3!.DisplayNames = new VC::TextProperty("Dagobert Duck");
        Console.WriteLine("prop3, DisplayName: {0}", GetDisplayName(prop3));


        static string GetDisplayName(VC::RelationProperty prop)
            => prop.Value!.VCard!.DisplayNames?.FirstOrDefault()?.Value ?? "<null>";
    }
}

/*
Console Output:

prop1, DisplayName: Donald Duck
prop2, DisplayName: <null>
prop3, DisplayName: Dagobert Duck
*/