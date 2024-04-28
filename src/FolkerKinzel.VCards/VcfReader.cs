using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards;

/// <summary>
/// Allows iterating through the contents of large VCF files without having to keep all 
/// the parsed content in memory.
/// </summary>
/// <example>
/// <code language="cs" source="..\Examples\VcfReaderExample.cs"/>
/// </example>
public sealed class VcfReader : IDisposable
{
    private const int DESERIALIZER_QUEUE_INITIAL_CAPACITY = 64;

    private readonly TextReader _textReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="VcfReader"/> class.
    /// </summary>
    /// <param name="reader">The <see cref="TextReader"/> to use for parsing the VCF data.</param>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VcfReaderExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <c>null</c>.</exception>
    public VcfReader(TextReader reader)
        => this._textReader = reader ?? throw new ArgumentNullException(nameof(reader));

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> of <see cref="VCard"/> objects that 
    /// can be used to iterate over the VCF contents.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="VCard"/> objects.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// Unlike the methods of the <see cref="Vcf"/> class, the <see cref="VCard.Dereference(IEnumerable{VCard?})"/>
    /// method is not called on the return value. The <see cref="AnsiFilter"/> class cannot be used by <see cref="VcfReader"/>
    /// either.
    /// </para>
    /// <para>
    /// If possible, use the <see cref="Vcf"/> class if it is justifiable to keep the parsed content completely in 
    /// memory. The utility of the <see cref="VcfReader"/> class is to read huge files.
    /// </para>
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VcfReaderExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="IOException">Could not read from the source.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<VCard> ReadToEnd() => EnumerateVCards(_textReader);

    internal static IEnumerable<VCard> EnumerateVCards(TextReader reader,
                                                       VCdVersion versionHint = VCdVersion.V2_1)
    {
        var info = new VcfDeserializationInfo();
        var rowReader = new VcfRowReader(reader, info);
        var queue = new Queue<VcfRow>(DESERIALIZER_QUEUE_INITIAL_CAPACITY);

        do
        {
            foreach (VcfRow vcfRow in rowReader)
            {
                queue.Enqueue(vcfRow);
            }

            if (queue.Count != 0)
            {
                var vCard = new VCard(queue, info, versionHint);
                yield return vCard;

                WriteDebug(vCard);

                queue.Clear();
            }

        } while (!rowReader.EOF);

        [Conditional("DEBUG")]
        [ExcludeFromCodeCoverage]
        static void WriteDebug(VCard vCard)
        {
            Debug.WriteLine("");
            Debug.WriteLine("", "Parsed " + nameof(VCard));
            Debug.WriteLine("");
            Debug.WriteLine(vCard);
        }
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="VcfReader"/> class.
    /// </summary>
    public void Dispose() => this._textReader.Dispose();
}
