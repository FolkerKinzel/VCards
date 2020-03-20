using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls
{
    /// <summary>
    /// Eine <see cref="VcfRow"/> stellt eine zusammengehörende Datenzeile der Vcf-Datei dar.
    /// </summary>
    partial class VcfRow
    {
        // Bsp. Vcf-Datenzeile:
        // item1.ADR;TYPE=HOME,WORK;PREF=1:;;Waldstr. 54;Kleinknuffelsdorf;Sachsen-Anhalt;06789;Germany


        public readonly string? Group; // Group im obigen Beispiel ist "item1"


        public readonly string Key; // Key im Beispiel is "ADR"



        // geparste Form der Parameter ;TYPE=HOME,WORK;PREF=1
        public readonly ParameterSection Parameters;


        // Value: ;;Waldstr. 54;Kleinknuffelsdorf;Sachsen-Anhalt;06789;Germany
        public string? Value { get; private set; }





    }
}