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
    public class KindPropertyTests
    {
        private const string GROUP = "myGroup";


        [TestMethod()]
        public void KindPropertyTest1()
        {
            const Enums.VCdKind kind = Enums.VCdKind.Application;

            var prop = new KindProperty(kind, GROUP);

            Assert.AreEqual(kind, prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }


        [TestMethod()]
        public void KindPropertyTest2()
        {
            const Enums.VCdKind kind = Enums.VCdKind.Application;

            var prop = new KindProperty(kind, GROUP);

            var vcard = new VCard
            {
                Kind = prop
            };

            string s = vcard.ToVcfString(Enums.VCdVersion.V4_0);

            IList<VCard> list = VCard.ParseVcf(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.Kind);

            prop = vcard.Kind;

            Assert.AreEqual(kind, prop!.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.IsFalse(prop.IsEmpty);
        }
    }
}