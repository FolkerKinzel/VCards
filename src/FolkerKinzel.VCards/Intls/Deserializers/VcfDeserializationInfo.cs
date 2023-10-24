using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed class VcfDeserializationInfo
{
    private const int INITIAL_STRINGBUILDER_CAPACITY = 4096;
    private const int MAX_STRINGBUILDER_CAPACITY = 4096 * 4;
    internal const int INITIAL_PARAMETERLIST_CAPACITY = 8;

    private DateAndOrTimeConverter? _dateAndOrTimeConverter;
    private TimeConverter? _timeConverter;

    internal StringBuilder Builder { get; } = new StringBuilder(INITIAL_STRINGBUILDER_CAPACITY);

    internal char[] AllQuotes { get; } = new char[] { '\"', '\'' };

    internal List<KeyValuePair<string, string>> ParameterList { get; } = new();

    internal DateAndOrTimeConverter DateAndOrTimeConverter
    {
        get
        {
            this._dateAndOrTimeConverter ??= new DateAndOrTimeConverter();
            return this._dateAndOrTimeConverter;
        }
    }

    internal TimeConverter TimeConverter
    {
        get
        {
            this._timeConverter ??= new TimeConverter();
            return this._timeConverter;
        }
    }

    internal ValueSplitter SemiColonSplitter { get; } = new ValueSplitter(';', StringSplitOptions.None);

    internal ValueSplitter CommaSplitter { get; } = new ValueSplitter(',', StringSplitOptions.RemoveEmptyEntries);

    internal void Reset()
    {
        Builder.Clear();

        if (Builder.Capacity > MAX_STRINGBUILDER_CAPACITY)
        {
            Builder.Capacity = INITIAL_STRINGBUILDER_CAPACITY;
        }
    }
}
