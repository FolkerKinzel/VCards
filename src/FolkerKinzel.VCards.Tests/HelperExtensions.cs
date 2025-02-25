﻿using System.Collections;
using FolkerKinzel.Helpers.Polyfills;

namespace FolkerKinzel.VCards.Tests;

public static class HelperExtensions
{
    public static IEnumerable<TSource> AsWeakEnumerable<TSource>(this IEnumerable<TSource> source) => source.EnumerateWeak().Cast<TSource>();

    private static IEnumerable EnumerateWeak(this IEnumerable source)
    {
        _ArgumentNullException.ThrowIfNull(source, nameof(source));

        foreach (object? o in source)
        {
            yield return o;
        }
    }

    public static int GetLinesCount(this string? source)
    {
        int count = 0;
        if (source is null) { return count; }
        if (source.Length == 0) { return 1; }

        using var reader = new StringReader(source);
        while (reader.ReadLine() is not null) { count++; }
        return count;
    }

    public static bool HasEmptyLine(this string? source)
    {
        if (source is null) { return false; }
        if (source.Length == 0) { return true; }

        using var reader = new StringReader(source);

        string? s;
        while ((s = reader.ReadLine()) is not null)
        {
            if (s.Length == 0)
            {
                return true;
            }
        }
        return false;
    }
}
