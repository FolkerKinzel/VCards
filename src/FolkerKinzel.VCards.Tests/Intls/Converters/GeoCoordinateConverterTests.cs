using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;
using System.Globalization;
using System.Text;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class GeoCoordinateConverterTests
    {
        //[TestMethod()]
        //public void DoubleParseTest()
        //{
        //    string s = "    7.5   ";

        //    NumberStyles numStyle = NumberStyles.AllowDecimalPoint
        //                              | NumberStyles.AllowLeadingSign
        //                              | NumberStyles.AllowLeadingWhite
        //                              | NumberStyles.AllowTrailingWhite;

        //    CultureInfo culture = CultureInfo.InvariantCulture;

        //    double d = double.Parse(s, numStyle, culture);

        //    Assert.IsInstanceOfType(d, typeof(double));

        //}




        [DataTestMethod()]
        [DataRow(VCdVersion.V4_0, "geo:0.800000,0.700000")]
        [DataRow(VCdVersion.V3_0, "0.800000;0.700000")]
        [DataRow(VCdVersion.V2_1, "0.800000;0.700000")]
        public void AppendToTest1(VCdVersion version, string expected)
        {
            var sb = new StringBuilder();

            GeoCoordinateConverter.AppendTo(sb, new GeoCoordinate(0.8, 0.7), version);

            Assert.AreEqual(expected, sb.ToString());
        }

        [DataTestMethod()]
        [DataRow(VCdVersion.V4_0)]
        [DataRow(VCdVersion.V3_0)]
        [DataRow(VCdVersion.V2_1)]
        public void AppendToTest2(VCdVersion version)
        {
            var sb = new StringBuilder();

            GeoCoordinateConverter.AppendTo(sb, null, version);

            Assert.AreEqual(0, sb.Length);
        }


        [TestMethod()]
        public void AppendToTest3()
        {
            var sb = new StringBuilder();

            GeoCoordinateConverter.AppendTo(sb, new GeoCoordinate(0.8, 0.7), VCdVersion.V3_0);

            Assert.AreEqual("0.800000;0.700000", sb.ToString());
        }
    }
}