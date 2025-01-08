using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class StringArrayConverterTests
{
    [TestMethod]
    public void ToStringArrayTest1() => Assert.IsNotNull(StringArrayConverter.ToStringArray(["Hello"]));


    [TestMethod]
    public void ToStringArrayTest2() => Assert.IsNotNull(StringArrayConverter.ToStringArray(["    "]));

}
