namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class UriConverterTests
{
    [TestMethod]
    public void ToAbsoluteUriTest1()
        => Assert.IsNull(UriConverter.ToAbsoluteUri(null));

    [TestMethod]
    public void ToAbsoluteUriTest2()
        => Assert.IsNull(UriConverter.ToAbsoluteUri("https://not allowed space.com"));

    [TestMethod]
    public void ToAbsoluteUriTest3()
        => Assert.IsNotNull(UriConverter.ToAbsoluteUri("www.not allowed space.com"));

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

