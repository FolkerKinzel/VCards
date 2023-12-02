using System.IO;
using System.Text;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards;

public static partial class Vcf
{
    /// <summary>Loads a VCF file.</summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <param name="textEncoding">The text encoding to use to read the file or <c>null</c>,
    /// to read the file with the standard-compliant text encoding <see cref="Encoding.UTF8"
    /// />.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class. (See <see cref="VCard.RegisterApp(Uri?)"/>.)</exception>
    public static IList<VCard> Load(string fileName, Encoding? textEncoding = null)
    {
        using StreamReader reader = InitializeStreamReader(fileName, textEncoding);
        return DoDeserialize(reader);
    }

    public static IList<VCard> Load(string fileName, AnsiFilter filter)
        => filter?.Load(fileName) ?? throw new ArgumentNullException(nameof(filter));

    public static IEnumerable<VCard> LoadMany(IEnumerable<string?> fileNames,
                                              AnsiFilter? filter = null)
    {
        _ArgumentNullException.ThrowIfNull(fileNames, nameof(fileNames));

        foreach (var fileName in fileNames)
        {
            if(fileName is null)
            {
                continue;
            }

            IList<VCard> vCards = filter is null ? Load(fileName) 
                                                 : filter.Load(fileName);

            for (int i = 0; i < vCards.Count; i++)
            {
                yield return vCards[i];
            }
        }
    }

    /// <summary>Parses a <see cref="string" />, that represents the content of a VCF
    /// file.</summary>
    /// <param name="vcf">A <see cref="string" /> that represents the content of a VCF
    /// file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of <paramref name="vcf" />.</returns>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="vcf" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class. (See 
    /// <see cref="VCard.RegisterApp(Uri?)"/>.)</exception>
    public static IList<VCard> Parse(string vcf)
    {
        _ArgumentNullException.ThrowIfNull(vcf, nameof(vcf));

        using var reader = new StringReader(vcf);
        return DoDeserialize(reader);
    }

    /// <summary>Deserializes a VCF file using a <see cref="TextReader" />.</summary>
    /// <param name="stream">A <see cref="TextReader" />.</param>
    /// <param name="textEncoding">The text encoding to use to read the file or <c>null</c>,
    /// to read the file with the standard-compliant text encoding <see cref="Encoding.UTF8"
    /// />.</param>
    /// <param name="leaveStreamOpen"><c>true</c> means that <paramref name="stream"/> will
    /// not be closed by the method. The default value is <c>false</c> to close 
    /// <paramref name="stream"/> when the method returns.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="stream"/> doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="stream" /> was closed.
    /// </exception>
    /// <exception cref="IOException"> Could not read from <paramref name="stream"/>.</exception>
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class. (See 
    /// <see cref="VCard.RegisterApp(Uri?)"/>.)</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<VCard> Deserialize(Stream stream,
                                           Encoding? textEncoding = null,
                                           bool leaveStreamOpen = false)
    {
        using var reader = new StreamReader(stream, textEncoding ?? Encoding.UTF8, true, 1024, leaveStreamOpen);
        return DoDeserialize(reader);
    }

    public static IList<VCard> Deserialize(Func<Stream?> factory, AnsiFilter filter)
        => filter?.Deserialize(factory) ?? throw new ArgumentNullException(nameof(filter));

    public static IEnumerable<VCard> DeserializeMany(IEnumerable<Func<Stream?>?> factories, AnsiFilter? filter = null)
    {
        _ArgumentNullException.ThrowIfNull(factories, nameof(factories));

        foreach (var factory in factories)
        {
            if (factory is null)
            {
                continue;
            }

            IList<VCard> vCards;

            if (filter is null)
            {
                using var stream = factory();

                vCards = stream is null ? Array.Empty<VCard>() 
                                        : Deserialize(stream);
            }
            else
            {
                vCards = filter.Deserialize(factory);
            }

            for (int i = 0; i < vCards.Count; i++)
            {
                yield return vCards[i];
            }
        }
    }

    internal static IList<VCard> DoDeserialize(TextReader reader,
                                              VCdVersion versionHint = VCdVersion.V2_1)
    {
        Debug.Assert(reader != null);
        DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoDeserialize) + "(TextReader)");

        var vCardList = VcfReader.EnumerateVCards(reader, versionHint).ToArray();
        VCard.DereferenceIntl(vCardList);

        return vCardList;
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
