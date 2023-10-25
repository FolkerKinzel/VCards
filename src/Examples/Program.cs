using System.Diagnostics;
using System.Globalization;
using FolkerKinzel.MimeTypes;

namespace Examples;

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

        //EmbeddedVCardExample.FromVCardExample();

        //WhatsAppDemo1.IntegrateWhatsAppNumberUsingIMPP();
        //WhatsAppDemo2.UsingTheWhatsAppType();
         VCardExample.ReadingAndWritingVCard(directoryPath);
         //VCard40Example.SaveSingleVCardAsVcf(directoryPath);

        //StartAnsiFilterExample();


        //string destinationPath = Path.Combine(sourcePath, "Ansi");

        //string fileName = "Hebrew.vcf";
        //int codePage = 1255;
        //System.Text.Encoding enc = FolkerKinzel.Strings.TextEncodingConverter.GetEncoding(codePage);
        //string s = File.ReadAllText(Path.Combine(sourcePath, fileName));
        //File.WriteAllText(Path.Combine(destinationPath, fileName), s, enc);

    }



    private static void StartAnsiFilterExample()
    {
        string sourcePath = Path.GetFullPath(@"..\..\..\AnsiFilterExamples");
        AnsiFilterExample.LoadVcfFilesWhichHaveDifferentAnsiEncodings(sourcePath);
    }
}
