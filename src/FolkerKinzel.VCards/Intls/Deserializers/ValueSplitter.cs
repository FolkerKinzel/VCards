using System.Collections;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal class ValueSplitter : IEnumerable<string>
{
    internal ValueSplitter(char splitChar, StringSplitOptions options)
    {
        this.SplitChar = splitChar;
        this.Options = options;
    }

    internal string? ValueString { get; set; }

    internal char SplitChar { get; }

    internal StringSplitOptions Options { get; }

    public IEnumerator<string> GetEnumerator()
    {
        if (ValueString is null)
        {
            yield break;
        }

        int i = 0;
        string valueString = ValueString;
        int valueStringLength = valueString.Length;
        bool removeEmptyEntries = Options.HasFlag(StringSplitOptions.RemoveEmptyEntries);

        while (i <= valueStringLength)
        {
            int splitIndex = GetNextSplitIndex(i);

            if (splitIndex == i)
            {
                if (!removeEmptyEntries)
                {
                    yield return string.Empty;
                }
            }
            else
            {
                int length = splitIndex - i;

                if (!removeEmptyEntries || ContainsData(i, length, valueString))
                {
                    yield return length == valueStringLength
                                    ? valueString
                                    : valueString.Substring(i, length);
                }
            }

            i = splitIndex + 1;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private int GetNextSplitIndex(int startIndex)
    {
        string s = ValueString!;
        char splitChar = SplitChar;
        bool masked = false;

        for (int i = startIndex; i < s.Length; i++)
        {
            if (masked)
            {
                masked = false;
                continue;
            }

            char c = s[i];

            if (c == splitChar)
            {
                return i;
            }

            if (c == '\\')
            {
                masked = true;
            }

        }//for

        return s.Length;
    }

    private static bool ContainsData(int startIndex, int length, string s)
    {
        for (int i = 0; i < length; i++)
        {
            if (char.IsWhiteSpace(s[i + startIndex]))
            {
                continue;
            }

            return true;
        }

        return false;
    }
}
