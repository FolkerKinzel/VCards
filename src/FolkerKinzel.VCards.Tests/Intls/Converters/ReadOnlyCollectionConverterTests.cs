using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class ReadOnlyCollectionConverterTests
{
    [TestMethod]
    public void ToReadOnlyCollectionTest1() => Assert.IsNotNull(StringArrayConverter.AsNonEmptyStringArray(["Hello"]));


    [TestMethod]
    public void ToReadOnlyCollectionTest2() => Assert.IsNull(StringArrayConverter.AsNonEmptyStringArray(["    "]));

}
