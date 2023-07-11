using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Tests;

public static class HelperExtensions
{
    public static IEnumerable<TSource> AsWeakEnumerable<TSource>(this IEnumerable<TSource> source) => source.EnumerateWeak().Cast<TSource>();

    private static IEnumerable EnumerateWeak(this IEnumerable source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        foreach (object? o in source)
        {
            yield return o;
        }
    }


    public static int GetLinesCount(this string? source) 
    {
        int count = 0;
        if (source == null) { return count; }

        using var reader = new StringReader(source);
        while(reader.ReadLine() != null) { count++; }
        return count;
    }

    public static bool HasEmptyLine(this string? source)
    {
        if (string.IsNullOrEmpty(source)) { return false; }

        using var reader = new StringReader(source);

        string? s;
        while ((s = reader.ReadLine()) != null)
        { 
            if(s.Length == 0)
            {
                return true;
            }
        }
        return false;
    }
}
