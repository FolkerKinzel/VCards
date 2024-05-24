using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static partial class StringExtension
{
    // The regex isn't perfect but finds most IETF language tags:
    private const string IETF_LANGUAGE_TAG_PATTERN = @"^[a-z]{2,3}-[A-Z]{2,3}$";

    internal static string UnMask(this ReadOnlySpan<char> value, VCdVersion version)
    {
        Debug.Assert(value != null);

        int idxOfBackSlash = value.IndexOf('\\');

        if (idxOfBackSlash == -1)
        {
            return value.ToString();
        }

        int processedLength = value.Length - idxOfBackSlash;

        if (processedLength > Const.STACKALLOC_CHAR_THRESHOLD)
        {
            using ArrayPoolHelper.SharedArray<char> buf = ArrayPoolHelper.Rent<char>(processedLength);
            Span<char> bufSpan = buf.Array.AsSpan();
            return CreateUnMaskedString(version, value, idxOfBackSlash, bufSpan);
        }
        else
        {
            Span<char> bufSpan = stackalloc char[processedLength];
            return CreateUnMaskedString(version, value, idxOfBackSlash, bufSpan);
        }

        static string CreateUnMaskedString(VCdVersion version, ReadOnlySpan<char> sourceSpan, int idxOfBackSlash, Span<char> bufSpan)
        {
            ReadOnlySpan<char> processedSpan = sourceSpan.Slice(idxOfBackSlash);

            int outputLength = version switch
            {
                VCdVersion.V2_1 => UnMask21(processedSpan, bufSpan),
                VCdVersion.V3_0 => UnMask30(processedSpan, bufSpan),
                _ => UnMask40(processedSpan, bufSpan)
            };

            bufSpan = bufSpan.Slice(0, outputLength);

            return idxOfBackSlash == 0 ? bufSpan.ToString()
                                       : StaticStringMethod.Concat(sourceSpan.Slice(0, idxOfBackSlash), bufSpan);
        }
    }

    private static int UnMask21(ReadOnlySpan<char> source, Span<char> destination)
    {
        int lastSourceIdx = source.Length - 1;
        int length = 0;

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = source[i];

            if (c == '\\' && source[i + 1] == ';')
            {
                continue;
            }

            destination[length++] = c;
        }

        destination[length++] = source[lastSourceIdx];
        return length;
    }

    private static int UnMask30(ReadOnlySpan<char> source, Span<char> destination)
    {
        int lastSourceIdx = source.Length - 1;
        int length = 0;

        var newLineSpan = Environment.NewLine.AsSpan();

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = source[i];

            if (c == '\\')
            {
                switch (source[i + 1])
                {
                    case 'n':
                    case 'N':
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case ',':
                    case ';':
                        continue;
                    default:
                        break;
                }
            }

            destination[length++] = c;
        }

        destination[length++] = source[lastSourceIdx];
        return length;
    }

    private static int UnMask40(ReadOnlySpan<char> source, Span<char> destination)
    {
        int lastSourceIdx = source.Length - 1;
        int length = 0;

        var newLineSpan = Environment.NewLine.AsSpan();

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = source[i];

            if (c == '\\')
            {
                switch (source[i + 1])
                {
                    case 'n':
                    case 'N':
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if(i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case ',':
                    case ';':
                        continue;
                    case '\\':
                        destination[length++] = c;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    default:
                        break;
                }
            }

            destination[length++] = c;
        }

        destination[length++] = source[lastSourceIdx];

        return length;
    }


    /// <summary> Indicates whether <paramref name="s" /> needs 
    /// Quoted-Printable encoding.</summary>
    /// <param name="s">The <see cref="string" /> to check, or <c>null</c>.</param>
    /// <returns><c>true</c>, if <paramref name="s"/> contains NON-ASCII
    /// characters or line breaks, otherwise <c>false</c>.
    /// If <paramref name="s"/> is <c>null</c>, the method returns <c>false</c>.</returns>
    public static bool NeedsToBeQpEncoded(this string? s)
    {
        if (s is null)
        {
            return false;
        }

        var span = s.AsSpan();

        if (span.ContainsNewLine())
        {
            return true;
        }

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] > 126)
            {
                return true;
            }
        }

        return false;
    }

    [ExcludeFromCodeCoverage]
    internal static bool IsIetfLanguageTag(this string value)
#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0 || NET6_0
    {
        try
        {
            return Regex.IsMatch(value, IETF_LANGUAGE_TAG_PATTERN, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(50));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
#else
    {
        try
        {
            return IetfLanguageTagRegex().IsMatch(value);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    [GeneratedRegex(IETF_LANGUAGE_TAG_PATTERN, RegexOptions.CultureInvariant, 50)]
    private static partial Regex IetfLanguageTagRegex();
#endif

}
