using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Extensions;

// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples
{
    public static class VCard40Example
    {
        public static void SaveSingleVCardAsVcf(string directoryName)
        {
            const string vcfExtension = ".vcf";

            // Note that argument validation and exception handling is completely omitted in this
            // example. The following "if" statement only ensures, that the method doesn't destroy
            // valueable data.
            if (Directory.GetFiles(directoryName).Any(x => x.EndsWith(vcfExtension, StringComparison.OrdinalIgnoreCase)))
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
                    "Sergei Rachmaninoff", new DateTime(1873,4,1), new DateTime(1943,3,28))),
                new VC::RelationVCardProperty(InitializeComposerVCard(
                    "Ludwig van Beethoven", new DateTime(1770,12,17), new DateTime(1827,3,26))),
                new VC::RelationVCardProperty(InitializeComposerVCard(
                    "Frédéric Chopin", new DateTime(1810,3,1), new DateTime(1849,10,17)))
            };

            var composersVCard = new VCard
            {
                DisplayNames = new VC::TextProperty[] { new VC::TextProperty("Composers") },
                Kind = new VC::KindProperty(VC::Enums.VCdKind.Group),
                Members = members
            };

            var vCardList = new List<VCard>() { composersVCard };

            // Replace the embedded VCards in composersVCard.Members with Guid references in order
            // to save them as separate vCard 4.0 .VCF files.
            // IMPORTANT: Never call ReferenceVCards() if you intend to serialize a vCard 2.1 or vCard 3.0 !
            vCardList.ReferenceVCards();

            Console.WriteLine();
            Console.WriteLine($"After ReferenceVCards() vCardList contains {vCardList.Count} VCard objects.");
            Console.WriteLine();
            Console.WriteLine("composersVCard:");
            Console.WriteLine();
            Console.WriteLine(composersVCard.ToVcfString(VC::Enums.VCdVersion.V4_0));

            // Make sure to save ALL VCard objects in vCard list - otherwise the information
            // originally stored in composersVCard will be irrevocably lost.
            foreach (VCard vcard in vCardList)
            {
                string fileName = Path.Combine(
                    directoryName,
                    $"{vcard.DisplayNames!.First()!.Value}{vcfExtension}");

                vcard.Save(fileName, VC::Enums.VCdVersion.V4_0);
            }

            // Clear the list and reload the .VCF files:
            vCardList.Clear();

            foreach (string fileName in Directory.EnumerateFiles(directoryName, $"*{vcfExtension}"))
            {
                vCardList.AddRange(VCard.Load(fileName));
            }

            // Make the reloaded VCard objects searchable:
            vCardList.DereferenceVCards();

            // Find the parsed result from "Composers.vcf":
            composersVCard = vCardList.FirstOrDefault(x => x.DisplayNames?.Any(x => x?.Value == "Composers") ?? false);

            if (composersVCard is null)
            {
                Console.WriteLine("Composers.vcf not found!");
            }
            else
            {
                //Retrieve Beethovens birth year from the members of the "Composers.vcf" group:
                Console.Write("What year was Beethoven born?: ");

                DateTimeOffset? birthDay = composersVCard.Members?
                    .Select(x => x as VC::RelationVCardProperty)
                    .Where(x => x?.Value != null)
                    .Select(x => x!.Value)
                        .FirstOrDefault(x => x!.DisplayNames?.Any(x => x?.Value == "Ludwig van Beethoven") ?? false)?
                            .BirthDayViews?
                            .Select(x => x as VC::DateTimeOffsetProperty)
                            .Where(x => x != null && !x.IsEmpty)
                            .FirstOrDefault()?
                            .Value;

                Console.WriteLine(birthDay.HasValue ? birthDay.Value.Year.ToString() : "Don't know.");
            }
        }


        private static VCard InitializeComposerVCard(string composersName, DateTime birthDate, DateTime deathDate)
        {
            var vCard = new VCard
            {
                DisplayNames = new VC::TextProperty[] { new VC::TextProperty(composersName) },
                BirthDayViews = new VC::DateTimeOffsetProperty[] { new VC::DateTimeOffsetProperty(birthDate) },
                DeathDateViews = new VC::DateTimeOffsetProperty[] { new VC::DateTimeOffsetProperty(deathDate) }
            };

            return vCard;
        }
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
MEMBER;VALUE=URI:urn:uuid:71cd4a7c-a8de-4a94-bc4d-8f567bc901fb
MEMBER;VALUE=URI:urn:uuid:e7a63731-c8cb-4177-a02b-8ed18864ef48
MEMBER;VALUE=URI:urn:uuid:6a3245b9-3e4d-4baa-9c50-02a0731755fd
END:VCARD

What year was Beethoven born?: 1770
.
*/
