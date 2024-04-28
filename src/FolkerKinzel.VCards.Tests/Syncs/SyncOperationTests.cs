using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Syncs.Tests;

[TestClass()]
public class SyncOperationTests
{
    [TestMethod]
    public void SetPropertyIDsTest1()
    {
        VCard.SyncTestReset();

        var vc = new VCard();
        var arr = new TextProperty[] { new("Donald"), new("Duck") };
        vc.DisplayNames = arr;

        vc.Sync.SetPropertyIDs();

        Assert.AreEqual(1, arr[0].Parameters.PropertyIDs!.First()!.ID);
        Assert.AreEqual(2, arr[1].Parameters.PropertyIDs!.First()!.ID);

        vc.Sync.SetPropertyIDs();

        Assert.AreEqual(1, arr[1].Parameters.PropertyIDs!.Count());
    }

    [TestMethod]
    public void SetPropertyIDsTest2()
    {
        VCard.SyncTestReset();
        VCard.RegisterApp(null);

        var vc = new VCard();
        var tProp = new TextProperty("Donald");
        vc.DisplayNames = tProp.Append(null);

        Assert.IsNull(tProp.Parameters.PropertyIDs);
        Assert.IsNull(vc.AppIDs);

        vc.Sync.SetPropertyIDs();
        Assert.IsNotNull(tProp.Parameters.PropertyIDs);
        Assert.AreEqual(1, tProp.Parameters.PropertyIDs.Count());

        //const string folker = "http://folker.de/id";
        ////const string contoso = "http://contoso.com/id";

        //var uri1 = new Uri(folker, UriKind.Absolute);
        ////var uri2 = new Uri(contoso, UriKind.Absolute);

        //vc.RegisterAppInInstance(uri1);
        //Assert.IsNotNull(vc.AppIDs);

        //vc.SetPropertyIDs();
        //Assert.AreEqual(2, tProp.Parameters.PropertyIDs.Count());
        //Assert.IsTrue(vc.AppIDs.All(x => x?.Parameters.PropertyIDs is null));

        //vc.SetPropertyIDs();
        //Assert.AreEqual(2, tProp.Parameters.PropertyIDs.Count());

        //vc.AppIDs = null;
        //Assert.IsNull(vc.Sync.CurrentAppID);
    }

    [TestMethod]
    public void SetPropertyIDsTest3()
    {
        var uri = new Uri("http://folker.de/");
        VCard.SyncTestReset();
        VCard.RegisterApp(uri);
        var vc = new VCard();

        vc.Sync.SetPropertyIDs();

        Assert.IsNull(vc.AppIDs);
    }

    [TestMethod()]
    public void ResetTest1()
    {
        var uri = new Uri("http://folker.de/");
        VCard.SyncTestReset();
        VCard.RegisterApp(uri);
        var vc = new VCard();
        var tProp = new TextProperty("Donald");
        vc.DisplayNames = tProp.Append(null);

        vc.Sync.SetPropertyIDs();
        Assert.AreEqual(uri.AbsoluteUri, vc.Sync.CurrentAppID?.GlobalID);
        Assert.IsNotNull(tProp.Parameters.PropertyIDs);
        Assert.AreEqual(1, tProp.Parameters.PropertyIDs.Count());
        Assert.AreEqual(1, vc.AppIDs?.Count());

        vc.Sync.Reset();
        Assert.IsNull(tProp.Parameters.PropertyIDs);
        Assert.IsNull(vc.AppIDs);
    }

    [TestMethod]
    public void EqualsTest1()
    {
        var vc = new VCard();
        SyncOperation sync = vc.Sync;
        Assert.IsFalse(sync.Equals((SyncOperation?)null));

        Assert.AreEqual(sync.GetHashCode(), ((object)sync).GetHashCode());
    }

    [TestMethod]
    public void ToStringTest1() => Assert.IsNotNull(new VCard().Sync.ToString());
}