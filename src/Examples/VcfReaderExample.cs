using FolkerKinzel.VCards;

namespace Examples;

public static class VcfReaderExample
{
    // Reads a very large VCF file whose contents cannot be
    // completely held in memory.
    public static void Example(string filePath)
    {
        using var textReader = new StreamReader(filePath);
        using var reader = new VcfReader(textReader);

        IEnumerable<VCard> result = reader.ReadToEnd();

        Console.WriteLine("The file \"{0}\" contains {1} vCards.",
                          Path.GetFileName(filePath),
                          result.Count());
    }
}

/*
Console Output:

The file "LargeFile.vcf" contains 1000 vCards.
*/