using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class StringArrayConverterTests
{
    [TestMethod]
    public void AsNonEmptyStringArrayTest1() => Assert.IsNotNull(StringArrayConverter.AsNonEmptyStringArray(["Hello"]));


    [TestMethod]
    public void AsNonEmptyStringArrayTest2() => Assert.IsNull(StringArrayConverter.AsNonEmptyStringArray(["    "]));

}
