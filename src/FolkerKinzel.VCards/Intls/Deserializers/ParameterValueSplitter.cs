using System.Collections;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal static class ParameterValueSplitter
{
    public static IEnumerable<string> Split(ReadOnlyMemory<char> mem,
                                            char splitChar,
                                            StringSplitOptions options,
                                            bool unMask)
    {
        if (mem.IsEmpty)
        {
            if (!options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
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
                             ? nextChunk.Span.UnMaskParameterValue(isLabel: false)
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
        if (mem.IsEmpty)
        {
            yield break;
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
            char c = span[i];

            if (c == '\"')
            {
                masked = !masked;
                continue;
            }

            if (masked)
            {
                continue;
            }

            if (c == splitChar)
            {
                return i;
            }

        }//for

        return span.Length;
    }
}


