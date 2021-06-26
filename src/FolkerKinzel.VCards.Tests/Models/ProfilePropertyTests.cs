using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class ProfilePropertyTests
    {
        private const string GROUP = "myGroup";

        [TestMethod()]
        public void ProfilePropertyTest1()
        {

            var prop = new ProfileProperty(GROUP);

            Assert.IsNotNull(prop);
            Assert.IsNotNull(prop.Value);
            Assert.AreEqual(GROUP, prop.Group);
            Assert.AreEqual("VCARD", prop.Value);

            string s = prop.Value;

            VCardProperty vcProp = prop;
            Assert.AreEqual(vcProp.Value, s);

            TextProperty textProp = prop;
            Assert.AreEqual(textProp.Value, s);
        }


        [TestMethod()]
        public void ProfilePropertyTest2()
        {

            var prop = new ProfileProperty(GROUP);

            var vcard = new VCard
            {
                Profile = prop
            };

            string s = vcard.ToVcfString();

            List<VCard> list = VCard.ParseVcf(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            vcard = list[0];

            Assert.IsNotNull(vcard.Profile);

            prop = vcard.Profile;

            Assert.AreEqual(GROUP, prop!.Group);
            Assert.IsFalse(prop.IsEmpty);
        }

    }
}