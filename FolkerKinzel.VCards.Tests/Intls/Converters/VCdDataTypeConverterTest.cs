using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class VCdDataTypeConverterTest
    {
        [TestMethod()]
        public void Roundtrip()
        {
            foreach (var kind in (VCdDataType[])Enum.GetValues(typeof(VCdDataType)))
            {
                string s = GetValueString(kind);

                var kind2 = VCdDataTypeConverter.Parse(s);

                Assert.AreEqual(kind, kind2);

                //var kind3 = Enum.Parse(typeof(VCdDataType), ((VCdDataType?)kind).ToVCardString(), true);

                //Assert.AreEqual(kind, kind3);

                // Test auf null
                Assert.AreEqual(null, VCdDataTypeConverter.Parse(null));

                // Test auf nicht definiert
                Assert.AreEqual(null, ((VCdDataType?)4711).ToVCardString());
            }

            static string GetValueString(VCdDataType kind)
            {
                return kind switch
                {
                    VCdDataType.PhoneNumber => "PHONE-NUMBER",
                    VCdDataType.UtcOffset => "UTC-OFFSET",
                    VCdDataType.LanguageTag => "LANGUAGE-TAG",
                    VCdDataType.DateTime => "DATE-TIME",
                    VCdDataType.DateAndOrTime => "DATE-AND-OR-TIME",
                    _ => kind.ToString().ToUpperInvariant()
                };
            }
        }
    }
}