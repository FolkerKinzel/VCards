using System.Globalization;
using FolkerKinzel.VCards;

namespace Examples;

internal class Program
{
    private static async Task Main()
    {
        // In order to initialize the library, the executing application MUST be registered
        // with the VCard class. 
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        string directoryPath = Path.GetFullPath("TestFiles");

        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true);
        }

        _ = Directory.CreateDirectory(directoryPath);

        //VcfReaderExample.Example(@"..\..\..\LargeFile.vcf");

        //WebExample.SynchronousExample();

        //await WebExample.AsyncExample();

        //ExtensionMethodExample.Example();

        //NoPidExample.RemovePropertyIdentification();

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
