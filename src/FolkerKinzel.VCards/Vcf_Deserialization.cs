using FolkerKinzel.Helpers;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;

namespace FolkerKinzel.VCards;

/// <summary>
/// Static class that provides methods to serialize and deserialize VCF files.
/// </summary>
public static partial class Vcf
{
    /// <summary>Loads a VCF file and allows to specify the <see cref="Encoding"/>.</summary>
    /// <param name="filePath">Absolute or relative path to a VCF file.</param>
    /// <param name="textEncoding">The text encoding to use to read the file or <c>null</c>,
    /// to read the file with the standard-compliant text encoding <see cref="Encoding.UTF8"
    /// />.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// 
    /// <remarks>When the method completes, the <see cref="VCard.Dereference(IEnumerable{VCard?})"/> 
    /// method has already been called on the return value.</remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="filePath" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"> <paramref name="filePath" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public static IReadOnlyList<VCard> Load(string filePath, Encoding? textEncoding = null)
    {
        using StreamReader reader = TextFile.OpenRead(filePath, textEncoding);
        return DoDeserialize(reader);
    }

    /// <summary>Loads a VCF file and selects the right <see cref="Encoding"/> automatically.</summary>
    /// <param name="filePath">Absolute or relative path to a VCF file.</param>
    /// <param name="filter">An <see cref="AnsiFilter"/> instance.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// 
    /// <remarks>When the method completes, the <see cref="VCard.Dereference(IEnumerable{VCard?})"/> 
    /// method has already been called on the return value.</remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\AnsiFilterExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> or
    /// <paramref name="filter"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="filePath" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public static IReadOnlyList<VCard> Load(string filePath, AnsiFilter filter)
        => filter?.Load(filePath) ?? throw new ArgumentNullException(nameof(filter));

    /// <summary>
    /// Loads a collection of VCF files and allows to specify an "AnsiFilter" instance for
    /// selecting the right <see cref="Encoding"/> automatically.
    /// </summary>
    /// <param name="filePaths">A collection of absolute or relative paths to VCF files.</param>
    /// <param name="filter">An <see cref="AnsiFilter"/> instance.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects.</returns>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="filePaths"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="filePaths" /> contains an
    /// item that is neither <c>null</c> nor a valid file path.</exception>
    /// <exception cref="IOException">A file could not be loaded.</exception>
    public static IEnumerable<VCard> LoadMany(IEnumerable<string?> filePaths,
                                              AnsiFilter? filter = null)
    {
        _ArgumentNullException.ThrowIfNull(filePaths, nameof(filePaths));

        foreach (string? filePath in filePaths)
        {
            if (filePath is null)
            {
                continue;
            }

            IReadOnlyList<VCard> vCards = filter is null ? Load(filePath)
                                                         : filter.Load(filePath);

            for (int i = 0; i < vCards.Count; i++)
            {
                yield return vCards[i];
            }
        }
    }

    /// <summary>Parses a <see cref="string" /> that represents the content of a VCF
    /// file.</summary>
    /// <param name="vcf">A <see cref="string" /> that represents the content of a VCF
    /// file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of <paramref name="vcf" />.</returns>
    /// 
    /// <remarks>When the method completes, the <see cref="VCard.Dereference(IEnumerable{VCard?})"/> 
    /// method has already been called on the return value.</remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\NoPidExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="vcf" /> is <c>null</c>.
    /// </exception>
    public static IReadOnlyList<VCard> Parse(string vcf)
    {
        _ArgumentNullException.ThrowIfNull(vcf, nameof(vcf));

        using var reader = new StringReader(vcf);
        return DoDeserialize(reader);
    }

