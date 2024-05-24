using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed class VcfDeserializationInfo
{
    internal const int INITIAL_PARAMETERLIST_CAPACITY = 8;

    private DateTimeConverter? _dateAndOrTimeConverter;
    private TimeConverter? _timeConverter;

    internal List<KeyValuePair<string, ReadOnlyMemory<char>>> ParameterList { get; } = [];

    internal DateTimeConverter DateAndOrTimeConverter
    {
        get
        {
            this._dateAndOrTimeConverter ??= new DateTimeConverter();
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
}
