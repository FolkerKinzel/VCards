using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass]
public class ParameterValueSplitterTests
{
    [TestMethod]
    public void SplitTest1() 
        => Assert.AreEqual(0, ParameterValueSplitter.Split(ReadOnlyMemory<char>.Empty).Count());

    [TestMethod]
    public void SplitTest2()
    {
        string[] result = ParameterValueSplitter.Split("\"a,b,c\",def,,\"\",\"g,h,i\"".AsMemory()).ToArray();
        Assert.AreEqual(5, result.Length);
    }

    [TestMethod]
    public void SplitIntoMemoriesTest1()
        => Assert.AreEqual(0, ParameterValueSplitter.SplitIntoMemories(ReadOnlyMemory<char>.Empty).Count());

    [TestMethod]
    public void SplitIntoMemoriesTest2()
    {
        string[] result = ParameterValueSplitter.SplitIntoMemories("\"a,b,c\",def,,\"\",\"g,h,i\"".AsMemory()).Select(x => x.ToString()).ToArray();
        Assert.AreEqual(5, result.Length);
    }
}
