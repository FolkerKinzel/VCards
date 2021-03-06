﻿using System;
using System.Collections.Generic;
using System.Linq;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Tests
{
    [TestClass]
    public class V4Tests
    {
        [TestMethod]
        public void Parse()
        {
            List<VCard>? vcard = VCard.Load(TestFiles.V4vcf);

            Assert.IsNotNull(vcard);
            Assert.AreNotEqual(0, vcard.Count);

        }

        [TestMethod]
        public void WriteEmptyVCard()
        {
            var vcard = new VCard();

            string s = vcard.ToVcfString(VCdVersion.V4_0);

            List<VCard>? cards = VCard.Parse(s);

            Assert.AreEqual(cards.Count, 1);

            vcard = cards[0];

            Assert.AreEqual(VCdVersion.V4_0, vcard.Version);

            Assert.IsNotNull(vcard.DisplayNames);
            Assert.AreEqual(vcard.DisplayNames!.Count(), 1);
            Assert.IsNotNull(vcard.DisplayNames!.First());
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

            _ = VCard.Parse(s);

            Assert.AreEqual(((DataUrl?)vcard.Keys?.First()?.Value)?.GetEmbeddedText(), ASCIITEXT);
            Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, null);
            Assert.IsTrue(((DataUrl?)vcard.Photos?.First()?.Value)?.GetEmbeddedBytes()?.SequenceEqual(bytes) ?? false);


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


        [TestMethod]
        public void SerializeVCard()
        {
            string s = Utility.CreateVCard().ToVcfString(VCdVersion.V4_0, VcfOptions.All);

            Assert.IsNotNull(s);

            List<VCard> list = VCard.Parse(s);

            Assert.IsNotNull(list);

            Assert.AreEqual(1, list.Count);

            VCard vcard = list[0];

            Assert.AreEqual(VCdVersion.V4_0, vcard.Version);
        }


        [TestMethod]
        public void FburlTest()
        {
            const string workUrl = "WorkUrl";
            const string homeUrl = "HomeUrl";

            const string calendar = "text/calendar";
            const string plain = "text/plain";

            var fburl1 = new TextProperty(workUrl);
            fburl1.Parameters.PropertyClass = PropertyClassTypes.Work;
            fburl1.Parameters.Preference = 1;
            fburl1.Parameters.DataType = VCdDataType.Uri;
            fburl1.Parameters.MediaType = calendar;

            var fburl2 = new TextProperty(homeUrl);
            fburl2.Parameters.PropertyClass = PropertyClassTypes.Home;
            fburl2.Parameters.Preference = 2;
            fburl2.Parameters.DataType = VCdDataType.Text;
            fburl2.Parameters.MediaType = plain;


            var vc = new VCard
            {
                FreeOrBusyUrls = new TextProperty[] { fburl1, fburl2 }
            };

            string s = vc.ToVcfString(VCdVersion.V4_0);

            Assert.IsNotNull(s);

            List<VCard> list = VCard.Parse(s);

            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);

            VCard vc2 = list[0];

            Assert.IsNotNull(vc2);


            IEnumerable<TextProperty?>? fburls = vc2.FreeOrBusyUrls;

            Assert.IsNotNull(fburls);
            Assert.AreEqual(2, fburls!.Count());

            TextProperty fb1 = fburls!.FirstOrDefault(x => x != null && x.Parameters.Preference == 1)!;

            Assert.IsNotNull(fb1);

            Assert.AreEqual(workUrl, fb1.Value);
            Assert.AreEqual(PropertyClassTypes.Work, fb1.Parameters.PropertyClass);
            Assert.AreEqual(calendar, fb1.Parameters.MediaType);
            Assert.AreEqual(VCdDataType.Uri, fb1.Parameters.DataType);


            TextProperty fb2 = fburls!.FirstOrDefault(x => x != null && x.Parameters.Preference == 2)!;

            Assert.IsNotNull(fb2);

            Assert.AreEqual(homeUrl, fb2.Value);
            Assert.AreEqual(PropertyClassTypes.Home, fb2.Parameters.PropertyClass);
            Assert.AreEqual(plain, fb2.Parameters.MediaType);
            Assert.AreEqual(VCdDataType.Text, fb2.Parameters.DataType);

        }


    }
}

