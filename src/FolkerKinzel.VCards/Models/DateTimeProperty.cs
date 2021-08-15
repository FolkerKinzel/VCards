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
        /// Kopierkonstruktor
        /// </summary>
        /// <param name="prop">Das zu klonende <see cref="DateTimeProperty"/>-Objekt.</param>
        protected DateTimeProperty(DateTimeProperty prop) : base(prop) { }

        /// <summary>
        /// Konstruktor, der abgeleiteten Klassen erlaubt, ein neues <see cref="DateTimeProperty"/>-Objekt zu initialisieren.
        /// </summary>
        /// <param name="parameters">Ein <see cref="ParameterSection"/>-Objekt, das den Parameter-Teil einer
        /// vCard-Property repräsentiert.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        protected DateTimeProperty(ParameterSection parameters, string? propertyGroup) : base(parameters, propertyGroup) { }


        internal static DateTimeProperty Create(VcfRow vcfRow, VCdVersion version)
        {
            Debug.Assert(vcfRow != null);

            switch (vcfRow.Parameters.DataType)
            {
                case VCdDataType.DateAndOrTime:
                case VCdDataType.DateTime:
                case VCdDataType.Date:
                case VCdDataType.TimeStamp:
                case null:
                    {
                        return vcfRow.Info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset dateTimeOffset)
                            ? new DateTimeOffsetProperty(dateTimeOffset, vcfRow)
                            : new DateTimeTextProperty(vcfRow, version);
                    }
                case VCdDataType.Time:
                    {

                        return vcfRow.Info.TimeConverter.TryParse(vcfRow.Value, out DateTimeOffset dateTimeOffset)
                            ? new DateTimeOffsetProperty(dateTimeOffset, vcfRow)
                            : new DateTimeTextProperty(vcfRow, version);
                    }
                default:
                    {
                        return new DateTimeTextProperty(vcfRow, version);
                    }

            }//switch
        }

        IEnumerator<DateTimeProperty> IEnumerable<DateTimeProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateTimeProperty>)this).GetEnumerator();

    }// class DateTimeProperty
}
