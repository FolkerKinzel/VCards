namespace FolkerKinzel.VCards.Tests;

[TestClass]
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
    [DataRow(double.NaN, 15, null)]
    [DataRow(15, double.NegativeInfinity, null)]
    [DataRow(double.PositiveInfinity, 15, null)]
    [DataRow(-101, 15, null)]
    [DataRow(101, 15, null)]
    [DataRow(15, 201, null)]
    [DataRow(15, -201, null)]
    [DataRow(52, 16, float.NaN)]
    [DataRow(52, 16, float.NegativeInfinity)]
    [DataRow(52, 16, -42.0F)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GeoCoordinateTest2(double latitude, double longitude, float? uncertainty)
        => _ = new GeoCoordinate(latitude, longitude, uncertainty);


    [TestMethod]
    public void GeoCoordinateTest3()
    {
        var geo = new GeoCoordinate(52, -181);
        Assert.AreEqual(179, geo.Longitude, 0.1);
    }

    [TestMethod]
    public void GeoCoordinateTest4()
    {
        var geo = new GeoCoordinate(52.123, -179.9999999);
        Assert.AreEqual(180, geo.Longitude, 0.1);
    }

    [DataTestMethod()]
    [DataRow(50, 50, 0.0F, 50, 50, null, false)]
    [DataRow(90, 50, null, 90, 0, null, true)]
    [DataRow(-90, -50, null, -90, 0, null, true)]
    [DataRow(91, 45, null, 89, -135, null, true)]
    [DataRow(91, -45, null, 89, 135, null, true)]
    [DataRow(-91, 45, null, -89, -135, null, true)]
    [DataRow(-91, -45, null, -89, 135, null, true)]
    [DataRow(52, 181, null, 52, -179, null, true)]
    [DataRow(5.123456, 0, null, 5.1234561, 0, null, true)]
    [DataRow(0, 5.1234568, null, 0, 5.1234561, null, false)]
    [DataRow(0, 5.1234563, null, 0, 5.1234561, null, true)]
    [DataRow(0, 0, null, 0, 0, null, true)]
    [DataRow(5.123456, 17, null, 5.123457, 17, null, false)]
    [DataRow(52, -180, null, 52, 180, null, true)]
    public void EqualsTest4(double latitude1, double longitude1, float? uncertainty1, double latitude2, double longitude2, float? uncertainty2, bool expected)
    {
        var geo1 = new GeoCoordinate(latitude1, longitude1, uncertainty1);
        var geo2 = new GeoCoordinate(latitude2, longitude2, uncertainty2);

        Assert.AreEqual(expected, geo1.Equals(geo2));
        Assert.AreEqual(expected, geo1 == geo2);
        Assert.AreNotEqual(expected, geo1 != geo2);

        if (expected)
        {
            Assert.AreEqual(geo1.GetHashCode(), geo2.GetHashCode());
        }
    }

    [DataTestMethod()]
    [DataRow(51.05022555003223, 12.130624133575036, null, 51.04930951936781, 12.128100583930106, 1000.0F, true)]
    [DataRow(51.05022555003223, 12.130624133575036, null, 51.04930951936781, 12.128100583930106, 100.0F, false)]
    [DataRow(89.99, 180.0, null, 89.99, -179.9999, 10000.0F, true)]
    [DataRow(89.99999, -17.5, null, 89.99999, -17.6F, null, true)]
    public void AreSamePositionTest1(double latitude1,
                                     double longitude1,
                                     float? uncertainty1,
                                     double latitude2,
                                     double longitude2,
                                     float? uncertainty2,
                                     bool expected)
    {
        var geo1 = new GeoCoordinate(latitude1, longitude1, uncertainty1);
        var geo2 = new GeoCoordinate(latitude2, longitude2, uncertainty2);

        Assert.AreEqual(expected, GeoCoordinate.AreSamePosition(geo1, geo2));
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AreSamePositionTest2()
        => _ = GeoCoordinate.AreSamePosition(null!, new GeoCoordinate(45, 45));

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AreSamePositionTest3()
        => _ = GeoCoordinate.AreSamePosition(new GeoCoordinate(45, 45), null!);

    [TestMethod()]
    public void EqualsTest1()
    {
        var geo = new GeoCoordinate(1, 1);
        var o = new object();

        Assert.IsFalse(geo.Equals(null));
        Assert.IsFalse(geo!.Equals(o));
    }

    [TestMethod()]
    public void EqualsTest2() => Assert.IsTrue(null == (GeoCoordinate?)null);

    [TestMethod()]
    public void EqualsTest3() => Assert.IsFalse(null == new GeoCoordinate(1, 1));

    //[TestMethod()]
    //public void GetHashCodeTest()
    //{
    //    Assert.Fail();
    //}


    [DataTestMethod()]
    //[DataRow(double.NaN, 15)]
    [DataRow(5.1234561, 0, null)]
    [DataRow(5.1234568, 5.1234561, null)]
    [DataRow(0, 0, null)]
    [DataRow(5.123456, -170.123457, null)]
    [DataRow(0, 0, 0.0F)]
    [DataRow(0, 0, 42.0F)]
    [DataRow(0, 0, 42.5F)]
    public void ToStringTest1(double latitude, double longitude, float? uncertainty)
    {
        var geo = new GeoCoordinate(latitude, longitude, uncertainty);

        string s = geo.ToString();

        Assert.IsNotNull(s);

        Console.WriteLine(s);
    }


    [DataTestMethod()]
    //[DataRow("0.8,0.7")]

    [DataRow("geo:0.8,0.7")]
    [DataRow("0.8;0.7")]
    //[DataRow(".8,0.7")]
    //[DataRow("geo:.8,0.7")]
    //[DataRow(".8,.7")]
    //[DataRow("geo:.8,.7")]
    //[DataRow("0.8,.7")]
    //[DataRow("geo:0.8,.7")]
    [DataRow(".8;0.7")]
    [DataRow(".8;.7")]
    [DataRow("0.8;.7")]
    //[DataRow("  0.8  ,  0.7  ")]
    [DataRow("  0.8  ; 0.7  ")]
    //[DataRow("  .8  ,  0.7  ")]
    //[DataRow("  .8  ,  .7  ")]
    //[DataRow("  0.8  ,  .7  ")]
    [DataRow("  .8  ;  0.7  ")]
    [DataRow("  .8  ;  .7  ")]
    [DataRow("  0.8  ;  .7  ")]
    public void TryParseTest1(string? s)
    {
        Assert.IsTrue(GeoCoordinate.TryParse(s.AsSpan(), out GeoCoordinate? geo));

        Assert.IsNotNull(geo);
        Assert.AreEqual(new GeoCoordinate(0.8, 0.7), geo);
    }

    [DataTestMethod()]
    [DataRow(null)]
    [DataRow(",0.7")]
    [DataRow("geo:,0.7")]
    [DataRow("geo:0.7,bla")]
    [DataRow("0.8;")]
    [DataRow(".8 0.7")]
    [DataRow("")]
    [DataRow("geo:")]
    [DataRow("0.8 0.7")]
    [DataRow("  ,  0.7  ")]
    [DataRow("  0.8 ;  ")]
    [DataRow("  ;0.8  ")]
    [DataRow("  .8 0.7  ")]
    [DataRow("   ")]
    [DataRow("  0.8  0.7  ")]
    [DataRow("geo:420,250;u=-42;other=val")]
    [DataRow("270;45")]
    public void TryParseTest2(string? s)
    {
        Assert.IsFalse(GeoCoordinate.TryParse(s.AsSpan(), out GeoCoordinate? geo));
        Assert.IsNull(geo);
    }

    [TestMethod()]
    public void TryParseTest3()
    {
        Assert.IsTrue(GeoCoordinate.TryParse("geo:0.8,0.7;u=42".AsSpan(), out GeoCoordinate? geo));

        Assert.IsNotNull(geo);
        Assert.AreEqual(new GeoCoordinate(0.8, 0.7, 42), geo);
    }

    [TestMethod]
    public void TryParseTest4()
    {
        Assert.IsTrue(GeoCoordinate.TryParse("geo:-0,-180".AsSpan(), out GeoCoordinate? geo));
        Assert.IsNotNull(geo);
        Assert.AreEqual(new GeoCoordinate(0, 180), geo);

    }

}
