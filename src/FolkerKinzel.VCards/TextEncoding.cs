namespace FolkerKinzel.VCards;

public class TextEncoding
{
    private DecoderValidationFallback? _validator;

    public TextEncoding(string? encodingWebName = null)
    {
        Default = encodingWebName is null
                             ? Encoding.UTF8 
                             : TextEncodingConverter.GetEncoding(encodingWebName, true);
    }

    public TextEncoding(int codePage) => Default = TextEncodingConverter.GetEncoding(codePage, true);

    public string? DefaultEncoding => Default.WebName;

    /// <summary> <see cref="Encoding.WebName" /> property
    /// of the <see cref="Encoding" /> object to use as a fallback. </summary>
    public string? FallbackEncoding {  get; private set; }

    public void SetFallbackEncoding(string? encodingWebName)
    {
        if (encodingWebName is null)
        {
            Fallback = null;
            return;
        }

        Fallback = TextEncodingConverter.GetEncoding(encodingWebName, true);
        InitValidator();
    }

    public void SetFallbackEncoding(int codePage)
    {
        Fallback = TextEncodingConverter.GetEncoding(codePage, true);
        InitValidator();
    }

    private void InitValidator()
    {
        if (_validator is null)
        {
            _validator = new DecoderValidationFallback();
            Default = TextEncodingConverter.GetEncoding(Default.CodePage, EncoderFallback.ReplacementFallback, _validator);
        }
    }

    internal Encoding Default { get; private set; }

    internal Encoding? Fallback { get; private set; }

    internal bool HasError => _validator is not null && _validator.HasError;

    internal void Reset() => _validator?.Reset();
}
