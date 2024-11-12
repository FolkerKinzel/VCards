namespace FolkerKinzel.VCards.Intls.Formatters.Tests;

[TestClass]
public class DefaultNameFormatterTests
{
    [TestMethod]
    public void DefaultNameFormatterTest1()
    {
        VCard vc = VCardBuilder.Create().NameViews.Add((Name?)null).VCard;
        Assert.IsNull(NameFormatter.Default.ToDisplayName(vc.NameViews!.First()!, vc));
    }

    [DataTestMethod]
    [DataRow("de", "1 A 2 B 3 C 4 D 5 E 6 F 7 G")]
    [DataRow("es", "1 A 2 B 3 C 5 E 4 D 6 F 7 G")]
    [DataRow("zh", "1 A 4 D 5 E 6 F 2 B 3 C 7 G")]
    [DataRow("vi", "1 A 4 D 5 E 6 F 3 C 2 B 7 G")]
    public void DefaultNameFormatterTest2(string lang, string result)
    {
        VCard vc = VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder
                .Create()
                .AddPrefix("1")
                .AddPrefix("A")
                .AddGiven("2")
                .AddGiven("B")
                .AddGiven2("3")
                .AddGiven2("C")
                .AddSurname2("4")
                .AddSurname2("D")
                .AddSurname("5")
                .AddSurname("E")
                .AddGeneration("6")
                .AddGeneration("F")
                .AddSuffix("7")
                .AddSuffix("G")
                .Build()
                )
            .Language.Set(lang)
            .VCard;

        Assert.AreEqual(result, NameFormatter.Default.ToDisplayName(vc.NameViews!.First()!, vc));
    }

    [TestMethod]
    public void DefaultNameFormatterTest3()
    {
        VCard vc = VCardBuilder
           .Create()
           .NameViews.Add(NameBuilder
               .Create()
               .AddGiven("Folker")
               .AddSurname("Kinzel")
               .Build(),
               parameters: p => p.ComponentOrder = ";4711"
               )
           .Language.Set("de-DE")
        .VCard;

        Assert.AreEqual("Folker Kinzel", NameFormatter.Default.ToDisplayName(vc.NameViews!.First()!, vc));
    }
}
