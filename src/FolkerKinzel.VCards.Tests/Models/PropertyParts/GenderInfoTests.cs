using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class GenderInfoTests
{
    [DataTestMethod]
    [DataRow(null, null, true)]
    [DataRow(null, "Other", false)]
    [DataRow(Sex.Unknown, null, false)]
    public void IsEmptyTest1(Sex? gender, string? identity, bool expected)
    {
        var info = new Gender(gender, identity);
        Assert.AreEqual(expected, info.IsEmpty);
    }
}
