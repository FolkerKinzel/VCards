using System.Collections.Generic;
using System.IO;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class AccessPropertyTests
    {
        private const string GROUP = "MyGroup";

        [TestMethod()]
        public void AccessPropertyTest1()
        {
            var prop = new AccessProperty(Enums.VCdAccess.Confidential, GROUP);

            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreEqual(Enums.VCdAccess.Confidential, prop.Value);
            Assert.AreEqual(Enums.VCdAccess.Confidential, ((VCardProperty)prop).Value);

            Assert.IsFalse(prop.IsEmpty);
        }


        [TestMethod()]
        public void AccessPropertyTest2()
        {
            var row = VcfRow.Parse($"{GROUP}.{VCard.PropKeys.CLASS}:private", new VcfDeserializationInfo());
            Assert.IsNotNull(row);

            var prop = new AccessProperty(row!);

            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreEqual(Enums.VCdAccess.Private, prop.Value);
            Assert.AreEqual(Enums.VCdAccess.Private, ((VCardProperty)prop).Value);


            Assert.IsFalse(prop.IsEmpty);
        }

        [TestMethod()]
        public void AppendValueTest()
        {
            var vcard = new VCard
            {
                Access = new AccessProperty(Enums.VCdAccess.Private, GROUP)
            };

            string serialized = vcard.ToVcfString();

            IList<VCard> list = VCard.ParseVcf(serialized);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];
            Assert.IsNotNull(vcard.Access);
            Assert.AreEqual(GROUP, vcard.Access!.Group);
            Assert.AreEqual(Enums.VCdAccess.Private, vcard.Access.Value);
        }


    }
}