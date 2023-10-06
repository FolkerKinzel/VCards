namespace FolkerKinzel.VCards.Intls.Encodings.Tests;

[TestClass]
public class Base64HelperTests
{
    [TestMethod]
    public void GetBytesOrNullTest1()
        => Assert.IsNull(Base64Helper.GetBytesOrNull("ä"));
}
