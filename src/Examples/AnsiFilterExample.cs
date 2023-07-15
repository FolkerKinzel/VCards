// Compile for .NET 7.0 or above and FolkerKinzel.VCards 5.0.0 or above
using System.Diagnostics;
using FolkerKinzel.VCards;

namespace Examples;

public static class MultiAnsiFilterExample
{
    /// <summary>
    /// The example loads several vCard 2.1 files which have different encodings and 
    /// shows their content in the text editor. The encoding is selected automatically.
    /// </summary>
    /// <remarks>
    /// Download the example files at
    /// https://github.com/FolkerKinzel/VCards/tree/1ac0a8ee718c18a3c0e187a955773ebae845ec8f/src/Examples/MultiAnsiFilterExamples
    /// </remarks>
    /// <param name="directoryPath">Path to the directory containing the example files.</param>
    public static void LoadVcfFilesWhichHaveDifferentAnsiEncodings(string directoryPath)
    {
        // If you have to read VCF files which might have ANSI encodings, use the 
        // AnsiFilter to read them automatically with the right encoding.
        // Give the constructor the ANSI code page as an argument which is most likely. This will
        // be the fallback code page if a VCF file couldn't be loaded as UTF-8 and didn't 
        // contain a CHARSET parameter. In our example we choose windows-1255 (Hebrew).
        var multiAnsiFilter = new MultiAnsiFilter(1255);

        var outFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
        using (StreamWriter writer = File.AppendText(outFileName))
        {
            foreach (string vcfFileName in Directory
                .EnumerateFiles(directoryPath)
                .Where(x => StringComparer.OrdinalIgnoreCase.Equals(Path.GetExtension(x), ".vcf")))
            {
                IList<VCard> vCards = multiAnsiFilter.LoadVcf(vcfFileName, out string encodingWebName);
                WriteToTextFile(vcfFileName, vCards, encodingWebName, writer);
            }
        }
        ShowInTextEditorAndDelete(outFileName);
    }
           
    private static void WriteToTextFile(string vcfFileName, IList<VCard> vCards, string encodingWebName, TextWriter writer)
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
constructor.
 */

