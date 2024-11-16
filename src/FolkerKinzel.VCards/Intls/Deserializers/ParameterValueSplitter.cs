using System;
using System.Collections;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Deserializers;

/// <summary>
/// Provides methods for splitting parameter values at ','.
/// </summary>
internal static class ParameterValueSplitter
{
    /// <summary>
    /// Splits a compound parameter value at ',' and unmasks its
    /// parts according to RFC 6868.
    /// </summary>
    /// <param name="mem">The read-only memory region containing the compound
    /// parameter value to split.</param>
    /// <returns>An IEnumerable of strings. Empty values are removed.</returns>
    public static IEnumerable<string> Split(ReadOnlyMemory<char> mem)
    {
        if (mem.IsEmpty)
        {
            yield break;
        }

        while (true)
        {
            ReadOnlySpan<char> span = mem.Span;

            int splitIndex = GetNextSplitIndex(span);

            ReadOnlySpan<char> nextSpan = span.Slice(0, splitIndex).Trim(ParameterSection.TRIM_CHARS);

            if (!nextSpan.IsWhiteSpace())
            {
                yield return nextSpan.UnMaskParameterValue(isLabel: false);
            }

            if (splitIndex == mem.Length)
            {
                yield break;
            }

            mem = mem.Slice(splitIndex + 1);
        }
    }

    /// <summary>
    /// Splits a compound parameter value at ','.
    /// </summary>
    /// <param name="mem">The read-only memory region containing the compound
    /// parameter value to split.</param>
    /// <returns>An IEnumerable of read-only character memory regions. The IEnumerable
    /// can contain empty memories.</returns>
    public static IEnumerable<ReadOnlyMemory<char>> SplitIntoMemories(ReadOnlyMemory<char> mem)
    {
        if (mem.IsEmpty)
        {
            yield break;
        }

        while (true)
        {
            int splitIndex = GetNextSplitIndex(mem.Span);
            ReadOnlyMemory<char> nextChunk = mem.Slice(0, splitIndex);

            yield return nextChunk;

            if (splitIndex == mem.Length)
            {
                yield break;
            }

            mem = mem.Slice(splitIndex + 1);
        }
    }

    private static int GetNextSplitIndex(ReadOnlySpan<char> span)
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

            if (c == ',')
            {
                return i;
            }

        }//for

        return span.Length;
    }
}


