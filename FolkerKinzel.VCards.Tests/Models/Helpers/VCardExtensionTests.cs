using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using FolkerKinzel.VCards.Models.Enums;
using System.Linq;

namespace FolkerKinzel.VCards.Models.Helpers.Tests
{
    [TestClass()]
    public class VCardExtensionTests
    {
        private static List<VCard?> GenerateVCardList()
        {
            var agent = new VCard()
            {
                DisplayNames = new TextProperty?[]
                {
                    null,
                    new TextProperty("The Agent", "myGroup")
                }
            };


            return new List<VCard?>
            {
                null,
                new VCard()
                {
                    Relations = new RelationProperty?[]
                    {
                        null,
                        new RelationVCardProperty(agent, RelationTypes.Agent | RelationTypes.CoWorker, "otherGroup" )
                    }
                }
            };
        }


        [TestMethod()]
        public void SetVCardReferencesTest()
        {
            List<VCard?>? list = GenerateVCardList();

            list.ReferenceVCards();

            Assert.AreEqual(3, list.Count);

            VCard? vc1 = list[1];

            Assert.IsInstanceOfType(vc1, typeof(VCard));
            Assert.IsNotNull(vc1?.Relations);

            object? o1 = vc1?.Relations?.FirstOrDefault(x => x is RelationUuidProperty)?.Value;

            Assert.IsTrue(o1 is Guid);

            VCard? vc2 = list[2];

            Assert.IsInstanceOfType(vc2, typeof(VCard));
            Assert.IsNotNull(vc2?.UniqueIdentifier);

            Guid? o2 = vc2?.UniqueIdentifier?.Value;

            Assert.IsTrue(o2.HasValue);
            Assert.AreEqual((Guid)o1!, o2!.Value);
        }

        [TestMethod()]
        public void DereferenceVCardsTest()
        {
            List<VCard?>? list = GenerateVCardList();

            list.ReferenceVCards();

            Assert.AreEqual(3, list.Count);
            Assert.IsNull(list[1]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));

            list.DereferenceVCards();

            Assert.AreEqual(3, list.Count);
            Assert.IsNotNull(list[1]?.Relations?.FirstOrDefault(x => x is RelationVCardProperty));
        }

        [TestMethod()]
        public void SaveVCardsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SerializeVCardsTest() => new List<VCard?>().SerializeVCards(null!, VCdVersion.V3_0);
    }
}