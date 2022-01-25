using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class ReadOnlyCollectionConverterTests
{
    [TestMethod]
    public void ToReadOnlyCollectionTest1() => Assert.IsNotNull(ReadOnlyCollectionConverter.ToReadOnlyCollection(new string[] { "Hello" }));

    [TestMethod]
    public void ToReadOnlyCollectionTest3() => Assert.IsNotNull(ReadOnlyCollectionConverter.ToReadOnlyCollection(new ReadOnlyCollection<string>(new string[] { "Hello" })));

    [TestMethod]
    public void ToReadOnlyCollectionTest2() => Assert.IsNotNull(ReadOnlyCollectionConverter.ToReadOnlyCollection(Yielder()));


    private static IEnumerable<string> Yielder()
    {
        yield return "Hello";
    }

}
