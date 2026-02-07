using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class Net472IssueTests
{
    [TestMethod]
    public void IssueTest1()
    {
        var nb = NameBuilder.Create();
        nb.AddGiven("Susi")
          .AddSurname("Sonntag");

        var builder = VCardBuilder.Create(true, true);
            
        builder.NameViews.Add(nb.Build());
    }

}
