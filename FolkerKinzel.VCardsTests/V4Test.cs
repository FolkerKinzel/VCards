using System;
using System.IO;
using System.Linq;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FolkerKinzel.VCards_Tests
{
    [TestClass]
    public class V4Test
    {
        

        [TestMethod]
        public void Parse()
        {
            var vcard = VCard.Load(VcfPaths.Vcard_4_0_Path);
          
            Assert.IsNotNull(vcard);
            Assert.AreNotEqual(0, vcard.Count);
        }

        [TestMethod]
        public void WriteEmptyVCard()
        {
            var vcard = new VCard();

            string s = vcard.ToVcfString(VCdVersion.V4_0);

            var cards = VCard.Parse(s);

            Assert.AreEqual(cards.Count, 1);

            vcard = cards[0];

            Assert.AreEqual(vcard.Version, VCdVersion.V4_0);

            Assert.IsNotNull(vcard.DisplayNames);
            Assert.AreEqual(vcard.DisplayNames.Count(), 1);
            Assert.IsNotNull(vcard.DisplayNames.First());
        }


        [TestMethod]
        public void TestLineWrapping()
        {
            var vcard = new VCard();

            string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
                "damit das Line-Wrappping getestet werden kann. " + Environment.NewLine +
                "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

            const string ASCIITEXT = "This is really a very very long ASCII-Text. This is needed to test the " +
                "LineWrapping. That's why I have to write so much even though I have nothing to say." +
                "For a better test, I write the same again: " +
                "This is really a very very long ASCII-Text. This is needed to test the " +
                "LineWrapping. That's why I have to write so much even though I have nothing to say.";

            byte[] bytes = CreateBytes();

            vcard.Notes = new TextProperty[]
            {
                new TextProperty(UNITEXT)
            };

            vcard.Keys = new DataProperty[] { new DataProperty(DataUrl.FromText(ASCIITEXT)) };

            vcard.Photos = new DataProperty[] { new DataProperty(DataUrl.FromBytes(bytes, "image/jpeg")) };

            string s = vcard.ToVcfString(VCdVersion.V4_0);


            Assert.IsNotNull(s);

            Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
                .All(x => x != null && x.Length <= VCard.MAX_BYTES_PER_LINE));

            VCard.Parse(s);

            Assert.AreEqual(((DataUrl?)vcard.Keys?.First()?.Value)?.GetEmbeddedText(), ASCIITEXT);
            Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, null);
            Assert.IsTrue(((DataUrl?)vcard.Photos?.First()?.Value)?.GetEmbeddedBytes().SequenceEqual(bytes) ?? false);


            static byte[] CreateBytes()
            {
                const int DATA_LENGTH = 200;

                var arr = new byte[DATA_LENGTH];

                for (byte i = 0; i < arr.Length; i++)
                {
                    arr[i] = i;
                }

                return arr;
            }

        }

    }
}
