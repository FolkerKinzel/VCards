using System.Collections.ObjectModel;
using FolkerKinzel.DataUrls;
using System.Security.Cryptography;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringBuilderExtension
{
    internal const string NEWLINE_REPLACEMENT = @"\n";

    internal static StringBuilder AppendMasked(this StringBuilder sb, string? s, VCdVersion version)
        => version switch
        {
            VCdVersion.V2_1 => AppendMaskedV2(sb, s),
            VCdVersion.V3_0 => AppendMaskedV3(sb, s),
            _ => AppendMaskedV4(sb, s)
        };

    private static StringBuilder AppendMaskedV2(StringBuilder sb, string? s)
    {
        ReadOnlySpan<char> span = s.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];
            _ = c == ';' ? sb.Append("\\;") : sb.Append(c);
        }//for

        return sb;
    }

    private static StringBuilder AppendMaskedV3(StringBuilder sb, string? s)
    {
        ReadOnlySpan<char> span = s.AsSpan();

        bool rFound = false;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];

            switch (c)
            {
                case '\r':
                    rFound = true;
                    sb.Append(NEWLINE_REPLACEMENT);
                    break;
                case '\n':
                    if (rFound)
                    {
                        rFound = false;
                        continue;
                    }
                    sb.Append(NEWLINE_REPLACEMENT);
                    break;
                case ';':
                    sb.Append("\\;");
                    break;
                case ',':
                    sb.Append("\\,");
                    break;
                default:
                    sb.Append(c);
                    break;
            }//switch

        }//for

        return sb;
    }

    private static StringBuilder AppendMaskedV4(StringBuilder sb, string? s)
    {
        ReadOnlySpan<char> span = s.AsSpan();

        bool rFound = false;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];

            switch (c)
            {
                case '\r':
                    rFound = true;
                    sb.Append(NEWLINE_REPLACEMENT);
                    break;
                case '\n':
                    if (rFound)
                    {
                        rFound = false;
                        continue;
                    }
                    sb.Append(NEWLINE_REPLACEMENT);
                    break;
                case ';':
                    sb.Append("\\;");
                    break;
                case ',':
                    sb.Append("\\,");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                default:
                    sb.Append(c);
                    break;
            }//switch

        }//for

        return sb;
    }

    internal static StringBuilder AppendEscapedAndQuoted(this StringBuilder sb, string s)
    {
        ReadOnlySpan<char> span = s.AsSpan();
        int startIdx = sb.Length;
        bool mustBeQuoted = false;
        bool rFound = false;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];

            switch (c)
            {
                case '\r':
                    rFound = true;
                    sb.Append(NEWLINE_REPLACEMENT);
                    break;
                case '\n':
                    if (rFound)
                    {
                        rFound = false;
                        continue;
                    }
                    sb.Append(NEWLINE_REPLACEMENT);
                    break;
                case '\"':
                    break;
                case ',':
                case ';':
                case ':':
                    mustBeQuoted = true;
                    sb.Append(c);
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return mustBeQuoted ? sb.Insert(startIdx, '\"').Append('\"') // this allocation is very rarely
                            : sb;
    }

    internal static StringBuilder AppendReadableProperty(this StringBuilder sb, ReadOnlyCollection<string> strings, int? maxLen = null)
    {
        Debug.Assert(sb is not null);
        Debug.Assert(strings is not null);
        Debug.Assert(strings.All(x => !string.IsNullOrEmpty(x)));

        // If strings is empty, the loop is not entered:
        for (int i = 0; i < strings.Count; i++)
        {
            AppendEntry(sb, strings[i], maxLen);
        }

        return sb;

        static void AppendEntry(StringBuilder sb, string entry, int? maxLen)
        {
            if (maxLen.HasValue)
            {
                int lineStartIndex = sb.LastIndexOf(Environment.NewLine[0]);
                lineStartIndex = lineStartIndex < 0 ? 0 : lineStartIndex + Environment.NewLine.Length;

                if (sb.Length != 0 && lineStartIndex != sb.Length)
                {
                    _ = sb.Length - lineStartIndex + entry.Length + 1 > maxLen.Value
                        ? sb.AppendLine()
                        : sb.Append(' ');
                }
            }
            else if (sb.Length != 0)
            {
                _ = sb.Append(' ');
            }
            _ = sb.Append(entry);
        }
    }
}
