using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class XMessengerParameterConverterTests
{
    [TestMethod]
    public void ConvertTest1()
    {
        var para = new ParameterSection
        {
            PropertyClass = PCl.Home | PCl.Work,
            PhoneType = Tel.PCS
        };
        XMessengerParameterConverter.ConvertToInstantMessengerType(para);

        Assert.AreEqual(Impp.Business | Impp.Personal | Impp.Mobile, para.InstantMessengerType);
    }

    [TestMethod]
    public void ConvertTest2()
    {
        var para = new ParameterSection
        {
            InstantMessengerType = Impp.Business | Impp.Personal | Impp.Mobile
        };

        XMessengerParameterConverter.ConvertFromInstantMessengerType(para);

        Assert.AreEqual(PCl.Home | PCl.Work, para.PropertyClass);
        Assert.AreEqual(Tel.PCS, para.PhoneType);
    }
}

