using System.Globalization;

namespace FolkerKinzel.VCards.Intls;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _Int
{
//    /// <summary>
//    /// Parses an <see cref="int"/>.
//    /// </summary>
//    /// <param name="value"></param>
//    /// <returns></returns>
//    /// <exception cref="FormatException"></exception>
//    public static int Parse(ReadOnlySpan<char> value)
//    {
//        try
//        {
//#if NET461 || NETSTANDARD2_0
//            return int.Parse(value.ToString(), NumberStyles.Integer);
//#else
//        return int.Parse(value, NumberStyles.Integer);
//#endif
//        }
//        catch (OverflowException e)
//        {
//            throw new FormatException(e.Message, e);
//        }
//    }

    /// <summary>
    /// Tries to parse an <see cref="int"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static bool TryParse(ReadOnlySpan<char> value, out int result)
#if NET461 || NETSTANDARD2_0
            => int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
#else
            => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
#endif
}



