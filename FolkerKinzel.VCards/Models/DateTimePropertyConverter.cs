using FK.VCards.Intls.Converters;
using FK.VCards.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK.VCards.Model
{
    /// <summary>
    /// Parst den in einer <see cref="DateTimeProperty"/> gespeicherten Text als
    /// <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    public class DateTimePropertyConverter
    {
        private DateAndOrTimeConverter DateConverter { get; set; }

        private TimeConverter TimeConverter { get; set; }


        public bool TryParseDateTimeValue(DateTimeProperty dateTimeProperty, out DateTimeOffset dateTimeOffset)
        {
            if(dateTimeProperty is null)
            {
                throw new ArgumentNullException(nameof(dateTimeProperty));
            }

                switch (dateTimeProperty.ParameterSection.DataType)
                {
                    case VCdDataType.DateAndOrTime:
                    case VCdDataType.DateTime:
                    case VCdDataType.Timestamp:
                    case null:
                        DateConverter ??= new DateAndOrTimeConverter();
                        return DateConverter.TryParse(dateTimeProperty.Value, out dateTimeOffset);
                    case VCdDataType.Time:
                        TimeConverter ??= new TimeConverter();
                        return TimeConverter.TryParse(dateTimeProperty.Value, out dateTimeOffset);
                    default:
                    dateTimeOffset = DateTimeOffset.MinValue;
                        return false;
                }
        }
    }
}
