using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;

namespace Examples;

/// <summary>
/// This example will save a vCard 4.0 Group-vCard and its members in separate
/// VCF files and will reload all these files and then read data from the group VCard.
/// Although the example shows that this is possible, it is generally recommended 
/// to save group VCards and their members in a common VCF file: In this case, the 
/// Reference and Dereference methods would not have to be called in own code.
/// </summary>
public static class VCard40Example
{
    public static void SaveSingleVCardAsVcf(string directoryPath)
    {
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
            .VCard;

        // (The easiest way would be to save everything in a common VCF file:)
        // composersVCard.SaveVcf("CommonFilePath.vcf", VCdVersion.V4_0);

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
                .Where(x => x.DisplayNames
                             .Items()
                             .Any(x => StringComparer.Ordinal.Equals(x.Value, "Composers")))
                .First()
                .ToVcfString(VCdVersion.V4_0));

        // Make sure to save ALL VCard objects in `referenced` - otherwise
        // the information originally stored in `composersVCard` will be
        // irrevocably lost.
        foreach (VCard vCard in referenced)
        {
            string fileName = Path.Combine(
                directoryPath,
                $"{vCard.DisplayNames?.First()?.Value ?? "unknown"}{vcfExtension}");

            vCard.SaveVcf(fileName, VCdVersion.V4_0);
        }

        // Reload the .VCF files:
        IEnumerable<VCard> vCards =
            Vcf.LoadMany(Directory.EnumerateFiles(directoryPath, $"*{vcfExtension}"));

        // Make the reloaded VCard objects searchable. (The Dereference method doesn't
        // change anything in vCards. Don't forget to assign the return value!):
        vCards.Dereference();

        // Find the parsed result from "Composers.vcf":
        composersVCard = vCards
            .FirstOrDefault
             (
              x => x.DisplayNames.Items().Any(x => x.Value == "Composers")
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
                       .VCard;

    private static bool TryFindBeethovensBirthday(VCard composersVCard, out DateOnly birthDay)
    {
        DateOnly date = default;
        bool found = composersVCard.Members
                .OrderByPref()
                .Select(x => x.Value.VCard)
                .OfType<VCard>()
                .FirstOrDefault(x => x.DisplayNames
                                      .Items()
                                      .Any(x => x.Value == "Ludwig van Beethoven"))?
                    .BirthDayViews
                    .FirstOrNull(x => x.Value.TryAsDateOnly(out date))
                     is not null;

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
CREATED;VALUE=TIMESTAMP:20241101T222925Z
REV:20241101T222930Z
UID:urn:uuid:2a842cde-fcef-4dd1-8ddf-d8bdd5311c8a
FN:Composers
MEMBER;VALUE=URI:urn:uuid:f289f7a3-cff1-4218-917d-4b658327d01f
MEMBER;VALUE=URI:urn:uuid:1f2ebe9f-ae42-4fd9-8ff8-3abcfd58f4bb
MEMBER;VALUE=URI:urn:uuid:333eeeaf-b39c-49ce-8320-89154af2f8ad
END:VCARD

What year was Beethoven born?: 1770
*/