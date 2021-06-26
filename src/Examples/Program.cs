using System;
using System.Globalization;
using System.IO;
using System.Threading;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace Examples
{
    internal class Program
    {
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;


            string directoryPath = Path.GetFullPath("TestFiles");

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            _ = Directory.CreateDirectory(directoryPath);


            //WhatsAppDemo1.IntegrateWhatsAppNumberUsingIMPP();
            //WhatsAppDemo2.UsingTheWhatsAppType();
            VCardExample.ReadingAndWritingVCard(directoryPath);
            //VCard40Example.SaveSingleVCardAsVcf(directoryPath);
        }
    }
}
