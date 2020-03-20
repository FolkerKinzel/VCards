using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class XmlPropertyTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "ArgumentException muss geworfen werden, wenn kein XML-Namespace angegeben ist.")]
        public void XmlPropertyTest1()
        {
            const string XML_TEXT1 =
                "<Folker>Kinzel</Folker>";
                

            XElement xelement = XElement.Parse(XML_TEXT1);

            _ = new XmlProperty(xelement);
        }


        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "ArgumentException muss geworfen werden, wenn der vCard 4.0 - XML-Namespace angegeben ist.")]
        public void XmlPropertyTest2()
        {
            const string XML_TEXT1 =
                "<Folker xmlns=\"urn:ietf:params:xml:ns:vcard-4.0\">Kinzel</Folker>";


            XElement xelement = XElement.Parse(XML_TEXT1);

            _ = new XmlProperty(xelement);
        }


        [TestMethod()]
        public void XmlPropertyTest3()
        {
            // null ist erlaubt:
            _ = new XmlProperty(null);

            const string XML_TEXT1 =
                "<Folker xmlns=\"https://www.folker-kinzel.de\">Kinzel</Folker>";


            const string GROUP = "group1";


            XElement xelement = XElement.Parse(XML_TEXT1);

            var prop = new XmlProperty(xelement, GROUP);

            Assert.AreEqual(prop.Group, GROUP);
            Assert.AreEqual(prop.Value, XML_TEXT1);
        }
    }
}