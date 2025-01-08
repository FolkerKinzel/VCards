using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Deserializers;

/// <summary>Represents a data row in the VCF file.</summary>
internal sealed partial class VcfRow
{
    // Bsp. Vcf-Datenzeile:
    // item1.ADR;TYPE=HOME,WORK;PREF=1:;;Waldstr. 54;Kleinknuffelsdorf;Sachsen-Anhalt;06789;Germany

    public readonly string? Group; // Group is "item1" in the example

    public readonly string Key; // Key is "ADR" in the example

    // ;TYPE=HOME,WORK;PREF=1
    public readonly ParameterSection Parameters;

    // Value: ;;Waldstr. 54;Kleinknuffelsdorf;Sachsen-Anhalt;06789;Germany
    public readonly ReadOnlyMemory<char> Value;

    //public readonly VcfDeserializationInfo Info;
}
