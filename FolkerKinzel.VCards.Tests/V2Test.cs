using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;
using System.Linq;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Helpers;
using System.Collections.Generic;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass]
    public class V2Test
    {
        [TestMethod]
        public void Parse()
        {
            List<VCard> vcard = VCard.Load(fileName: TestFiles.V2vcf);

            Assert.IsNotNull(vcard);
            Assert.AreNotEqual(0, vcard.Count);
        }

        [TestMethod]
        public void ParseOutlook()
        {
            List<VCard> vcard = VCard.Load(fileName: TestFiles.OutlookV2vcf);

            Assert.IsNotNull(vcard);
            Assert.IsNotNull(vcard.FirstOrDefault());

            //string s = vcard[0].ToString();

            DataProperty? photo = vcard[0].Photos?.FirstOrDefault();
            Assert.IsNotNull(photo);

            if(photo?.Value is DataUrl dataUrl)
            {
                Assert.AreEqual(dataUrl.MimeType.MediaType, "image/jpeg");
                //System.IO.File.WriteAllBytes(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"Testbild{dataUrl.GetFileExtension()}"), dataUrl.GetEmbeddedBytes());
            }
            else
            {
                Assert.Fail();
            }

            VCard.Save(System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), $"TestV2.1.vcf"),
                vcard!, VCdVersion.V2_1, VcfOptions.Default.Set(VcfOptions.WriteNonStandardProperties));
        }


        [TestMethod]
        public void WriteEmptyVCard()
        {
            var vcard = new VCard();

            string s = vcard.ToVcfString(VCdVersion.V2_1);

            List<VCard> cards = VCard.Parse(s);

            Assert.AreEqual(cards.Count, 1);

            vcard = cards[0];

            Assert.AreEqual(vcard.Version, VCdVersion.V2_1);

            Assert.IsNotNull(vcard.NameViews);
            Assert.AreEqual(vcard.NameViews.Count(), 1);
            Assert.IsNotNull(vcard.NameViews.First());
        }

        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
        public void TestLineWrapping()
        {
            var vcard = new VCard();

            const string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
                "damit das Line-Wrappping mit Quoted-Printable-Encoding getestet werden kann. " +
                "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

            const string ASCIITEXT = "This is really a very very long ASCII-Text. This is needed to test the " +
                "vCard 2.1 - LineWrapping. That's why I have to write so much even though I have nothing to say.";

            byte[] bytes = CreateBytes();

            vcard.Notes = new TextProperty[]
            {
                new TextProperty(UNITEXT)
            };

            vcard.Keys = new DataProperty[] { new DataProperty(DataUrl.FromText(ASCIITEXT)) };

            vcard.Photos = new DataProperty[] { new DataProperty(DataUrl.FromBytes(bytes, "image/jpeg")) };

            string s = vcard.ToVcfString(VCdVersion.V2_1);

           
            Assert.IsNotNull(s);

            Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
                .All(x => x != null && x.Length <= VCard.MAX_BYTES_PER_LINE));

            VCard.Parse(s);

            Assert.AreEqual(((DataUrl?)vcard.Keys?.First()?.Value)?.GetEmbeddedText(), ASCIITEXT);
            Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, "image/jpeg");
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
