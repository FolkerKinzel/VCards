﻿using System.Collections;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass]
public class PropertyValueSplitterTests
{
    [DataTestMethod]
    [DataRow(null, ',', StringSplitOptions.None, 1)]
    [DataRow(null, ';', StringSplitOptions.None, 1)]
    [DataRow("", ',', StringSplitOptions.None, 1)]
    [DataRow("", ';', StringSplitOptions.None, 1)]
    [DataRow(",", ',', StringSplitOptions.None, 2)]
    [DataRow(",", ';', StringSplitOptions.None, 1)]
    [DataRow(";", ',', StringSplitOptions.None, 1)]
    [DataRow(";", ';', StringSplitOptions.None, 2)]
    [DataRow(@"\,", ',', StringSplitOptions.None, 1)]
    [DataRow(@"\,", ';', StringSplitOptions.None, 1)]
    [DataRow(@"\;", ',', StringSplitOptions.None, 1)]
    [DataRow(@"\;", ';', StringSplitOptions.None, 1)]
    public void SplitTest1(string? valueString, char splitChar, StringSplitOptions options, int expectedParts)
    {
        int count = 0;

        IEnumerable<string> valueSplitter = PropertyValueSplitter.Split(valueString.AsMemory(), splitChar, VCdVersion.V4_0);

        foreach (string s in valueSplitter)
        {
            Assert.IsNotNull(s);
            count++;
        }

        Assert.AreEqual(expectedParts, count);
    }


    [DataTestMethod]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ',', new string[] { @"Bun,go", @"Bon;go;Banga" })]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ';', new string[] { @"Bun,go,Bon;go", "Banga" })]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments",
        Justification = "Better for testing")]
    public void SplitTest2(string? valueString, char splitChar, string[] expected)
    {
        IEnumerable<string> valueSplitter = PropertyValueSplitter.Split(valueString.AsMemory(), splitChar, VCdVersion.V3_0);

        string[] arr = [.. valueSplitter];
        CollectionAssert.AreEqual(arr, expected, StringComparer.Ordinal);
    }


    [DataTestMethod]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ',', new string[] { @"Bun,go", @"Bon;go;Banga" })]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ';', new string[] { @"Bun,go,Bon;go", "Banga" })]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments",
        Justification = "Better for testing")]
    public void SplitTest3(string? valueString, char splitChar, string[] expected)
    {
        IEnumerable<string> valueSplitter = PropertyValueSplitter.Split(valueString.AsMemory(), splitChar, VCdVersion.V3_0);

        IEnumerable enumerable = valueSplitter;

        int count = 0;

        foreach (object? item in enumerable)
        {
            count++;
        }

        Assert.AreEqual(count, expected.Length);
    }

    [TestMethod]
    public void SplitTest4()
    {
        IEnumerable<string> splitter = PropertyValueSplitter.Split(",,,".AsMemory(), ',', VCdVersion.V2_1);
        Assert.AreEqual(4, splitter.Count());
    }

    [TestMethod]
    public void SplitTest5()
    {
        IEnumerable<string> splitter = PropertyValueSplitter.Split("abc".AsMemory(), ',', VCdVersion.V4_0);
        Assert.AreEqual(1, splitter.Count());
    }
}
