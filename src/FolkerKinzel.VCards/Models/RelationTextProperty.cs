using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um den Namen einer Person, zu der eine Beziehung besteht, anzugeben.
/// </summary>
public sealed class RelationTextProperty : RelationProperty, IEnumerable<RelationTextProperty>
{
    /// <summary>
    /// Copy ctor.
    /// </summary>
    /// <param name="prop"></param>
    private RelationTextProperty(RelationTextProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="RelationTextProperty"/>-Objekt.
    /// </summary>
    /// <param name="text">Name einer Person, zu der eine Beziehung besteht oder <c>null</c>.</param>
    /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    public RelationTextProperty(string? text, RelationTypes? relation = null, string? propertyGroup = null)
        : base(relation, propertyGroup)
    {
        this.Parameters.DataType = VCdDataType.Text;

        if (!string.IsNullOrWhiteSpace(text))
        {
            this.Value = text;
        }
    }


    internal RelationTextProperty(VcfRow row) : base(row.Parameters, row.Group)
        => this.Value = string.IsNullOrWhiteSpace(row.Value) ? null : row.Value;


    /// <summary>
    /// Die von der <see cref="RelationTextProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new string? Value
    {
        get;
    }


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        if (serializer.Version == VCdVersion.V2_1 && Value != null && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = ValueEncoding.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }

        this.Parameters.DataType = VCdDataType.Text;
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        StringBuilder builder = serializer.Builder;
        StringBuilder worker = serializer.Worker;

        if (serializer.Version == VCdVersion.V2_1)
        {
            _ = this.Parameters.Encoding == ValueEncoding.QuotedPrintable
                ? builder.Append(QuotedPrintable.Encode(Value, builder.Length))
                : builder.Append(Value);
        }
        else
        {
            _ = worker.Clear().Append(Value).Mask(serializer.Version);
            _ = builder.Append(worker);
        }
    }

    IEnumerator<RelationTextProperty> IEnumerable<RelationTextProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc/>
    public override object Clone() => new RelationTextProperty(this);

}
