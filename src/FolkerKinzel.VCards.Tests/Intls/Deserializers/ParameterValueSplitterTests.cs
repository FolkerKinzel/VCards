namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass]
public class ParameterValueSplitterTests
{
    [TestMethod]
    public void SplitTest1()
        => Assert.IsEmpty(ParameterValueSplitter.Split(ReadOnlyMemory<char>.Empty));

    [TestMethod]
    public void SplitTest2()
    {
        string[] result = ParameterValueSplitter.Split("\"a,b,c\",def,,\"\",\"g,h,i\"".AsMemory()).ToArray();
        Assert.HasCount(5, result);
    }

    [TestMethod]
    public void SplitIntoMemoriesTest1()
        => Assert.IsEmpty(ParameterValueSplitter.SplitIntoMemories(ReadOnlyMemory<char>.Empty));

    [TestMethod]
    public void SplitIntoMemoriesTest2()
    {
        string[] result = ParameterValueSplitter.SplitIntoMemories("\"a,b,c\",def,,\"\",\"g,h,i\"".AsMemory()).Select(x => x.ToString()).ToArray();
        Assert.HasCount(5, result);
    }
}
