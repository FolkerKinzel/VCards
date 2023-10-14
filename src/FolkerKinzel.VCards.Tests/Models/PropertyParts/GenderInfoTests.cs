using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts.Tests;

[TestClass]
public class GenderInfoTests
{
    [DataTestMethod]
    [DataRow(null, null, true)]
    [DataRow(null, "Other", false)]
    [DataRow(Gender.Unknown, null, false)]
    public void IsEmptyTest1(Gender? gender, string? identity, bool expected)
    {
        var info = new GenderInfo(gender, identity);
        Assert.AreEqual(expected, info.IsEmpty);
    }
}