    /// <summary>Deserializes a <see cref="Stream"/> of VCF data.</summary>
    /// <param name="stream">The <see cref="Stream"/> to deserialize.</param>
    /// <param name="textEncoding">The text encoding to use for deserialization or <c>null</c>,
    /// to deserialize the <see cref="Stream"/> with the standard-compliant text encoding <see cref="Encoding.UTF8"
    /// />.</param>
    /// <param name="leaveStreamOpen"><c>true</c> means that <paramref name="stream"/> will
    /// not be closed by the method. The default value is <c>false</c> to close 
    /// <paramref name="stream"/> when the method returns.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects.</returns>
    /// 
    /// <remarks>When the method completes, the <see cref="VCard.Dereference(IEnumerable{VCard?})"/> 
    /// method has already been called on the return value.</remarks>
    /// 
    /// 
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="stream"/> doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="stream" /> was closed.
    /// </exception>
    /// <exception cref="IOException"> Could not read from <paramref name="stream"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<VCard> Deserialize(Stream stream,
                                                   Encoding? textEncoding = null,
                                                   bool leaveStreamOpen = false)
    {
        using var reader = new StreamReader(stream, textEncoding ?? Encoding.UTF8, true, 1024, leaveStreamOpen);
        return DoDeserialize(reader);
    }

    /// <summary>
    /// Deserializes a <see cref="Stream"/> of VCF data and selects the right <see cref="Encoding"/>
    /// automatically.
    /// </summary>
    /// <param name="factory">A function that returns a <see cref="Stream"/> or <c>null</c>.</param>
    /// <param name="filter">An <see cref="AnsiFilter"/> instance.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// <see cref="AnsiFilter"/> only recognizes one <see cref="Encoding"/> per <see cref="Stream"/>.
    /// This means that if the <see cref="Stream"/> that <paramref name="factory"/> returns contains 
    /// VCF data with different <see cref="Encoding"/>s, decoding errors may occur.
    /// </para>
    /// <para>
    /// Any <see cref="Stream"/>s that are used within the method will be closed when the method 
    /// completes.
    /// </para>
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\WebExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> or 
    /// <paramref name="filter"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The <see cref="Stream"/> that <paramref name="factory"/> 
    /// returns doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="factory"/> returns a closed stream.
    /// </exception>
    /// <exception cref="IOException"> The method could not read from the <see cref="Stream"/>
    /// that <paramref name="factory"/> returns.</exception>
    public static IReadOnlyList<VCard> Deserialize(Func<Stream?> factory, AnsiFilter filter)
        => filter?.Deserialize(factory) ?? throw new ArgumentNullException(nameof(filter));

    /// <summary>
    /// Deserializes a <see cref="Stream"/> of VCF data in an asynchronous operation.
    /// </summary>
    /// <param name="factory">A function that takes a <see cref="CancellationToken"/> as
    /// argument and returns a <see cref="Stream"/> of VCF data as an 
    /// asynchronous operation.</param>
    /// <param name="textEncoding">The text encoding to use for deserialization or <c>null</c>,
    /// to deserialize the <see cref="Stream"/> with the standard-compliant text encoding <see cref="Encoding.UTF8"
    /// />.</param>
    /// <param name="token">A cancellation token that can be used by other objects or threads to 
    /// receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// 
    /// <remarks>
    /// The <see cref="Stream"/>s that is used within the method will be closed when the method 
    /// completes.
    /// </remarks>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The <see cref="Stream"/> that <paramref name="factory"/> 
    /// returns asynchronously doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="factory"/> returns a closed stream.
    /// </exception>
    /// <exception cref="IOException"> The method could not read from the <see cref="Stream"/>.
    /// </exception>
    public static async Task<IReadOnlyList<VCard>> DeserializeAsync(Func<CancellationToken, Task<Stream>> factory,
                                                                    Encoding? textEncoding = null,
                                                                    CancellationToken token = default)
    {
        _ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        using Stream? stream = await factory(token).ConfigureAwait(false);

        return stream is null ? []
                              : Deserialize(stream, textEncoding);
    }

    /// <summary>
    /// Deserializes a <see cref="Stream"/> of VCF data and selects the correct <see cref="Encoding"/>
    /// automatically in an asynchronous operation.
    /// </summary>
    /// <param name="factory">A function that takes a <see cref="CancellationToken"/> as
    /// argument and returns a <see cref="Stream"/> of VCF data as an 
    /// asynchronous operation.</param>
    /// <param name="filter">An <see cref="AnsiFilter"/> instance.</param>
    /// <param name="token">A cancellation token that can be used by other objects or threads to 
    /// receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// <see cref="AnsiFilter"/> only recognizes one <see cref="Encoding"/> per <see cref="Stream"/>.
    /// This means that if a <see cref="Stream"/> contains VCF data with different <see cref="Encoding"/>s, 
    /// decoding errors may occur.
    /// </para>
    /// <para>
    /// Any <see cref="Stream"/>s that are used within the method will be closed when the method 
    /// completes.
    /// </para>
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\WebExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> or 
    /// <paramref name="filter"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The <see cref="Stream"/> that <paramref name="factory"/> 
    /// returns asynchronously doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="factory"/> returns a closed stream.
    /// </exception>
    /// <exception cref="IOException"> The method could not read from the <see cref="Stream"/>.
    /// </exception>
    public static Task<IReadOnlyList<VCard>> DeserializeAsync(Func<CancellationToken, Task<Stream>> factory,
                                                              AnsiFilter filter,
                                                              CancellationToken token = default)
        => filter?.DeserializeAsync(factory, token) ?? throw new ArgumentNullException(nameof(filter));

    /// <summary>
    /// Deserializes a collection of <see cref="Stream"/>s of VCF data and allows to specify an 
    /// <see cref="AnsiFilter"/> to select the correct <see cref="Encoding"/> automatically.
    /// </summary>
    /// <param name="factories">A collection of functions that return a either a 
    /// <see cref="Stream"/> or <c>null</c>. The collection may return <c>null</c> values.</param>
    /// <param name="filter">An <see cref="AnsiFilter"/> instance, or <c>null</c> to parse all
    /// <see cref="Stream"/>s with the default <see cref="Encoding"/> UTF-8.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// <see cref="AnsiFilter"/> only recognizes one <see cref="Encoding"/> per <see cref="Stream"/>.
    /// This means that if a <see cref="Stream"/> contains VCF data with different <see cref="Encoding"/>s, 
    /// decoding errors may occur.
    /// </para>
    /// <para>
    /// Any <see cref="Stream"/>s that are used within the method will be closed when the method 
    /// completes.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="factories"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">One of the <see cref="Stream"/>s doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException">One of the functions in <paramref name="factories"/> returns a 
    /// closed stream.
    /// </exception>
    /// <exception cref="IOException"> The method could not read from one of the <see cref="Stream"/>s.</exception>
    public static IEnumerable<VCard> DeserializeMany(IEnumerable<Func<Stream?>?> factories, AnsiFilter? filter = null)
    {
        _ArgumentNullException.ThrowIfNull(factories, nameof(factories));

        foreach (Func<Stream?>? factory in factories)
        {
            if (factory is null)
            {
                continue;
            }

            IReadOnlyList<VCard> vCards;

            if (filter is null)
            {
                using Stream? stream = factory();
                vCards = stream is null ? [] : Deserialize(stream);
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

    /// <summary>
    /// Deserializes a collection of <see cref="Stream"/>s of VCF data in an asynchronous operation and allows to specify an 
    /// <see cref="AnsiFilter"/> to select the right <see cref="Encoding"/> automatically.
    /// </summary>
    /// <param name="factories">A collection of functions that return a either a <see cref="Stream"/> or <c>null</c> as an 
    /// asynchronous operation. The collection may return <c>null</c> values.</param>
    /// <param name="filter">An <see cref="AnsiFilter"/> instance, or <c>null</c> to parse all
    /// <see cref="Stream"/>s with the default <see cref="Encoding"/> UTF-8.</param>
    /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of 
    /// cancellation.</param>
    /// <returns>A collection that provides asynchronous iteration over <see cref="VCard"/> values.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// <see cref="AnsiFilter"/> only recognizes one <see cref="Encoding"/> per <see cref="Stream"/>.
    /// This means that if a <see cref="Stream"/> contains VCF data with different <see cref="Encoding"/>s, 
    /// decoding errors may occur.
    /// </para>
    /// <para>
    /// Any <see cref="Stream"/>s that are used within the method will be closed when the method 
    /// completes.
    /// </para>
    /// </remarks>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="factories"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">One of the <see cref="Stream"/>s doesn't support reading.</exception>
    /// <exception cref="ObjectDisposedException">One of the functions in <paramref name="factories"/> returns a 
    /// closed stream.
    /// </exception>
    /// <exception cref="IOException"> The method could not read from one of the <see cref="Stream"/>s.</exception>
    public static async IAsyncEnumerable<VCard> DeserializeManyAsync(IEnumerable<Func<CancellationToken, Task<Stream>>?> factories,
                                                                     AnsiFilter? filter = null,
                                                                     [EnumeratorCancellation] CancellationToken token = default)
    {
        _ArgumentNullException.ThrowIfNull(factories, nameof(factories));

        foreach (Func<CancellationToken, Task<Stream>>? factory in factories)
        {
            if (factory is null)
            {
                continue;
            }

            IReadOnlyList<VCard> vCards;

            if (filter is null)
            {
                using Stream stream = await factory(token).ConfigureAwait(false);
                vCards = stream is null ? [] : Deserialize(stream);
            }
            else
            {
                vCards = await filter.DeserializeAsync(factory, token);
            }

            for (int i = 0; i < vCards.Count; i++)
            {
                yield return vCards[i];
            }
        }
    }

    internal static IReadOnlyList<VCard> DoDeserialize(TextReader reader,
                                              VCdVersion versionHint = VCdVersion.V2_1)
    {
        Debug.Assert(reader is not null);
        DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoDeserialize) + "(TextReader)");

        VCard[] vCards = [.. VcfReader.EnumerateVCards(reader, versionHint)];
        VCard.DereferenceIntl(vCards);

        return vCards;
    }
}
