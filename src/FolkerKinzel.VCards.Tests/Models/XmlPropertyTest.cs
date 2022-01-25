using System.Xml.Linq;

namespace FolkerKinzel.VCards.Models.Tests;

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

        var xelement = XElement.Parse(XML_TEXT1);
        _ = new XmlProperty(xelement);
    }


    [TestMethod()]
    [ExpectedException(typeof(ArgumentException),
        "ArgumentException muss geworfen werden, wenn der vCard 4.0 - XML-Namespace angegeben ist.")]
    public void XmlPropertyTest2()
    {
        const string XML_TEXT1 =
            "<Folker xmlns=\"urn:ietf:params:xml:ns:vcard-4.0\">Kinzel</Folker>";

        var xelement = XElement.Parse(XML_TEXT1);
        _ = new XmlProperty(xelement);
    }


    [TestMethod()]
    public void XmlPropertyTest3()
    {
        // null ist erlaubt - Der cast ist nur im Test nötig, da hier eine Verwechslungsgefahr
        // mit dem internal ctor besteht:
        _ = new XmlProperty((XElement?)null);

        const string XML_TEXT1 =
            "<Folker xmlns=\"https://www.folker-kinzel.de\">Kinzel</Folker>";

        const string GROUP = "group1";

        var xelement = XElement.Parse(XML_TEXT1);
        var prop = new XmlProperty(xelement, GROUP);

        Assert.AreEqual(prop.Group, GROUP);
        Assert.AreEqual(prop.Value, XML_TEXT1);
    }
}
