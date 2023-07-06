using System.Text;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringBuilderExtension
{
    internal const string NEWLINE_REPLACEMENT = @"\n";

    internal static StringBuilder UnMask(this StringBuilder builder, VCdVersion version)
    {
        Debug.Assert(builder != null);

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


    internal static StringBuilder Mask(this StringBuilder builder, VCdVersion version)
    {
        Debug.Assert(builder != null);

        if (version == VCdVersion.V2_1)
        {
            _ = builder.Replace(";", @"\;");
            return builder;
        }

        if (version >= VCdVersion.V4_0)
        {
            _ = builder
                .Replace(@"\", @"\\");
        }

        _ = builder
            .Replace(Environment.NewLine, NEWLINE_REPLACEMENT)
            .Replace(",", @"\,")
            .Replace(";", @"\;");

        return builder;
    }


    internal static StringBuilder MaskNewLine(this StringBuilder builder)
    {
        _ = builder
                .Replace(@"\", @"\\")
                .Replace(Environment.NewLine, NEWLINE_REPLACEMENT);

        return builder;
    }

#if NET40
        /// <summary>
        /// Entfernt führenden und nachgestellten Leerraum vom Inhalt
        /// von <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">Der <see cref="StringBuilder"/>, dessen Inhalt verändert wird.</param>
        /// <returns>Gibt <paramref name="builder"/> zurück, damit Aufrufe verkettet werden können.</returns>
        internal static StringBuilder Trim(this StringBuilder builder)
        {
            Debug.Assert(builder != null);

            while (builder.Length >= 1 && char.IsWhiteSpace(builder[0]))
            {
                _ = builder.Remove(0, 1);
            }

            while (builder.Length >= 1 && char.IsWhiteSpace(builder[builder.Length - 1]))
            {
                _ = builder.Remove(builder.Length - 1, 1);
            }

            return builder;
        }


        internal static StringBuilder TrimEnd(this StringBuilder builder)
        {
            while (builder.Length >= 1 && char.IsWhiteSpace(builder[builder.Length - 1]))
            {
                _ = builder.Remove(builder.Length - 1, 1);
            }

            return builder;
        }

        internal static StringBuilder ToLowerInvariant(this StringBuilder builder)
        {
            Debug.Assert(builder != null);

            for (int i = 0; i < builder.Length; i++)
            {
                builder[i] = char.ToLowerInvariant(builder[i]);
            }

            return builder;
        }
#endif

    /// <summary>
    /// Entfernt einfache und doppelte Gänsefüßchen, die sich am Beginnn oder Ende des Inhalts
    /// von <paramref name="builder"/> befinden.
    /// </summary>
    /// <param name="builder">Der <see cref="StringBuilder"/>, dessen Inhalt verändert wird.</param>
    /// <returns>Gibt <paramref name="builder"/> zurück, damit Aufrufe verkettet werden können.</returns>
    internal static StringBuilder RemoveQuotes(this StringBuilder builder)
    {
        Debug.Assert(builder != null);

        while (builder.Length >= 1 && (builder[0] == '\"' || builder[0] == '\''))
        {
            _ = builder.Remove(0, 1);
        }

        while (builder.Length >= 1 && (builder[builder.Length - 1] == '\"' || builder[builder.Length - 1] == '\''))
        {
            _ = builder.Remove(builder.Length - 1, 1);
        }

        return builder;
    }

}
