using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
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
    /// ist die <see cref="ParameterSection.DataType"/>-Eigenschaft hilfreich.</remarks>
    public abstract class DateTimeProperty : VCardProperty
    {
        /// <summary>
        /// Konstruktor, der abgeleiteten Klassen erlaubt, ein neues <see cref="DateTimeProperty"/>-Objekt zu initialisieren,
        /// bei dem ein Gruppenname für vCard-Properties angegeben ist.
        /// </summary>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        protected DateTimeProperty(string? propertyGroup) { this.Group = propertyGroup; }


        internal static DateTimeProperty Create(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
        {
            Debug.Assert(vcfRow != null);
            Debug.Assert(info != null);

            //if(version == VCdVersion.V2_1)
            //{
            //    vcfRow.Parameters.DataType = VCdDataType.Date;
            //}
            //else if(version == VCdVersion.V3_0 && !vcfRow.Parameters.DataType.HasValue)
            //{
            //    vcfRow.Parameters.DataType = VCdDataType.DateAndOrTime;
            //}


            switch (vcfRow.Parameters.DataType)
            {
                case VCdDataType.DateAndOrTime:
                case VCdDataType.DateTime:
                case VCdDataType.Date:
                case VCdDataType.Timestamp:
                case null:
                    {
                        if (info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset dateTimeOffset))
                        {
                            return BuildDateTimeOffsetProperty(dateTimeOffset);
                        }
                        else
                        {
                            return BuildDateTimeTextProperty();
                        }
                    }
                case VCdDataType.Time:
                    {

                        if (info.TimeConverter.TryParse(vcfRow.Value, out DateTimeOffset dateTimeOffset))
                        {
                            return BuildDateTimeOffsetProperty(dateTimeOffset);
                        }
                        else
                        {
                            return BuildDateTimeTextProperty();
                        }
                    }
                default:
                    {
                        return BuildDateTimeTextProperty();
                    }

            }//switch


            string UnMask()
            {
                StringBuilder builder = info.Builder;
                builder.Clear().Append(vcfRow.Value).UnMask(version);

                return builder.ToString();
            }

            DateTimeTextProperty BuildDateTimeTextProperty()
            {
                var textProp = new DateTimeTextProperty(UnMask(), vcfRow.Group);

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
    }// class DateTimeProperty
}
