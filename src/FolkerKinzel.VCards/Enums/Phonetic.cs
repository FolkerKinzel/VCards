namespace FolkerKinzel.VCards.Enums;

/// <summary>
/// Named constants to specify the phonetic system. (4 - RFC 9554)
/// </summary>
public enum Phonetic
{
    // CAUTION: If the enum is expanded, PhoneticConverter must be updated

    /// <summary> <c>ipa</c> International Phonetic Alphabet (4 - RFC 9554)</summary>
    Ipa,

    /// <summary> <c>jyut</c> Cantonese romanization system "Jyutping" (4 - RFC 9554)</summary>
    Jyut,

    /// <summary> <c>piny</c> Standard Mandarin romanization system "Hanyu Pinyin" (4 - RFC 9554)</summary>
    Piny,

    /// <summary> <c>script</c> Unknown phonetic system (4 - RFC 9554)</summary>
    Script
}
