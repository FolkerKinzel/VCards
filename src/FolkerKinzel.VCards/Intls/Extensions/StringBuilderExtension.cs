using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringBuilderExtension
{
    internal const string NEWLINE_REPLACEMENT = @"\n";

    /// <summary>
    /// Appends a vCard-property value masked to a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/>.</param>
    /// <param name="propVal">The vCard-property value.</param>
    /// <param name="version">The vCard version to use for masking.</param>
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    internal static StringBuilder AppendValueMasked(this StringBuilder builder, string? propVal, VCdVersion version)
        => version switch
        {
            VCdVersion.V2_1 => AppendValueMaskedV2(builder, propVal),
            VCdVersion.V3_0 => AppendValueMaskedV3(builder, propVal),
            _ => AppendValueMaskedV4(builder, propVal)
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


    /// <summary>
    /// Appends a vCard parameter value to a <see cref="StringBuilder"/> an quotes and masks it
    /// if necessary.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/>.</param>
    /// <param name="paramVal">The vCard parameter value to append.</param>
    /// <param name="version">The vCard version to use for masking.</param>
    /// <param name="isLabel">Pass <c>true</c> if <paramref name="paramVal"/> is the value of a <c>LABEL</c>
    /// parameter.</param>
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    internal static StringBuilder AppendParameterValueEscapedAndQuoted(this StringBuilder builder,
                                                                       string paramVal,
                                                                       VCdVersion version,
                                                                       bool isLabel = false)
    {
        Debug.Assert(version != VCdVersion.V2_1);

        int startIdx = builder.Length;
        return (version == VCdVersion.V3_0 ? AppendParameterValueEscapedV30(builder, paramVal) : AppendParameterValueEscapedV40(builder, paramVal, isLabel))
                ? builder.Insert(startIdx, '\"').Append('\"') // this allocation is very rarely
                : builder;
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

    /// <summary>
    /// Appends <paramref name="paramVal"/> masked and quoted according to RFC 6868
    /// and makes a special treatment for line breaks in <c>LABEL</c> parameters.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/>.</param>
    /// <param name="paramVal">The parameter value to append.</param>
    /// <param name="isLabel"><c>true</c> if <paramref name="paramVal"/> is the value
    /// of a <c>LABEL</c> parameter.</param>
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// <remarks>
    /// <para>
    /// RFC 6350 6.3.1 states that newlines in LABEL parameters "are encoded
    /// as \n, as they are for property values". 
    /// </para>
    /// <para>
    /// According to the suggestion in the StackOverflow question 
    /// </para>
    /// <para>
    /// https://stackoverflow.com/questions/20508633/how-to-encode-newlines-in-vcard-4-0-parameter-values-n-or-n
    /// </para>
    /// <para>
    /// there is made a special treatment for newlines in <c>LABEL</c> parameters.
    /// Ev'rything else is encoded according to RFC 6868.
    /// </para>
    /// </remarks>
    private static bool AppendParameterValueEscapedV40(StringBuilder builder,
                                                       string paramVal,
                                                       bool isLabel)
    {
        ReadOnlySpan<char> span = paramVal.AsSpan();
        
        bool mustBeQuoted = false;
        bool rFound = false;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];

            switch (c)
            {
                case '\r':
                    rFound = true;
                    builder.Append(isLabel ? NEWLINE_REPLACEMENT : "^n");
                    break;
                case '\n':
                    if (rFound)
                    {
                        rFound = false;
                        continue;
                    }
                    builder.Append(isLabel ? NEWLINE_REPLACEMENT : "^n");
                    break;
                case '\"':
                    builder.Append("^'");
                    break;
                case ',':
                case ';':
                case ':':
                    mustBeQuoted = true;
                    builder.Append(c);
                    break;
                case '^':
                    builder.Append("^^");
                    break;
                default:
                    builder.Append(c);
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
