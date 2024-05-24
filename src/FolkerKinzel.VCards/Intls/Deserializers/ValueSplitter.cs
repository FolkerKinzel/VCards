using System.Collections;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal static class ValueSplitter2
{
    public static IEnumerable<string> Split(ReadOnlyMemory<char> mem,
                                            char splitChar,
                                            StringSplitOptions options,
                                            bool unMask,
                                            VCdVersion version)
    {
        if (mem.IsEmpty)
        {
            if(!options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
            {
                yield return string.Empty;
            }

            yield break;
        }

        while (true)
        {
            int splitIndex = GetNextSplitIndex(mem.Span, splitChar);

            ReadOnlyMemory<char> nextChunk = mem.Slice(0, splitIndex);

            if (!options.HasFlag(StringSplitOptions.RemoveEmptyEntries) || !nextChunk.Span.IsWhiteSpace())
            {
                yield return unMask
                             ? nextChunk.Span.UnMask(version)
                             : nextChunk.ToString();
            }

            if (splitIndex == mem.Length)
            {
                yield break;
            }

            mem = mem.Slice(splitIndex + 1);
        }
    }

    public static IEnumerable<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> mem,
                                                          char splitChar)
    {
        if(mem.IsEmpty)
        {
            yield break ;
        }

        while (true)
        {
            int splitIndex = GetNextSplitIndex(mem.Span, splitChar);
            ReadOnlyMemory<char> nextChunk = mem.Slice(0, splitIndex);

            yield return nextChunk;

            if (splitIndex == mem.Length)
            {
                yield break;
            }

            mem = mem.Slice(splitIndex + 1);
        }
    }

    private static int GetNextSplitIndex(ReadOnlySpan<char> span, char splitChar)
    {
        bool masked = false;

        for (int i = 0; i < span.Length; i++)
        {
            if (masked)
            {
                masked = false;
                continue;
            }

            char c = span[i];

            if (c == splitChar)
            {
                return i;
            }

            if (c == '\\')
            {
                masked = true;
            }

        }//for

        return span.Length;
    }
}


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

                if (!removeEmptyEntries || !valueString.AsSpan(i, length).IsWhiteSpace())
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
        ReadOnlySpan<char> s = ValueString.AsSpan();
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
}


