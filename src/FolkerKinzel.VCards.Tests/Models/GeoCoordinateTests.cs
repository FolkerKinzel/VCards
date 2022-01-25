namespace FolkerKinzel.VCards.Models.Tests;

[TestClass()]
public class GeoCoordinateTests
{
    [TestMethod()]
    public void GeoCoordinateTest1()
    {
        double latitude = 17.5;
        double longitude = 4.2;

        var geo = new GeoCoordinate(latitude, longitude);

        Assert.AreEqual(latitude, geo.Latitude);
        Assert.AreEqual(longitude, geo.Longitude);
    }


    [DataTestMethod()]
    [DataRow(double.NaN, 15)]
    [DataRow(15, double.NegativeInfinity)]
    [DataRow(double.PositiveInfinity, 15)]
    [DataRow(-91, 15)]
    [DataRow(91, 15)]
    [DataRow(15, 181)]
    [DataRow(15, -181)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GeoCoordinateTest2(double latitude, double longitude) => _ = new GeoCoordinate(latitude, longitude);


    [DataTestMethod()]
    //[DataRow(double.NaN, 15, 27, double.NaN, true)]
    //[DataRow(double.NegativeInfinity, 15, 27, double.PositiveInfinity, true)]
    [DataRow(5.123456, 0, 5.1234561, 0, true)]
    [DataRow(0, 5.1234568, 0, 5.1234561, false)]
    [DataRow(0, 5.1234563, 0, 5.1234561, true)]
    [DataRow(0, 0, 0, 0, true)]
    [DataRow(5.123456, 17, 5.123457, 17, false)]
    public void EqualsTest(double latitude1, double longitude1, double latitude2, double longitude2, bool expected)
    {
        var geo1 = new GeoCoordinate(latitude1, longitude1);
        var geo2 = new GeoCoordinate(latitude2, longitude2);

        Assert.AreEqual(expected, geo1.Equals(geo2));

        if (expected)
        {
            Assert.AreEqual(geo1.GetHashCode(), geo2.GetHashCode());
        }
    }


    [TestMethod()]
    public void EqualsTest1()
    {
        var geo = new GeoCoordinate(1, 1);

        Assert.IsFalse(geo.Equals(null));
        Assert.IsFalse(geo!.Equals(new object()));
    }

    //[TestMethod()]
    //public void GetHashCodeTest()
    //{
    //    Assert.Fail();
    //}


    [DataTestMethod()]
    //[DataRow(double.NaN, 15)]
    [DataRow(5.1234561, 0)]
    [DataRow(5.1234568, 5.1234561)]
    [DataRow(0, 0)]
    [DataRow(5.123456, -170.123457)]
    public void ToStringTest(double latitude, double longitude)
    {
        var geo = new GeoCoordinate(latitude, longitude);

        string s = geo.ToString();

        Console.WriteLine(s);
    }


    [DataTestMethod()]
    [DataRow("0.8,0.7")]
    [DataRow("geo:0.8,0.7")]
    [DataRow("0.8;0.7")]
    [DataRow(".8,0.7")]
    [DataRow("geo:.8,0.7")]
    [DataRow(".8,.7")]
    [DataRow("geo:.8,.7")]
    [DataRow("0.8,.7")]
    [DataRow("geo:0.8,.7")]
    [DataRow(".8;0.7")]
    [DataRow(".8;.7")]
    [DataRow("0.8;.7")]
    [DataRow("  0.8  ,  0.7  ")]
    [DataRow("  0.8  ; 0.7  ")]
    [DataRow("  .8  ,  0.7  ")]
    [DataRow("  .8  ,  .7  ")]
    [DataRow("  0.8  ,  .7  ")]
    [DataRow("  .8  ;  0.7  ")]
    [DataRow("  .8  ;  .7  ")]
    [DataRow("  0.8  ;  .7  ")]
    public void TryParseTest1(string? s)
    {
        Assert.IsTrue(GeoCoordinate.TryParse(s, out GeoCoordinate? geo));

        Assert.IsNotNull(geo);
        Assert.AreEqual(new GeoCoordinate(0.8, 0.7), geo);
    }

    [DataTestMethod()]
    [DataRow(",0.7")]
    [DataRow("geo:,0.7")]
    [DataRow("0.8;")]
    [DataRow(".8 0.7")]
    [DataRow("")]
    [DataRow("geo:")]
    [DataRow("0.8 0.7")]
    [DataRow("  ,  0.7  ")]
    [DataRow("  0.8 ;  ")]
    [DataRow("  .8 0.7  ")]
    [DataRow("   ")]
    [DataRow("  0.8  0.7  ")]
    public void TryParseTest2(string? s)
    {
        Assert.IsFalse(GeoCoordinate.TryParse(s, out GeoCoordinate? geo));
        Assert.IsNull(geo);
    }

}
