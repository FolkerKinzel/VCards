using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Extensions;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples;

public static class VCard40Example
{
    public static void SaveSingleVCardAsVcf(string directoryPath)
    {
        const string vcfExtension = ".vcf";

        // Note that argument validation and exception handling is completely omitted in this
        // example. The following "if" statement only ensures, that the method doesn't destroy
        // valueable data.
        if (Directory.GetFiles(directoryPath).Any(x => x.EndsWith(vcfExtension, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("The method \"SaveSingleVCardAsVcf(string)\" could not be executed");
            Console.WriteLine("because the destination directory contains .VCF files, that might");
            Console.WriteLine("be overwritten.");

            return;
        }

        // Initialize a group vCard with composers names and live dates:
        var members = new VC::RelationVCardProperty[]
        {
                new VC::RelationVCardProperty(InitializeComposerVCard(
                    "Sergei Rachmaninoff", new DateOnly(1873,4,1), new DateOnly(1943,3,28))),
                new VC::RelationVCardProperty(InitializeComposerVCard(
                    "Ludwig van Beethoven", new DateOnly(1770,12,17), new DateOnly(1827,3,26))),
                new VC::RelationVCardProperty(InitializeComposerVCard(
                    "Frédéric Chopin", new DateOnly(1810,3,1), new DateOnly(1849,10,17)))
        };

        var composersVCard = new VCard
        {
            DisplayNames = new VC::TextProperty("Composers"),
            Kind = new VC::KindProperty(VC::Enums.VCdKind.Group),
            Members = members
        };

        // Replace the embedded VCards in composersVCard.Members with Guid references in order
        // to save them as separate vCard 4.0 .VCF files.
        // IMPORTANT: Never call ReferenceVCards() if you intend to serialize a vCard 2.1 or vCard 3.0 !
        IEnumerable<VCard> referenced = composersVCard.ReferenceVCards();
        // (The extension method can be called on a single VCard because VCard implements IEnumerable<VCard>.)

        Console.WriteLine();
        Console.WriteLine($"After ReferenceVCards() vCardList contains {referenced.Count()} VCard objects.");
        Console.WriteLine();
        Console.WriteLine("composersVCard:");
        Console.WriteLine();
        Console.WriteLine(
            referenced
                .Where(x => x.DisplayNames?.Any(x => StringComparer.Ordinal.Equals(x?.Value, "Composers")) ?? false)
                .First()
                .ToVcfString(VCdVersion.V4_0));

        // Make sure to save ALL VCard objects in referenced - otherwise the information
        // originally stored in composersVCard will be irrevocably lost.
        foreach (VCard vcard in referenced)
        {
            string fileName = Path.Combine(
                directoryPath,
                $"{vcard.DisplayNames!.First()!.Value}{vcfExtension}");

            vcard.SaveVcf(fileName, VCdVersion.V4_0);
        }

        // Reload the .VCF files:
        var vCardList = new List<VCard>();

        foreach (string fileName in Directory.EnumerateFiles(directoryPath, $"*{vcfExtension}"))
        {
            vCardList.AddRange(VCard.LoadVcf(fileName));
        }

        // Make the reloaded VCard objects searchable:
        IEnumerable<VCard> dereferenced = vCardList.DereferenceVCards();

        // Find the parsed result from "Composers.vcf":
        composersVCard = dereferenced.FirstOrDefault(x => x.DisplayNames?.Any(x => x?.Value == "Composers") ?? false);

        if (composersVCard is null)
        {
            Console.WriteLine("Composers.vcf not found!");
        }
        else
        {
            //Retrieve Beethovens birth year from the members of the "Composers.vcf" group:
            Console.Write("What year was Beethoven born?: ");

            DateOnly birthDay = DateOnly.MinValue;
            VC::DateAndOrTimeProperty? prop = composersVCard.Members?
                .Select(x => x as VC::RelationVCardProperty)
                .Select(x => x?.Value)
                    .FirstOrDefault(x => x?.DisplayNames?.Any(x => x?.Value == "Ludwig van Beethoven") ?? false)?
                        .BirthDayViews?
                        .FirstOrDefault(x => x?.Value?.TryAsDateOnly(out birthDay) ?? false);

            Console.WriteLine(prop is not null ? birthDay.Year : "Don't know.");
        }
    }


    private static VCard InitializeComposerVCard(string composersName, DateOnly birthDate, DateOnly deathDate)
    {
        var vCard = new VCard
        {
            DisplayNames = new VC::TextProperty(composersName),
            BirthDayViews = VC::DateAndOrTimeProperty.Create(birthDate),
            DeathDateViews = VC::DateAndOrTimeProperty.Create(deathDate)
        };

        return vCard;
    }
}

/*
Console Output:

After ReferenceVCards() vCardList contains 4 VCard objects.

composersVCard:

BEGIN:VCARD
VERSION:4.0
KIND:group
FN:Composers
MEMBER;VALUE=URI:urn:uuid:f3f879b0-b1fb-481d-8a6b-44b8989b1a58
MEMBER;VALUE=URI:urn:uuid:dca80599-cece-4cac-ae9f-a18f08fe6e1b
MEMBER;VALUE=URI:urn:uuid:6ce8b30e-2750-45a1-9e61-4041505400fa
END:VCARD

What year was Beethoven born?: 1770
.
*/
