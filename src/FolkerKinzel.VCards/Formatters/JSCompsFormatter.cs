using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Formatters;

public static class JSCompsFormatter
{
    private const int NOT_FOUND = -1;
    private const int SEPARATOR_INDICATOR_LENGTH = 2;

    public static bool TryFormat(ICompoundProperty property, [NotNullWhen(true)] out string? formatted)
    {
        _ArgumentNullException.ThrowIfNull(property, nameof(property));

        ReadOnlySpan<char> jsComps = property.Parameters.ComponentOrder.AsSpan();
        formatted = null;

        if (jsComps.IsEmpty)
        {
            return false;
        }

        int length = GetSemicolon(jsComps);

        if (length == NOT_FOUND) { return false; }

        List<string> list = [];

        try
        {
            string defaultSeparator = length == 0 ? " " : ParseSeparator(jsComps.Slice(0, length));

            bool addDefaultSeparator = false;

            while (length != NOT_FOUND)
            {
                jsComps = jsComps.Slice(length);
                length = GetSemicolon(jsComps);

                ReadOnlySpan<char> chunk = length == NOT_FOUND ? jsComps : jsComps.Slice(0,length);

                if (IsSeparator(jsComps))
                {
                    addDefaultSeparator = false;
                    list.Add(ParseSeparator(chunk));
                }
                else
                {
                    if(addDefaultSeparator)
                    {
                        list.Add(defaultSeparator);
                    }

                    addDefaultSeparator = true;

                    list.Add(ParseToken(chunk, property));
                }
            }
        }
        catch
        {
            return false;
        }

        formatted = string.Concat(list);

        return true;
    }

    private static string ParseToken(ReadOnlySpan<char> readOnlySpan, ICompoundProperty prop)
    {
        int commaIdx = readOnlySpan.IndexOf(',');
        return commaIdx == NOT_FOUND
            ? prop[_Int.Parse(readOnlySpan)][0]
            : prop[_Int.Parse(readOnlySpan.Slice(0, commaIdx))][_Int.Parse(readOnlySpan.Slice(commaIdx + 1))];
    }

    private static int GetSemicolon(ReadOnlySpan<char> jsComps)
    {
        int idx = jsComps.IndexOf(';');

        if (idx > 0)
        {
            if (jsComps[idx - 1] == '\\')
            {
                int tmp = GetSemicolon(jsComps.Slice(idx + 1));
                return tmp == NOT_FOUND ? NOT_FOUND : tmp + idx + 1;
            }
        }

        return idx;
    }

    private static bool IsSeparator(ReadOnlySpan<char> jsComps) => jsComps.StartsWith('s');

    private static string ParseSeparator(ReadOnlySpan<char> jsComps)
        => jsComps.Slice(SEPARATOR_INDICATOR_LENGTH).UnMaskValue(VCdVersion.V4_0);
}


