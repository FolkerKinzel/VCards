using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Linq;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class GenderPropertyTests
    {
        private const string GROUP = "myGroup";
        private const string IDENTITY = "identity";

        [DataTestMethod()]
        [DataRow(GROUP, VCdSex.Male, null)]
        [DataRow(GROUP, VCdSex.Female, null)]
        [DataRow(GROUP, VCdSex.Other, null)]
        [DataRow(GROUP, VCdSex.Unknown, null)]
        [DataRow(GROUP, VCdSex.NonOrNotApplicable, null)]
        [DataRow(GROUP, null, null)]
        [DataRow(null, null, IDENTITY)]
        [DataRow(null, VCdSex.Female, IDENTITY)]
        [DataRow(GROUP, VCdSex.Female, IDENTITY)]
        public void GenderPropertyTest1(string? expectedGroup, VCdSex? expectedSex, string? expectedGenderIdentity)
        {
            var genderProp = new GenderProperty(expectedSex, expectedGenderIdentity, expectedGroup);

            Assert.IsNotNull(genderProp.Value);
            Assert.AreEqual(expectedGroup, genderProp.Group);
            Assert.AreEqual(expectedSex, genderProp.Value.Sex);
            Assert.AreEqual(expectedGenderIdentity, genderProp.Value.GenderIdentity);
        }


        [DataTestMethod()]
        [DataRow(GROUP + ".GENDER:M", GROUP, VCdSex.Male, null)]
        [DataRow(GROUP + ".GENDER:F", GROUP, VCdSex.Female, null)]
        [DataRow(GROUP + ".GENDER:O", GROUP, VCdSex.Other, null)]
        [DataRow(GROUP + ".GENDER:U", GROUP, VCdSex.Unknown, null)]
        [DataRow(GROUP + ".GENDER:N", GROUP, VCdSex.NonOrNotApplicable, null)]
        [DataRow(GROUP + ".GENDER:", GROUP, null, null)]
        [DataRow(GROUP + ".GENDER:;", GROUP, null, null)]
        [DataRow(GROUP + ".GENDER: ; ", GROUP, null, null)]
        [DataRow("GENDER: ;" + IDENTITY, null, null, IDENTITY)]
        [DataRow("GENDER:F;" + IDENTITY, null, VCdSex.Female, IDENTITY)]
        [DataRow(GROUP + ".GENDER:F;" + IDENTITY, GROUP, VCdSex.Female, IDENTITY)]
        [DataRow(GROUP + ".GENDER:F;", GROUP, VCdSex.Female, null)]
        public void GenderPropertyTest2(string s, string? expectedGroup, VCdSex? expectedSex, string? expectedGenderIdentity)
        {
            var vcfRow = VcfRow.Parse(s, new VcfDeserializationInfo());

            Assert.IsNotNull(vcfRow);

            var genderProp = new GenderProperty(vcfRow!, Enums.VCdVersion.V4_0);

            Assert.IsNotNull(genderProp.Value);
            Assert.AreEqual(expectedGroup, genderProp.Group);
            Assert.AreEqual(expectedSex, genderProp.Value.Sex);
            Assert.AreEqual(expectedGenderIdentity, genderProp.Value.GenderIdentity);
        }


        [TestMethod]
        public void GenderPropertyTest3()
        {
            var prop = new GenderProperty(VCdSex.Female, IDENTITY, GROUP);

            var vcard = new VCard
            {
                GenderViews = prop
            };

            string s = vcard.ToVcfString(VCdVersion.V4_0);

            IList<VCard> list = VCard.ParseVcf(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.GenderViews);

            prop = vcard.GenderViews!.First() as GenderProperty;

            Assert.IsNotNull(prop);
            Assert.AreEqual(VCdSex.Female, prop!.Value.Sex);
            Assert.AreEqual(IDENTITY, prop!.Value.GenderIdentity);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);

        }
    }
}