using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringBuilderExtension
{
    internal const string NEWLINE_REPLACEMENT = @"\n";

    internal static StringBuilder AppendValueMasked(this StringBuilder sb, string? s, VCdVersion version)
        => version switch
        {
            VCdVersion.V2_1 => AppendValueMaskedV2(sb, s),
            VCdVersion.V3_0 => AppendValueMaskedV3(sb, s),
            _ => AppendValueMaskedV4(sb, s)
        };

    private static StringBuilder AppendValueMaskedV2(StringBuilder sb, string? s)
    {
        ReadOnlySpan<char> span = s.AsSpan();

        // Line breaks should be quoted-printable encoded
        Debug.Assert(!span.ContainsNewLine());

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];
            _ = c == ';' ? sb.Append("\\;") : sb.Append(c);
        }//for

        return sb;
    }

    private static StringBuilder AppendValueMaskedV3(StringBuilder sb, string? s)
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
                    // RFC 2426 2.4.2 says that the COLON (ASCII decimal 58)
                    // MUST be escaped with a BACKSLASH, but the associated
                    // example doesn't
                //case ':':
                //    sb.Append("\\:");
                //    break;
                default:
                    sb.Append(c);
                    break;
            }//switch

        }//for

        return sb;
    }

    private static StringBuilder AppendValueMaskedV4(StringBuilder sb, string? s)
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

    internal static StringBuilder AppendParameterValueEscapedAndQuoted(this StringBuilder sb,
                                                         string s,
                                                         VCdVersion version,
                                                         bool isLabel = false)
    {
        int startIdx = sb.Length;
        return (version == VCdVersion.V3_0 ? AppendParameterValueEscapedV30(sb, s) : AppendParameterValueEscapedV40(sb, s, isLabel))
                ? sb.Insert(startIdx, '\"').Append('\"') // this allocation is very rarely
                : sb;
    }

    private static bool AppendParameterValueEscapedV30(StringBuilder sb, string s)
    {
        ReadOnlySpan<char> span = s.AsSpan();

        bool mustBeQuoted = false;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];

            if (c == '\"' || char.IsControl(c))
            {
                continue;
            }

            if(c is ';' or ',' or ':')
            {
                mustBeQuoted = true;
            }

            sb.Append(c);
        }

        return mustBeQuoted;
    }

    private static bool AppendParameterValueEscapedV40(StringBuilder sb, string s, bool isLabel)
    {
        ReadOnlySpan<char> span = s.AsSpan();
        
        bool mustBeQuoted = false;
        bool rFound = false;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];

            switch (c)
            {
                case '\r':
                    rFound = true;
                    sb.Append(isLabel ? NEWLINE_REPLACEMENT : "^n");
                    break;
                case '\n':
                    if (rFound)
                    {
                        rFound = false;
                        continue;
                    }
                    sb.Append(isLabel ? NEWLINE_REPLACEMENT : "^n");
                    break;
                case '\"':
                    sb.Append("^'");
                    break;
                case ',':
                case ';':
                case ':':
                    mustBeQuoted = true;
                    sb.Append(c);
                    break;
                case '^':
                    sb.Append("^^");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return mustBeQuoted;
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
