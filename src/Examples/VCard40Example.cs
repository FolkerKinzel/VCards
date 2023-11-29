using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples;

public static class VCard40Example
{
    public static void SaveSingleVCardAsVcf(string directoryPath)
    {
        // In order to initialize the library, the executing application MUST be registered
        // with the VCard class. To do this, call the static method VCard.RegisterApp with an absolute
        // Uri once when the program starts. (UUID URNs are ideal for this.) This registration
        // is used for the data synchronization mechanism introduced with vCard 4.0 (PID and
        // CLIENTPIDMAP).
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        const string vcfExtension = ".vcf";

        // Note that argument validation and exception handling is completely omitted in this
        // example. The following "if" statement only ensures, that the method doesn't destroy
        // valueable data.
        if (Directory.GetFiles(directoryPath)
                     .Any(x => x.EndsWith(vcfExtension, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("The method \"SaveSingleVCardAsVcf(string)\" could not be executed");
            Console.WriteLine("because the destination directory contains .VCF files, that might");
            Console.WriteLine("be overwritten.");

            return;
        }

        // Initialize a group vCard with composers names and live dates:
        VCard? composersVCard = VCardBuilder
            .Create()
            .DisplayNames.Add("Composers")
            .Kind.Set(Kind.Group)
            .Members.Add(InitializeComposerVCard(
                    "Sergei Rachmaninoff", new DateOnly(1873, 4, 1), new DateOnly(1943, 3, 28)))
            .Members.Add(InitializeComposerVCard(
                    "Ludwig van Beethoven", new DateOnly(1770, 12, 17), new DateOnly(1827, 3, 26)))
            .Members.Add(InitializeComposerVCard(
                    "Frédéric Chopin", new DateOnly(1810, 3, 1), new DateOnly(1849, 10, 17)))
            .Build();

        // Replace the embedded VCards in composersVCard.Members with Guid
        // references in order to save them as separate vCard 4.0 .VCF files.
        // IMPORTANT: Never call Reference() if you intend to serialize
        // a vCard 2.1 or vCard 3.0 !

        IEnumerable<VCard> referenced = composersVCard.Reference();
        // (The extension method can be called on a single VCard because
        // VCard implements IEnumerable<VCard>.)

        Console.WriteLine();
        Console.WriteLine(
            $"After ReferenceVCards() vCardList contains {referenced.Count()} VCard objects.");
        Console.WriteLine();
        Console.WriteLine("composersVCard:");
        Console.WriteLine();
        Console.WriteLine(
            referenced
                .Where(x => x.DisplayNames?
                             .Any(x => StringComparer.Ordinal.Equals(x?.Value, "Composers")) ?? false)
                .First()
                .ToVcfString(VCdVersion.V4_0));

        // Make sure to save ALL VCard objects in `referenced` - otherwise
        // the information originally stored in `composersVCard` will be
        // irrevocably lost.
        foreach (VCard vCard in referenced)
        {
            string fileName = Path.Combine(
                directoryPath,
                $"{vCard.DisplayNames!.First()!.Value}{vcfExtension}");

            vCard.SaveVcf(fileName, VCdVersion.V4_0);
        }

        // Reload the .VCF files:
        IEnumerable<VCard> vCards = 
            Vcf.LoadMany(Directory.EnumerateFiles(directoryPath, $"*{vcfExtension}"));

        // Make the reloaded VCard objects searchable:
        IEnumerable<VCard> dereferenced = vCards.Dereference();

        // Find the parsed result from "Composers.vcf":
        composersVCard = dereferenced
            .FirstOrDefault
             (
              x => x.DisplayNames?.Any(x => x?.Value == "Composers") ?? false
             );

        if (composersVCard is null)
        {
            Console.WriteLine("Composers.vcf not found!");
        }
        else
        {
            //Retrieve Beethovens birth year from the members of the "Composers.vcf" group:
            Console.Write("What year was Beethoven born?: ");
            Console.WriteLine(
                TryFindBeethovensBirthday(composersVCard, out DateOnly birthDay)
                   ? birthDay.Year 
                   : "Don't know.");
        }
    }

    private static VCard InitializeComposerVCard(
        string composersName, DateOnly birthDate, DateOnly deathDate)
        => VCardBuilder.Create()
                       .DisplayNames.Add(composersName)
                       .BirthDayViews.Add(birthDate)
                       .DeathDateViews.Add(deathDate)
                       .Build();

    private static bool TryFindBeethovensBirthday(VCard composersVCard, out DateOnly birthDay)
    {
        DateOnly date = default;
        bool found = composersVCard.Members
                .OrderByPref()
                .Where(x =>  x.Value!.VCard is not null)
                .Select(x => x.Value!.VCard)
                    .FirstOrDefault(x => x!.DisplayNames?
                                           .Any(x => x?.Value == "Ludwig van Beethoven") ?? false)?
                    .BirthDayViews?
                    .FirstOrNull(x => x.Value?.TryAsDateOnly(out date) ?? false)
                    != null;

        birthDay = date;
        return found;
    }
}

/*
Console Output:

After ReferenceVCards() vCardList contains 4 VCard objects.

composersVCard:

BEGIN:VCARD
VERSION:4.0
KIND:group
REV:20231113T205916Z
UID:urn:uuid:37eae851-8fed-4562-a753-57d5bbca8280
FN;PID=1.1:Composers
MEMBER;VALUE=URI;PID=1.1:urn:uuid:f166bb68-b252-4f48-b6e5-791a209814f6
MEMBER;VALUE=URI;PID=2.1:urn:uuid:579bbfc2-8366-4095-b70f-11205a6151e0
MEMBER;VALUE=URI;PID=3.1:urn:uuid:6b05a04b-9440-4d6d-b770-2a5b9f07bc25
CLIENTPIDMAP:1;urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556
END:VCARD

What year was Beethoven born?: 1770
*/