using System.Diagnostics;
using System.Globalization;
using FolkerKinzel.MimeTypes;

namespace Examples;

internal class Program
{
    private static void Main()
    {
        //var uri = new Uri("därectory/image.png", UriKind.RelativeOrAbsolute);

        //var absUri = new Uri("http://a");
        //bool res = Uri.TryCreate(absUri, uri, out uri);
        ////Debug.Assert(uri.IsAbsoluteUri);
        //string[] segments = uri.Segments;
        //Debug.Assert(segments.Length > 0);
        //string segment = segments[segments.Length - 1];

        //string? b = Path.GetExtension(segment);

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
        // VCardExample.ReadingAndWritingVCard(directoryPath);
        // VCard40Example.SaveSingleVCardAsVcf(directoryPath);

        StartAnsiFilterExample();


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
