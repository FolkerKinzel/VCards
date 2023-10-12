using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Abstrakte Basisklasse für Klassen, die die Daten von vCard-Properties kapseln, die Beziehungen zu anderen Menschen
/// beschreiben. Dies sind insbesondere die vCard-4.0-Property <c>RELATED</c>, die vCard-2.1- und -3.0-Property <c>AGENT</c> sowie
/// Non-Standard-Properties zur Angabe des Namens des Ehepartners (wie z.B. <c>X-SPOUSE</c>).
/// </summary>
public abstract class RelationProperty : VCardProperty, IEnumerable<RelationProperty>
{
    private bool _isValueInitialized;
    private Relation? _value;

    public static RelationProperty FromUri(Uri? uri,
                                    RelationTypes? relation = null,
                                    string? propertyGroup = null)
        => uri is null
            ? FromText(null, relation, propertyGroup)
            : !uri.IsAbsoluteUri
                ? throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)), nameof(uri))
                : new RelationUriProperty
                    (
                    new UriProperty(uri, new ParameterSection() { RelationType = relation }, propertyGroup)
                    );


    public static RelationProperty FromText(string? text,
                                            RelationTypes? relation = null,
                                            string? propertyGroup = null)
    {
        var prop = new TextProperty(text, propertyGroup);
        prop.Parameters.RelationType = relation;
        prop.Parameters.DataType = VCdDataType.Text;

        return new RelationTextProperty(prop);
    }

    public static RelationProperty FromGuid(Guid uuid,
                                            RelationTypes? relation = null,
                                            string? propertyGroup = null)
        => new RelationUuidProperty(uuid, relation, propertyGroup);


    public static RelationProperty FromVCard(VCard? vCard,
                                             RelationTypes? relation = null,
                                             string? propertyGroup = null)
        => vCard is null
            ? FromText(null, relation, propertyGroup)
            : new RelationVCardProperty(vCard, relation, propertyGroup);


    public new Relation? Value
    {
        get
        {
            if (!_isValueInitialized)
            {
                InitializeValue();
            }

            return _value;
        }
    }

    /// <inheritdoc/>
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? base.ToString();


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    /// <summary>
    /// Kopierkonstruktor.
    /// </summary>
    /// <param name="prop">Das zu klonende <see cref="RelationProperty"/>-Objekt.</param>
    protected RelationProperty(RelationProperty prop) : base(prop) { }

    /// <summary>
    /// Konstruktor, der von abgeleiteten Klassen beim Parsen von VCF-Dateien verwendet wird.
    /// </summary>
    /// <param name="parameters">Die <see cref="ParameterSection"/> des <see cref="RelationProperty"/> Objekts.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    protected RelationProperty(ParameterSection parameters, string? propertyGroup)
        : base(parameters, propertyGroup) { }


    /// <summary>
    /// Konstruktor, der von abgeleiteten Klassen aufgerufen wird.
    /// </summary>
    /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    protected RelationProperty(RelationTypes? relation, string? propertyGroup)
        : base(new ParameterSection(), propertyGroup)
        => this.Parameters.RelationType = relation;


    internal static RelationProperty Parse(VcfRow vcfRow, VCdVersion version)
    {
        vcfRow.UnMask(version);

        if (string.IsNullOrWhiteSpace(vcfRow.Value) || vcfRow.Parameters.DataType == Enums.VCdDataType.Text)
        {
            return new RelationTextProperty(vcfRow, version);
        }

        if (vcfRow.Value.IsUuidUri())
        {
            var relation = new RelationUuidProperty(UuidConverter.ToGuid(vcfRow.Value),
                                                    vcfRow.Parameters.RelationType,
                                                    propertyGroup: vcfRow.Group);

            relation.Parameters.Assign(vcfRow.Parameters);

            return relation;
        }
        else if (Uri.TryCreate(vcfRow.Value.Trim(), UriKind.Absolute, out Uri? uri))
        {
            var relation = new RelationUriProperty
                (
                new UriProperty(uri, vcfRow.Parameters, propertyGroup: vcfRow.Group)
                );

            return relation;
        }
        else
        {
            return new RelationTextProperty(vcfRow, version);
        }
    }

    IEnumerator<RelationProperty> IEnumerable<RelationProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<RelationProperty>)this).GetEnumerator();

    private void InitializeValue()
    {
        _isValueInitialized = true;

        _value = this switch
        {
            RelationTextProperty tProp => tProp.IsEmpty ? null : new Relation(tProp.Value),

            // The vCard could be empty or not when initializing, but users get
            // always a reference to it to change it afterwards:
            RelationVCardProperty vcProp => new Relation(vcProp.Value),

            RelationUuidProperty guidProp => guidProp.IsEmpty ? null : new Relation(guidProp.Value),
            RelationUriProperty uriProp => new Relation(uriProp.Value),
            _ => null
        };
    }
}
