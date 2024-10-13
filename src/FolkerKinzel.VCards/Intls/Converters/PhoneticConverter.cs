using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class PhoneticConverter
{
    private static class TypeValue
    {
        internal const string IPA = "ipa";
        internal const string JYUT = "jyut";
        internal const string PINY = "piny";
        internal const string SCRIPT = "script";
    }

    internal static Phonetic? Parse(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(TypeValue.IPA, comp) ? Phonetic.Ipa
             : typeValue.Equals(TypeValue.PINY, comp) ? Phonetic.Piny
             : typeValue.Equals(TypeValue.JYUT, comp) ? Phonetic.Jyut
             : typeValue.Equals(TypeValue.SCRIPT, comp) ? Phonetic.Script
             : null;
    }

    internal static string? ToVcfString(this Phonetic? value)
        => value switch
        {
            Phonetic.Piny => TypeValue.PINY,
            Phonetic.Ipa => TypeValue.IPA,
            Phonetic.Script => TypeValue.SCRIPT,
            Phonetic.Jyut => TypeValue.JYUT,
            _ => null,
        };
}
