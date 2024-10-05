using System.Text;
using FolkerKinzel.VCards;

string vcf = new StringBuilder()
   .AppendLine("BEGIN:VCARD")
   .AppendLine("VERSION:3.0")
   .AppendLine("FN:Folker")
   .AppendLine("END:VCARD")
   .ToString();

using TextReader stringReader = new StringReader(vcf);
using var vcfReader = new VcfReader(stringReader);

IEnumerable<FolkerKinzel.VCards.VCard> result = vcfReader.ReadToEnd();

var parsedVCard = result.First();

Console.WriteLine(parsedVCard);
