using FolkerKinzel.MimeTypes.Properties;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;

namespace Benchmarks;

internal static class VCardProvider
{

    internal static VCard CreatePhotoVCard()
    {
//        const string photoFilePath =
//#if DEBUG
//        @"..\..\..\TestFiles\Folker.png";
//#else
//        @"..\..\..\..\..\..\..\TestFiles\Folker.png";
//#endif
//        //using System.IO.UnmanagedMemoryStream? stream = Res.ResourceManager.GetStream("Folker");
        
//        var picBytes = (byte[]?)Res.ResourceManager.GetObject("Folker");

        return VCardBuilder
            .Create(CreateVCard())
            .Photos.AddBytes(Properties.Res.Folker, "image/png")
            //.Photos.AddFile("C:\\Users\\fkinz\\source\\repos\\FolkerKinzel.VCards\\src\\Benchmarks\\TestFiles\\Folker.png")
            .VCard;
    }

    internal static VCard CreateVCard()
    {
        return VCardBuilder
            .Create()
            .NameViews.Add(NameBuilder.Create().AddFamilyName( "Mustermann").AddGivenName( "Jürgen"))
            .DisplayNames.Add("Jürgen Mustermann")
            .EMails.Add("juergen@home.de", parameters: p => p.PropertyClass = PCl.Home)
            .EMails.Add("juergen@work.com", parameters: p => p.PropertyClass = PCl.Work)
            .EMails.SetPreferences()

            .Phones.Add("01234-56789876", parameters: p => p.PhoneType = Tel.Cell)
            .Phones.Add("09876-54321234", parameters: p => { p.PropertyClass = PCl.Home; p.PhoneType = Tel.Voice | Tel.Fax; })
            .Phones.SetPreferences()

            .Addresses.Add("Blümchenweg 14a", "Göppingen", null, "73035")
            .BirthDayViews.Add(1985, 5, 7)
            .VCard;
    }
}