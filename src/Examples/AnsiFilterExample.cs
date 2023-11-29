using System.Diagnostics;
using FolkerKinzel.VCards;

namespace Examples;

public static class AnsiFilterExample
{
    /// <summary>
    /// The example loads several vCard 2.1 files which have different encodings and 
    /// shows their content in the text editor. The encoding is selected automatically.
    /// </summary>
    /// <param name="directoryPath">Path to the directory containing the example files.</param>
    /// <remarks>
    /// See the VCF files used in this example at
    /// https://github.com/FolkerKinzel/VCards/tree/master/src/Examples/AnsiFilterExamples
    /// </remarks>
    public static void LoadVcfFilesWhichHaveDifferentAnsiEncodings(string directoryPath)
    {
        // In order to initialize the library, the executing application MUST be registered
        // with the VCard class. To do this, call the static method VCard.RegisterApp with an absolute
        // Uri once when the program starts. (UUID URNs are ideal for this.) This registration
        // is used for the data synchronization mechanism introduced with vCard 4.0 (PID and
        // CLIENTPIDMAP).
        VCard.RegisterApp(new Uri("urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556"));

        // If you have to read VCF files that might have ANSI encodings, use the AnsiFilter
        // to read them automatically with the right encoding.
        // Give the constructor as an argument the ANSI code page that is most likely. This
        // will be the fallback code page if a VCF file couldn't be loaded as UTF-8 and didn't 
        // contain a CHARSET parameter. In our example we choose windows-1255 (Hebrew).
        var ansiFilter = new AnsiFilter(1255);

        var outFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");

        using (StreamWriter writer = File.AppendText(outFileName))
        {
            foreach (string vcfFileName in Directory
                .EnumerateFiles(directoryPath)
                .Where(x => StringComparer.OrdinalIgnoreCase.Equals(Path.GetExtension(x), ".vcf")))
            {
                IList<VCard> vCards = Vcf.Load(vcfFileName, ansiFilter);
                WriteToTextFile(vcfFileName, vCards, ansiFilter.UsedEncoding?.WebName, writer);
            }
        }

        ShowInTextEditorAndDelete(outFileName);
    }
           
    private static void WriteToTextFile(string vcfFileName,
                                        IList<VCard> vCards,
                                        string? encodingWebName,
                                        TextWriter writer)
    {
        const string indent = "    ";
        writer.Write(Path.GetFileName(vcfFileName));
        writer.WriteLine(':');
        writer.Write(indent);
        writer.WriteLine(vCards.FirstOrDefault()?.DisplayNames?.FirstOrDefault()?.Value);
        writer.Write(indent);
        writer.Write("Encoding: ");
        writer.WriteLine(encodingWebName);
        writer.WriteLine();
    }

    private static void ShowInTextEditorAndDelete(string outFileName)
    {
        Process.Start(new ProcessStartInfo { FileName = outFileName, UseShellExecute = true })?
               .WaitForExit();
        File.Delete(outFileName);
    }
}

/*
Output:

 German.vcf:
    Sören Täve Nüßlebaum
    Encoding: windows-1252

Greek.vcf:
    Βαγγέλης
    Encoding: windows-1253

Hebrew.vcf:
    אפרים קישון
    Encoding: windows-1255

Ukrainian.vcf:
    Віталій Володимирович Кличко
    Encoding: windows-1251

utf-8.vcf:
    孔夫子
    Encoding: utf-8

Please note that Hebrew.vcf and utf-8.vcf have been read properly without
any CHARSET parameter in the VCF files: UTF-8 is the default character set 
and windows-1255 (Hebrew) had been set as the default fallback value in the 
AnsiFilter constructor.
 */

