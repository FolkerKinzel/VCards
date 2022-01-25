using System.Collections;

namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass]
public class ValueSplitterTests
{
    [DataTestMethod]
    [DataRow(null, ',', StringSplitOptions.None, 0)]
    [DataRow(null, ';', StringSplitOptions.None, 0)]
    [DataRow(null, ',', StringSplitOptions.RemoveEmptyEntries, 0)]
    [DataRow(null, ';', StringSplitOptions.RemoveEmptyEntries, 0)]
    [DataRow("", ',', StringSplitOptions.None, 1)]
    [DataRow("", ';', StringSplitOptions.None, 1)]
    [DataRow("", ',', StringSplitOptions.RemoveEmptyEntries, 0)]
    [DataRow("", ';', StringSplitOptions.RemoveEmptyEntries, 0)]
    [DataRow(",", ',', StringSplitOptions.None, 2)]
    [DataRow(",", ';', StringSplitOptions.None, 1)]
    [DataRow(",", ',', StringSplitOptions.RemoveEmptyEntries, 0)]
    [DataRow(",", ';', StringSplitOptions.RemoveEmptyEntries, 1)]
    [DataRow(";", ',', StringSplitOptions.None, 1)]
    [DataRow(";", ';', StringSplitOptions.None, 2)]
    [DataRow(";", ',', StringSplitOptions.RemoveEmptyEntries, 1)]
    [DataRow(";", ';', StringSplitOptions.RemoveEmptyEntries, 0)]
    [DataRow(@"\,", ',', StringSplitOptions.None, 1)]
    [DataRow(@"\,", ';', StringSplitOptions.None, 1)]
    [DataRow(@"\,", ',', StringSplitOptions.RemoveEmptyEntries, 1)]
    [DataRow(@"\,", ';', StringSplitOptions.RemoveEmptyEntries, 1)]
    [DataRow(@"\;", ',', StringSplitOptions.None, 1)]
    [DataRow(@"\;", ';', StringSplitOptions.None, 1)]
    [DataRow(@"\;", ',', StringSplitOptions.RemoveEmptyEntries, 1)]
    [DataRow(@"\;", ';', StringSplitOptions.RemoveEmptyEntries, 1)]
    public void GetEnumeratorTest1(string? valueString, char splitChar, StringSplitOptions options, int expectedParts)
    {
        int count = 0;

        var valueSplitter = new ValueSplitter(splitChar, options)
        {
            ValueString = valueString
        };

        foreach (var s in valueSplitter)
        {
            Assert.IsNotNull(s);
            count++;
        }

        Assert.AreEqual(expectedParts, count);
    }


    [DataTestMethod]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ',', new string[] { @"Bun\,go", @"Bon\;go;Banga" })]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ';', new string[] { @"Bun\,go,Bon\;go", "Banga" })]
    public void GetEnumeratorTest2(string? valueString, char splitChar, string[] expected)
    {
        var valueSplitter = new ValueSplitter(splitChar, StringSplitOptions.None)
        {
            ValueString = valueString
        };

        var arr = valueSplitter.ToArray();
        CollectionAssert.AreEqual(arr, expected, StringComparer.Ordinal);
    }


    [DataTestMethod]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ',', new string[] { @"Bun\,go", @"Bon\;go;Banga" })]
    [DataRow(@"Bun\,go,Bon\;go;Banga", ';', new string[] { @"Bun\,go,Bon\;go", "Banga" })]
    public void GetEnumeratorTest3(string? valueString, char splitChar, string[] expected)
    {
        var valueSplitter = new ValueSplitter(splitChar, StringSplitOptions.None)
        {
            ValueString = valueString
        };

        IEnumerable enumerable = valueSplitter;

        int count = 0;

        foreach (var item in enumerable)
        {
            count++;
        }

        Assert.AreEqual(count, expected.Length);
    }
}
