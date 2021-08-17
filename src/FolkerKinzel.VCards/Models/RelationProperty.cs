using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Abstrakte Basisklasse für Klassen, die die Daten von vCard-Properties kapseln, die Beziehungen zu anderen Menschen
    /// beschreiben. Dies sind insbesondere die vCard-4.0-Property <c>RELATED</c>, die vCard-2.1- und -3.0-Property <c>AGENT</c> sowie
    /// Non-Standard-Properties zur Angabe des Namens des Ehepartners (wie z.B. <c>X-SPOUSE</c>).
    /// </summary>
    public abstract class RelationProperty : VCardProperty, IEnumerable<RelationProperty>
    {
        /// <summary>
        /// Kopierkonstruktor.
        /// </summary>
        /// <param name="prop">Das zu klonende <see cref="RelationProperty"/>-Objekt.</param>
        protected RelationProperty(RelationProperty prop) : base(prop) { }

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
        protected RelationProperty(RelationTypes? relation, string? propertyGroup) : base(new ParameterSection(), propertyGroup) => this.Parameters.RelationType = relation;


        internal static RelationProperty Parse(VcfRow vcfRow, VCdVersion version)
        {
            vcfRow.UnMask(version);

            if (vcfRow.Value is null || vcfRow.Parameters.DataType == Enums.VCdDataType.Text)
            {
                return new RelationTextProperty(vcfRow);
            }

#if NET40
            if (vcfRow.Value.IsUuidUri())
#else
            if (vcfRow.Value?.AsSpan().IsUuidUri() ?? false)
#endif
            {
                var relation = new RelationUuidProperty(
#if NET40
                    UuidConverter.ToGuid(vcfRow.Value),
#else
                    UuidConverter.ToGuid(vcfRow.Value.AsSpan()),
#endif
                    vcfRow.Parameters.RelationType,
                    propertyGroup: vcfRow.Group);

                relation.Parameters.Assign(vcfRow.Parameters);

                return relation;
            }
            else
            {
                if (Uri.TryCreate(vcfRow.Value, UriKind.RelativeOrAbsolute, out Uri? uri))
                {
                    var relation = new RelationUriProperty(
                        uri,
                        vcfRow.Parameters.RelationType,
                        propertyGroup: vcfRow.Group);

                    relation.Parameters.Assign(vcfRow.Parameters);

                    return relation;
                }
                else
                {
                    return new RelationTextProperty(vcfRow);
                }
            }
        }

        IEnumerator<RelationProperty> IEnumerable<RelationProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<RelationProperty>)this).GetEnumerator();
    }
}
