using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class UuidConverterTest
{
    [TestMethod]
    public void IsUuidUriTest1()
    {
        string guidString = "550e8400-e29b-11d4-a716-446655440000";
        guidString = "urn:uuid:" + guidString;
        Assert.IsTrue(guidString.IsUuidUri());
    }


    [TestMethod]
    public void IsUuidUriTest2()
    {
        string guidString = "550e8400-e29b-11d4-a716-446655440000";
        Assert.IsFalse(guidString.IsUuidUri());
    }

    [TestMethod]
    public void IsUuidUriTest3()
    {
        string guidString = "550e8400-e29b-11d4-a716-446655440000";
        guidString = "https://" + guidString;
        Assert.IsFalse(guidString.IsUuidUri());
    }

    [TestMethod]
    public void IsUuidUriTest5()
    {
        string guidString = "550e8400";
        Assert.IsFalse(guidString.IsUuidUri());
    }

    [TestMethod]
    public void IsUuidUriTest5b()
    {
        string? guidString = null;
        Assert.IsFalse(guidString.IsUuidUri());
    }

    [TestMethod]
    public void IsUuidUriTest6()
    {
        string guidString = "550e8400e29b11d4a716446655440000";
        guidString = "   urn:uuid:" + guidString;
        Assert.IsTrue(guidString.IsUuidUri());
    }

    [TestMethod]
    public void IsUuidUriTest7()
    {
        string guidString = " ";
        Assert.IsFalse(guidString.IsUuidUri());
    }

    [TestMethod]
    public void IsUuidUriTest8()
    {
        string guidString = new(' ', 100);
        Assert.IsFalse(guidString.IsUuidUri());
    }

    [TestMethod]
    public void ToGuidTest1()
    {
        string guidString = "550e8400e29b11d4a716446655440000";
        Assert.IsTrue(UuidConverter.TryAsGuid(guidString.AsSpan(), out Guid guid));
        Assert.AreNotEqual(guid, Guid.Empty);
        guidString = "urn:uuid:" + guidString;
        Assert.IsTrue(UuidConverter.TryAsGuid(guidString.AsSpan(), out Guid guid2));
        Assert.AreEqual(guid, guid2);
    }

    [TestMethod]
    public void ToGuidTest6()
    {
        string guidString = "550e8400e29b11d4a716446655440000";
        Assert.IsTrue(UuidConverter.TryAsGuid(guidString.AsSpan(), out Guid guid));
        Assert.AreNotEqual(guid, Guid.Empty);
        guidString = "    urn:uuid:" + guidString;
        Assert.IsTrue(UuidConverter.TryAsGuid(guidString.AsSpan(), out Guid guid2));
        Assert.AreEqual(guid, guid2);
    }


    [TestMethod]
    public void ToGuidTest2()
    {
        string guidString = "550e8400-e29b-11d4-a716-446655440000";
        Assert.IsTrue(UuidConverter.TryAsGuid(guidString.AsSpan(), out Guid guid));
        Assert.AreNotEqual(guid, Guid.Empty);
        Assert.AreEqual(guidString, guid.ToString());
        guidString = "urn:uuid:" + guidString;
        Assert.IsTrue(UuidConverter.TryAsGuid(guidString.AsSpan(), out Guid guid2));
        Assert.AreEqual(guid, guid2);
        var sb = new StringBuilder();
        _ = sb.AppendUuid(guid2);
        Assert.AreEqual(sb.ToString(), guidString);
    }


    [TestMethod]
    public void ToGuidTest3()
    {
        Assert.IsFalse(UuidConverter.TryAsGuid("".AsSpan(), out Guid guid3));
        Assert.AreEqual(Guid.Empty, guid3);
    }

    [TestMethod]
    public void ToGuidTest4()
    {
        Assert.IsFalse(UuidConverter.TryAsGuid(string.Empty.AsSpan(), out Guid guid4));
        Assert.AreEqual(Guid.Empty, guid4);
    }


    [TestMethod]
    public void ToGuidTest5()
    {
        Assert.IsFalse(UuidConverter.TryAsGuid("               ".AsSpan(), out Guid guid4));
        Assert.AreEqual(Guid.Empty, guid4);
    }

}
