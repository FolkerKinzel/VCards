using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class XMessengerParameterConverterTests
{
    [TestMethod]
    public void ConvertTest1()
    {
        var para = new ParameterSection
        {
            PropertyClass = PropertyClassTypes.Home | PropertyClassTypes.Work,
            PhoneType = PhoneTypes.PCS
        };
        XMessengerParameterConverter.ConvertToInstantMessengerType(para);

        Assert.AreEqual(ImppTypes.Business | ImppTypes.Personal | ImppTypes.Mobile, para.InstantMessengerType);
    }

    [TestMethod]
    public void ConvertTest2()
    {
        var para = new ParameterSection
        {
            InstantMessengerType = ImppTypes.Business | ImppTypes.Personal | ImppTypes.Mobile
        };

        XMessengerParameterConverter.ConvertFromInstantMessengerType(para);

        Assert.AreEqual(PropertyClassTypes.Home | PropertyClassTypes.Work, para.PropertyClass);
        Assert.AreEqual(PhoneTypes.PCS, para.PhoneType);
    }
}

