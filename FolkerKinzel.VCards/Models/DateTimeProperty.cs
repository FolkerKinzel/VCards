using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt Informationen über Zeit und Datum.
    /// </summary>
    /// <remarks>Die Informationen könnnen in verschiedenen Formaten vorliegen. So ist z.B. in vCard 4.0 auch
    /// freier Text wie "dienstags nach Mitternacht" möglich. Um herauszufinden, welcher Art die enthaltene Information ist,
    /// ist die <see cref="ParameterSection.DataType">Parameters.DataType</see>-Eigenschaft hilfreich.</remarks>
    public abstract class DateTimeProperty : VCardProperty, IEnumerable<DateTimeProperty>
    {
        /// <summary>
        /// Konstruktor, der abgeleiteten Klassen erlaubt, ein neues <see cref="DateTimeProperty"/>-Objekt zu initialisieren.
        /// </summary>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        protected DateTimeProperty(string? propertyGroup) : base(propertyGroup) { }


        internal static DateTimeProperty Create(VcfRow vcfRow, VCdVersion version)
        {
            Debug.Assert(vcfRow != null);


            switch (vcfRow.Parameters.DataType)
            {
                case VCdDataType.DateAndOrTime:
                case VCdDataType.DateTime:
                case VCdDataType.Date:
                case VCdDataType.Timestamp:
                case null:
                    {
                        return vcfRow.Info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset dateTimeOffset)
                            ? BuildDateTimeOffsetProperty(dateTimeOffset)
                            : (DateTimeProperty)BuildDateTimeTextProperty();
                    }
                case VCdDataType.Time:
                    {

                        return vcfRow.Info.TimeConverter.TryParse(vcfRow.Value, out DateTimeOffset dateTimeOffset)
                            ? BuildDateTimeOffsetProperty(dateTimeOffset)
                            : (DateTimeProperty)BuildDateTimeTextProperty();
                    }
                default:
                    {
                        vcfRow.UnMask(version);
                        return BuildDateTimeTextProperty();
                    }

            }//switch


            DateTimeTextProperty BuildDateTimeTextProperty()
            {
                var textProp = new DateTimeTextProperty(vcfRow.Value, vcfRow.Group);

                textProp.Parameters.AltID = vcfRow.Parameters.AltID;
                textProp.Parameters.Language = vcfRow.Parameters.Language;
                textProp.Parameters.NonStandardParameters = vcfRow.Parameters.NonStandardParameters;

                return textProp;
            }

            DateTimeOffsetProperty BuildDateTimeOffsetProperty(DateTimeOffset dateTimeOffset)
            {
                var dtProp = new DateTimeOffsetProperty(dateTimeOffset, vcfRow.Group);

                dtProp.Parameters.DataType = vcfRow.Parameters.DataType ?? VCdDataType.DateAndOrTime;
                dtProp.Parameters.AltID = vcfRow.Parameters.AltID;
                dtProp.Parameters.Language = vcfRow.Parameters.Calendar;
                dtProp.Parameters.NonStandardParameters = vcfRow.Parameters.NonStandardParameters;

                return dtProp;
            }

        }

        IEnumerator<DateTimeProperty> IEnumerable<DateTimeProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateTimeProperty>)this).GetEnumerator();

    }// class DateTimeProperty
}
