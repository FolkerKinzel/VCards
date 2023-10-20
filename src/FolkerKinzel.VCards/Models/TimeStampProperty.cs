using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Encapsulates the vCard property <c>REV</c>, which represents a timestamp
    /// of the last update of the <see cref="VCard" />.</summary>
public sealed class TimeStampProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop" />
    private TimeStampProperty(TimeStampProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="TimeStampProperty" /> object, das
    /// den Zeitpunkt des Konstruktoraufrufs als Zeitstempel kapselt. </summary>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks> Der Konstruktor setzt den <see cref="ParameterSection.DataType" />-Parameter
    /// automatisch auf den Wert <see cref="VCdDataType.TimeStamp" />. </remarks>
    public TimeStampProperty(string? propertyGroup = null) : this(DateTimeOffset.UtcNow, propertyGroup) { }


    /// <summary>  Initializes a new <see cref="TimeStampProperty" />-Objekt mit
    /// dem angegebenen Zeitstempel. </summary>
    /// <param name="value">Ein <see cref="DateTimeOffset" />-Objekt.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks> Der Konstruktor setzt den <see cref="ParameterSection.DataType" />-Parameter
    /// automatisch auf den Wert <see cref="VCdDataType.TimeStamp" />. </remarks>
    public TimeStampProperty(DateTimeOffset value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
    {
        Value = value;
        Parameters.DataType = VCdDataType.TimeStamp;
    }


    internal TimeStampProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        // ein statischer DateAndOrTimeConverter kann nicht benutzt werden, da das die 
        // Threadsafety zerstören würde:
        if (vcfRow.Info.DateAndOrTimeConverter.TryParse(vcfRow.Value.AsSpan(), out OneOf<DateOnly, DateTimeOffset> value))
        {
            Value = value.Match<DateTimeOffset>(
                dateOnly => new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local)),
                dateTimeOffset => dateTimeOffset);
            Parameters.DataType = VCdDataType.TimeStamp;
        }
    }


    /// <summary> The data provided by the  <see cref="TimeStampProperty" />. </summary>
    public new DateTimeOffset Value
    {
        get;
    }


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    /// <inheritdoc />
    public override bool IsEmpty => Value < new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        StringBuilder worker = serializer.Worker;
        _ = worker.Clear();
        DateAndOrTimeConverter.AppendTimeStampTo(worker, this.Value, serializer.Version);
        _ = worker.Mask(serializer.Version);
        _ = serializer.Builder.Append(worker);
    }

    /// <inheritdoc />
    public override object Clone() => new TimeStampProperty(this);
}
