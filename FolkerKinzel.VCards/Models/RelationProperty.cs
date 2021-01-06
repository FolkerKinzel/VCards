using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Abstrakte Basisklasse für Klassen, die die Daten von vCard-Properties kapseln, die Beziehungen zu anderen Menschen
    /// beschreiben. Dies sind insbesondere die vCard-4.0-Property <c>RELATED</c>, die vCard-2.1- und -3.0-Property <c>AGENT</c> sowie
    /// Non-Standard-Properties zur Angabe des Namens des Ehepartners (wie z.B. <c>X-SPOUSE</c>).
    /// </summary>
    public abstract class RelationProperty : VCardProperty
    {
        [InternalProtected]
        internal RelationProperty(ParameterSection parameters, string? propertyGroup)
            : base(parameters, propertyGroup)
        {
            InternalProtectedAttribute.Run();
        }


        /// <summary>
        /// Konstruktor, der von abgeleiteten Klassen aufgerufen wird.
        /// </summary>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum, der die 
        /// Beziehung zu einer Person beschreibt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        protected RelationProperty(RelationTypes? relation, string? propertyGroup)
        {
            this.Group = propertyGroup;
            this.Parameters.RelationType = relation;
        }


        internal static RelationProperty Parse(VcfRow row, VCardDeserializationInfo info, VCdVersion version)
        {
            row.UnMask(info, version);

            if (row.Value is null || row.Parameters.DataType == Enums.VCdDataType.Text)
            {
                return new RelationTextProperty(row, info, version);
            }

            if (row.Value.IsUuid())
            {
                var relation = new RelationUuidProperty(
                    UuidConverter.ToGuid(row.Value),
                    propertyGroup: row.Group);

                relation.Parameters.Assign(row.Parameters);

                return relation;
            }
            else
            {
                if (Uri.TryCreate(row.Value, UriKind.RelativeOrAbsolute, out Uri uri))
                {
                    var relation = new RelationUriProperty(
                        uri,
                        propertyGroup: row.Group);

                    relation.Parameters.Assign(row.Parameters);

                    return relation;
                }
                else
                {
                    return new RelationTextProperty(row, info, version);
                }
            }
        }

    }
}
