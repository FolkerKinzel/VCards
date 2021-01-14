using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Deserializers
{
    /// <summary>
    /// Eine <see cref="VcfRow"/> stellt eine zusammengehörende Datenzeile der Vcf-Datei dar.
    /// </summary>
    internal sealed partial class VcfRow
    {
        // Bsp. Vcf-Datenzeile:
        // item1.ADR;TYPE=HOME,WORK;PREF=1:;;Waldstr. 54;Kleinknuffelsdorf;Sachsen-Anhalt;06789;Germany


        public readonly string? Group; // Group im obigen Beispiel ist "item1"


        public readonly string Key; // Key im Beispiel is "ADR"



        // geparste Form der Parameter ;TYPE=HOME,WORK;PREF=1
        public readonly ParameterSection Parameters;

        public readonly VCardDeserializationInfo Info;


        // Value: ;;Waldstr. 54;Kleinknuffelsdorf;Sachsen-Anhalt;06789;Germany
        public string? Value { get; private set; }


        ///// <summary>
        ///// Wird nur beim Lesen von vCard 2.1 benötigt, um fehlende Base64-Daten nachträglich hinzuzufügen.
        ///// </summary>
        ///// <param name="newValue">Der neue Wert für <see cref="Value"/>.</param>
        //internal void SetValue(string newValue) => Value = newValue;





    }
}