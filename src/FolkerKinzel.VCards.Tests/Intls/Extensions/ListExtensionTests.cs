using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FolkerKinzel.VCards.Intls.Extensions.Tests;

[TestClass]
public class ListExtensionTests
{
    [DataTestMethod]
    [DataRow("")]
    [DataRow("1")]
    [DataRow("12")]
    [DataRow("123")]
    [DataRow("1234")]  
    [DataRow("12345")]
    public void ConcatTest1(string input)
    {
        var list = new List<ReadOnlyMemory<char>>();

        for (int i = 0; i < input.Length; i++)
        {
            list.Add(input[i].ToString().AsMemory());
        }

        Assert.AreEqual(input, list.Concat().ToString());
    }

    [TestMethod]
    public void ConcatTest2()
    {
        var list = new List<ReadOnlyMemory<char>>();

        for (int i = 0; i < 5; i++)
        {
            list.Add(ReadOnlyMemory<char>.Empty);
        }

        Assert.AreEqual("", list.Concat().ToString());
    }

    [TestMethod]
    public void ConcatTest3()
    {
        var list = new List<ReadOnlyMemory<char>>();
        string s = new('a', 100);

        for (int i = 0; i < 5; i++)
        {
            list.Add(s.AsMemory());
        }

        Assert.AreEqual(string.Concat([s,s,s,s,s,]), list.Concat().ToString());
    }
}
