using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards;

public sealed class VcfReader : IDisposable
{
    private const int DESERIALIZER_QUEUE_INITIAL_CAPACITY = 64;

    private readonly TextReader _textReader;

    public VcfReader(TextReader textReader) 
        => this._textReader = textReader ?? throw new ArgumentNullException(nameof(textReader));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<VCard> ReadToEnd() => EnumerateVCards(_textReader);
    
    internal static IEnumerable<VCard> EnumerateVCards(TextReader reader,
                                                       VCdVersion versionHint = VCdVersion.V2_1)
    {
        var info = new VcfDeserializationInfo();
        var vcfReader = new VcfRowReader(reader, info);
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
                yield return vCard;

                WriteDebug(vCard);

                queue.Clear();
            }
        } while (!vcfReader.EOF);

        [Conditional("DEBUG")]
        static void WriteDebug(VCard vCard)
        {
            Debug.WriteLine("");
            Debug.WriteLine("", "Parsed " + nameof(VCard));
            Debug.WriteLine("");
            Debug.WriteLine(vCard);
        }
    }

    public void Dispose() => this._textReader.Dispose();
}
