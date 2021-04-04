using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class RelationTextPropertyTests
    {
        private const string GROUP = "myGroup";

        [TestMethod()]
        public void RelationTextPropertyTest1()
        {
            const Enums.RelationTypes relation = Enums.RelationTypes.Acquaintance;
            var text = "Bruno Hübchen";

            var prop = new RelationTextProperty(text, relation, GROUP);

            Assert.AreEqual(text, prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);

            Assert.AreEqual(relation, prop.Parameters.RelationType);
            Assert.AreEqual(Enums.VCdDataType.Text, prop.Parameters.DataType);
        }


        [TestMethod()]
        public void RelationTextPropertyTest2()
        {
            const Enums.RelationTypes relation = Enums.RelationTypes.Acquaintance;
            var text = "Bruno Hübchen";

            var prop = new RelationTextProperty(text, relation, GROUP);

            var vcard = new VCard
            {
                Relations = prop
            };

            string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);

            List<VCard> list = VCard.Parse(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.Relations);

            prop = vcard.Relations!.First() as RelationTextProperty;

            Assert.IsNotNull(prop);
            Assert.AreEqual(text, prop!.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);

            Assert.AreEqual(relation, prop.Parameters.RelationType);
            Assert.AreEqual(Enums.VCdDataType.Text, prop.Parameters.DataType);
        }


        [TestMethod()]
        public void RelationTextPropertyTest3()
        {
            const Enums.RelationTypes relation = Enums.RelationTypes.Agent;
            var text = "Bruno Hübchen";

            var prop = new RelationTextProperty(text, relation, GROUP);

            var vcard = new VCard
            {
                Relations = prop
            };

            string s = vcard.ToVcfString(Enums.VCdVersion.V2_1);

            List<VCard> list = VCard.Parse(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.Relations);

            prop = vcard.Relations!.First() as RelationTextProperty;

            Assert.IsNotNull(prop);
            Assert.AreEqual(text, prop!.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);

            Assert.AreEqual(relation, prop.Parameters.RelationType);
            Assert.AreEqual(Enums.VCdDataType.Text, prop.Parameters.DataType);
        }

    }
}