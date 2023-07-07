using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class XMessengerParameterConverterTests
{
    [TestMethod]
    public void ConvertTest1()
    {
        var para = new ParameterSection();
        para.PropertyClass = PropertyClassTypes.Home | PropertyClassTypes.Work;
        para.TelephoneType = TelTypes.PCS;
        XMessengerParameterConverter.ConvertToInstantMessengerType(para);

        Assert.AreEqual(ImppTypes.Business | ImppTypes.Personal | ImppTypes.Mobile, para.InstantMessengerType);
    }

    [TestMethod]
    public void ConvertTest2()
    {
        var para = new ParameterSection();
        para.InstantMessengerType = ImppTypes.Business | ImppTypes.Personal | ImppTypes.Mobile;
        XMessengerParameterConverter.ConvertFromInstantMessengerType(para);

        Assert.AreEqual(PropertyClassTypes.Home | PropertyClassTypes.Work, para.PropertyClass);
        Assert.AreEqual(TelTypes.PCS, para.TelephoneType);
    }
}

