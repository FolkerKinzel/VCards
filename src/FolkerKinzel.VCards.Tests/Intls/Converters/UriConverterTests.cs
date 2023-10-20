namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class UriConverterTests
{
    [TestMethod]
    public void ToAbsoluteUriTest1()
        => Assert.IsFalse(UriConverter.TryConvertToAbsoluteUri(null, out _));

    public void ToAbsoluteUriTest1b()
        => Assert.IsFalse(UriConverter.TryConvertToAbsoluteUri("", out _));

    public void ToAbsoluteUriTest1c()
       => Assert.IsFalse(UriConverter.TryConvertToAbsoluteUri("    ", out _));

    [TestMethod]
    public void ToAbsoluteUriTest2()
        => Assert.IsFalse(UriConverter.TryConvertToAbsoluteUri("https://not allowed space.com", out _));

    [TestMethod]
    public void ToAbsoluteUriTest3()
        => Assert.IsFalse(UriConverter.TryConvertToAbsoluteUri("www.not allowed space.com", out _));

    [TestMethod]
    public void ToAbsoluteUriTest4()
        => Assert.IsTrue(UriConverter.TryConvertToAbsoluteUri("  www.nospace.com   ", out _));

    [TestMethod]
    public void ToAbsoluteUriTest5()
        => Assert.IsTrue(UriConverter.TryConvertToAbsoluteUri(" http://www.nospace.com   ", out _));

    [TestMethod]
    public void GetFileTypeExtensionFromUriTest1()
        => Assert.AreEqual(".bin", UriConverter.GetFileTypeExtensionFromUri(null));

    [TestMethod]
    public void GetFileTypeExtensionFromUriTest2()
        => Assert.AreEqual(".png", UriConverter.GetFileTypeExtensionFromUri(new Uri("picture.png", UriKind.Relative)));

    [TestMethod]
    public void GetFileTypeExtensionFromUriTest3()
        => Assert.AreEqual(".bin", UriConverter.GetFileTypeExtensionFromUri(new Uri("picture/png", UriKind.Relative)));

    [TestMethod]
    public void GetFileTypeExtensionFromUriTest4()
        => Assert.AreEqual(".htm", UriConverter.GetFileTypeExtensionFromUri(
            new Uri("http://contoso.com", UriKind.Absolute)));


}

