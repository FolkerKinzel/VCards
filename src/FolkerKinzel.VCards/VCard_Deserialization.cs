using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>Loads a VCF file.</summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <param name="textEncoding">The text encoding to use to read the file or <c>null</c>,
    /// to read the file with the standard-compliant text encoding <see cref="Encoding.UTF8"
    /// />.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public static IList<VCard> LoadVcf(string fileName, Encoding? textEncoding = null)
    {
        using StreamReader reader = InitializeStreamReader(fileName, textEncoding);
        return DoParseVcf(reader);
    }


    /// <summary>Parses a <see cref="string" />, that represents the content of a VCF
    /// file.</summary>
    /// <param name="vcf">A <see cref="string" /> that represents the content of a VCF
    /// file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of <paramref name="vcf" />.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="vcf" /> is <c>null</c>.</exception>
    public static IList<VCard> ParseVcf(string vcf)
    {
        if (vcf == null)
        {
            throw new ArgumentNullException(nameof(vcf));
        }

        using var reader = new StringReader(vcf);
        return DoParseVcf(reader);
    }


    /// <summary>Deserializes a VCF file using a <see cref="TextReader" />.</summary>
    /// <param name="reader">A <see cref="TextReader" />.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="reader" /> was closed.</exception>
    /// <exception cref="IOException"> <paramref name="reader" />could not read from
    /// the <see cref="Stream" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<VCard> DeserializeVcf(TextReader reader)
        => DoParseVcf(reader ?? throw new ArgumentNullException(nameof(reader)));


    private static List<VCard> DoParseVcf(TextReader reader, VCdVersion versionHint = VCdVersion.V2_1)
    {
        Debug.Assert(reader != null);
        DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoParseVcf) + "(TextReader)");

        var info = new VcfDeserializationInfo();
        var vCardList = new List<VCard>();
        var vcfReader = new VcfReader(reader, info);
        var queue = new Queue<VcfRow>(DESERIALIZER_QUEUE_INITIAL_CAPACITY);

        do
        {
            foreach (VcfRow vcfRow in vcfReader)
            {
                queue.Enqueue(vcfRow);
            }

            if (queue.Count != 0)
            {
                var vCard = new VCard(queue, info, versionHint);
                vCardList.Add(vCard);

                Debug.WriteLine("");
                Debug.WriteLine("", "Parsed " + nameof(VCard));
                Debug.WriteLine("");
                Debug.WriteLine(vCard);

                queue.Clear();

                if (info.Builder.Capacity > VcfDeserializationInfo.MAX_STRINGBUILDER_CAPACITY)
                {
                    info.Builder.Clear().Capacity = VcfDeserializationInfo.INITIAL_STRINGBUILDER_CAPACITY;
                }
            }
        } while (!vcfReader.EOF);

        return VCard.Dereference(vCardList, false).ToList();
    }


    private static VCard? ParseNestedVcard(string? content, VcfDeserializationInfo info, VCdVersion versionHint)
    {
        // Version 2.1 ist unmaskiert:
        content = versionHint == VCdVersion.V2_1
            ? content
            : content.UnMask(info.Builder, versionHint);

        using var reader = new StringReader(content ?? string.Empty);

        List<VCard> list = DoParseVcf(reader, versionHint);

        return list.FirstOrDefault();
    }


    [ExcludeFromCodeCoverage]
    private static StreamReader InitializeStreamReader(string fileName, Encoding? textEncoding)
    {
        try
        {
            return new StreamReader(fileName, textEncoding ?? Encoding.UTF8, true);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(fileName));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message, nameof(fileName), e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (NotSupportedException e)
        {
            throw new ArgumentException(e.Message, nameof(fileName), e);
        }
        catch (System.Security.SecurityException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (PathTooLongException e)
        {
            throw new ArgumentException(e.Message, nameof(fileName), e);
        }
        catch (Exception e)
        {
            throw new IOException(e.Message, e);
        }
    }

}
