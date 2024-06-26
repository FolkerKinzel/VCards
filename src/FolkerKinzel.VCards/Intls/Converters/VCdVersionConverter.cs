using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class VCdVersionConverter
{
    internal static VCdVersion Parse(ReadOnlySpan<char> value)
    {
        return value.IsEmpty
            ? VCdVersion.V2_1
            : value[0] switch
            {
                '4' => VCdVersion.V4_0,
                '3' => VCdVersion.V3_0,
                _ => VCdVersion.V2_1
            };
    }
}
