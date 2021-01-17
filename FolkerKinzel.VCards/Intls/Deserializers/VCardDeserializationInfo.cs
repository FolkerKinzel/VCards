﻿using FolkerKinzel.VCards.Intls.Converters;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal sealed class VCardDeserializationInfo
    {
        internal const int INITIAL_STRINGBUILDER_CAPACITY = 4096;
        internal const int MAX_STRINGBUILDER_CAPACITY = 4096*2;

        private DateAndOrTimeConverter? _dateAndOrTimeConverter;
        private TimeConverter? _timeConverter;


        internal StringBuilder Builder { get; } = new StringBuilder(INITIAL_STRINGBUILDER_CAPACITY);


        internal readonly char[] AllQuotes = new char[] { '\"', '\'' };


        internal readonly List<KeyValuePair<string, string>> ParameterList = new List<KeyValuePair<string, string>>();


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
