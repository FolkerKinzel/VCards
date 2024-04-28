namespace FolkerKinzel.VCards.Intls.Deserializers.Tests;

[TestClass]
public class VcfDeserializerTests
{
    [TestMethod]
    public void ResetTest1()
    {
        var info = new VcfDeserializationInfo();
        info.Builder.Append(new string('a', 20000));
        int capacity = info.Builder.Capacity;
        info.Reset();
        Assert.AreEqual(0, info.Builder.Length);
        Assert.IsTrue(info.Builder.Capacity < capacity);
    }
}
