using System.Globalization;

namespace FolkerKinzel.VCards.Intls;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _Int
{
    public static bool TryParse(ReadOnlySpan<char> value, out int result)
#if NET461 || NETSTANDARD2_0
            => int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
#else
            => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
#endif
}



