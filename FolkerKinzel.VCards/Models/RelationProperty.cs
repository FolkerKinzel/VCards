using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Text;

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
            : base(parameters, propertyGroup) => InternalProtectedAttribute.Run();


        /// <summary>
        /// Konstruktor, der von abgeleiteten Klassen aufgerufen wird.
        /// </summary>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        protected RelationProperty(RelationTypes? relation, string? propertyGroup) : base(propertyGroup) => this.Parameters.RelationType = relation;


        internal static RelationProperty Parse(VcfRow row, VCdVersion version)
        {
            if (row.Value is null || row.Parameters.DataType == Enums.VCdDataType.Text)
            {
                return new RelationTextProperty(row, version);
            }

            row.UnMask(version);
#if NET40
            if (row.Value.IsUuidUri())
#else
            if (row.Value?.AsSpan().IsUuidUri() ?? false)
#endif
            {
                var relation = new RelationUuidProperty(
                    UuidConverter.ToGuid(row.Value),
                    row.Parameters.RelationType,
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
                        row.Parameters.RelationType,
                        propertyGroup: row.Group);

                    relation.Parameters.Assign(row.Parameters);

                    return relation;
                }
                else
                {
                    return new RelationTextProperty(row, version);
                }
            }
        }

    }
}
