using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed class VcfDeserializationInfo
{
    internal const int INITIAL_PARAMETERLIST_CAPACITY = 8;

    private DateAndOrTimeConverter? _dateAndOrTimeConverter;
    private TimeConverter? _timeConverter;
    private TimeStampConverter? _timeStampConverter;

    internal List<KeyValuePair<string, ReadOnlyMemory<char>>> ParameterList { get; } = [];

    internal DateAndOrTimeConverter DateAndOrTimeConverter
    {
        get
        {
            this._dateAndOrTimeConverter ??= new DateAndOrTimeConverter();
            return this._dateAndOrTimeConverter;
        }
    }

    internal TimeStampConverter TimeStampConverter
    {
        get
        {
            this._timeStampConverter ??= new TimeStampConverter();
            return this._timeStampConverter;
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
