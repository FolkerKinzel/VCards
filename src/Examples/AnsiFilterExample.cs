// Compile for .NET 7.0 or above and FolkerKinzel.VCards 5.0.0 or above
using System.Diagnostics;
using FolkerKinzel.VCards;

namespace Examples;

public static class AnsiFilterExample
{
    /// <summary>
    /// The example loads VCF files which have different encodings and 
    /// shows their content in the text editor. The encoding is selected automatically.
    /// </summary>
    /// <remarks>
    /// Download the example files at
    /// https://github.com/FolkerKinzel/VCards/tree/1ac0a8ee718c18a3c0e187a955773ebae845ec8f/src/Examples/AnsiFilterExamples
    /// </remarks>
    /// <param name="directoryPath">Path to the directory containing the example files.</param>
    public static void LoadVcfFilesWhichHaveDifferentAnsiEncodings(string directoryPath)
    {
        // To load VCF files that could be ANSI encoded automatically with the right encoding,
        // use the AnsiFilter class with the ANSI codepage which is most likely. In our example
        // we choose windows-1251 (Cyrillic).
        // The AnsiFilter object switches - depending of the content of the VCF file - automatically
        // between UTF-8 and this code page.
        var ansiFilter = new AnsiFilter(1255);

        var outFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
        using (StreamWriter writer = File.AppendText(outFileName))
        {
            foreach (string vcfFileName in Directory
                .EnumerateFiles(directoryPath)
                .Where(x => StringComparer.OrdinalIgnoreCase.Equals(Path.GetExtension(x), ".vcf")))
            {
                IList<VCard> vCards = ansiFilter.LoadVcf(vcfFileName, out string encodingWebName);
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

 Ukrainian.vcf:
    Віталій Володимирович Кличко
    Encoding: windows-1251

utf-8.vcf:
    孔夫子
    Encoding: utf-8
 */

