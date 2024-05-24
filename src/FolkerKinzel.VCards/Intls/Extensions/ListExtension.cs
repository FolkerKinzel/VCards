using System.Runtime.InteropServices;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class ListExtension
{
#if NET8_0_OR_GREATER

    internal static ReadOnlyMemory<char> Concat(this List<ReadOnlyMemory<char>> list)
    {
        Debug.Assert(list != null);

        return list.Count switch
        {
            0 => default,
            1 => list[0],
            _ => StaticStringMethod.Concat(CollectionsMarshal.AsSpan(list)).AsMemory()
        };
    }
#else
    internal static ReadOnlyMemory<char> Concat(this List<ReadOnlyMemory<char>> list)
    {
        Debug.Assert(list != null);

        return list.Count switch
        {
            0 => default,
            1 => list[0],
            2 => StaticStringMethod.Concat(list[0].Span, list[1].Span).AsMemory(),
            3 => StaticStringMethod.Concat(list[0].Span, list[1].Span, list[2].Span).AsMemory(),
            4 => StaticStringMethod.Concat(list[0].Span, list[1].Span, list[2].Span, list[3].Span).AsMemory(),
            _ => DoConcat(list).AsMemory()
        };
    }

    private static string DoConcat(List<ReadOnlyMemory<char>> values)
    {
        int length = 0;

        for (int i = 0; i < values.Count; i++)
        {
            length += values[i].Length;
        }

        if (length == 0)
        {
            return string.Empty;
        }

        if (length > Const.STACKALLOC_CHAR_THRESHOLD)
        {
            using ArrayPoolHelper.SharedArray<char> buf = ArrayPoolHelper.Rent<char>(length);
            Span<char> bufSpan = buf.Array.AsSpan();
            FillBuf(values, bufSpan);
            return new string(buf.Array, 0, length);
        }
        else
        {
            Span<char> bufSpan = stackalloc char[length];
            FillBuf(values, bufSpan);
            return bufSpan.ToString();
        }

        /////////////////////////////////////////////////////////

        static void FillBuf(List<ReadOnlyMemory<char>> values, Span<char> bufSpan)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ReadOnlySpan<char> span = values[i].Span;
                span.CopyTo(bufSpan);
                bufSpan = bufSpan.Slice(span.Length);
            }
        }
    }


#endif

}
