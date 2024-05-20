using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringBuilderExtension
{
    internal const string NEWLINE_REPLACEMENT = @"\n";

    internal static StringBuilder UnMask(this StringBuilder builder, VCdVersion version)
    {
        Debug.Assert(builder is not null);

        return version switch
        {
            VCdVersion.V2_1 => builder.UnMaskV2(),
            VCdVersion.V3_0 => builder.UnMaskV3(),
            _ => builder.UnMaskV4()
        };
    }

    private static StringBuilder UnMaskV2(this StringBuilder builder) => builder.Replace(@"\;", ";");

    private static StringBuilder UnMaskV3(this StringBuilder builder)
    {
        return builder
            .Replace(@"\n", Environment.NewLine)
            .Replace(@"\N", Environment.NewLine)
            .Replace(@"\,", ",")
            .Replace(@"\;", ";");
    }

    private static StringBuilder UnMaskV4(this StringBuilder builder)
    {
        bool masked = false;
        for (int i = 0; i < builder.Length; i++)
        {
            if (builder[i] == '\\')
            {
                if (!masked)
                {
                    builder.Remove(i, 1);
                    masked = true;
                    --i;
                    continue;
                }

                masked = false;
                continue;
            }

            if (masked)
            {
                masked = false;
                switch (builder[i])
                {
                    case 'n':
                    case 'N':
                        builder.Remove(i, 1);
                        builder.Insert(i, Environment.NewLine);
                        i += Environment.NewLine.Length - 1;
                        break;
                    default:
                        break;
                }
            }
        }

        return builder;
    }

    internal static StringBuilder AppendMasked(this StringBuilder sb, string? s, VCdVersion version)
    {
        var span = s.AsSpan();

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
                    if (version > VCdVersion.V2_1)
                    {
                        sb.Append('\\');
                    }

                    sb.Append(c);
                    break;
                case '\\':
                    if (version >= VCdVersion.V4_0)
                    {
                        sb.Append('\\');
                    }
                    sb.Append(c);
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
            var span = s.AsSpan();
            int startIdx = sb.Length;
            sb.Append('\"');
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

            return mustBeQuoted ? sb.Append('"')
                                : sb.Remove(startIdx, 1);
        }

        internal static StringBuilder AppendReadableProperty(this StringBuilder sb, ReadOnlyCollection<string> strings, int? maxLen = null)
        {
            Debug.Assert(sb is not null);
            Debug.Assert(strings is not null);
            Debug.Assert(strings.All(x => !string.IsNullOrEmpty(x)));

            // Wenn strings leer ist, wird die Schleife nicht gestartet:
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
