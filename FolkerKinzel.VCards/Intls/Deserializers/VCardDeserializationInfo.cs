using FolkerKinzel.VCards.Intls.Converters;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal sealed class VCardDeserializationInfo
    {
        private const int INITIAL_STRINGBUILDER_CAPACITY = 1024;

        private DateAndOrTimeConverter? _dateAndOrTimeConverter;
        private TimeConverter? _timeConverter;


        internal StringBuilder Builder { get; } = new StringBuilder(INITIAL_STRINGBUILDER_CAPACITY);

#if NET40
        internal readonly char[] Semicolon = new char[] { ';' };

        internal readonly char[] Dot = new char[] { '.' };

        internal readonly char[] EqualSign = new char[] { '=' };

        internal readonly char[] DoubleQuotes = new char[] { '\"' };

        internal readonly char[] Comma = new char[] { ',' };
#else
        internal readonly char Semicolon = ';';

        internal readonly char Dot = '.';

        internal readonly char EqualSign = '=';

        internal readonly char DoubleQuotes = '\"';

        internal readonly char Comma = ',';
#endif

        internal readonly char[] AllQuotes = new char[] { '\"', '\'' };


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

    }
}
