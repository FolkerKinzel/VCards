using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Represents the vCard property <c>N</c>, which stores the name of the
    /// vCard subject.</summary>
    /// <remarks>
    /// <note type="tip">
    /// Sie können die Methode <see cref="NameProperty.ToDisplayName" /> verwenden,
    /// um aus den strukturierten Namensdarstellungen formatierte Darstellungen zu erzeugen,
    /// die für die Benutzer der Anwendung lesbar sind.
    /// </note>
    /// </remarks>
public sealed class NameProperty : VCardProperty, IEnumerable<NameProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop" />
    private NameProperty(NameProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="NameProperty" /> object. </summary>
    /// <param name="lastName" />
    /// <param name="firstName" />
    /// <param name="middleName" />
    /// <param name="prefix" />
    /// <param name="suffix" />
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="ToDisplayName" />
    public NameProperty(
        IEnumerable<string?>? lastName = null,
        IEnumerable<string?>? firstName = null,
        IEnumerable<string?>? middleName = null,
        IEnumerable<string?>? prefix = null,
        IEnumerable<string?>? suffix = null,
        string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
    {
        Value = new Name(lastName: ReadOnlyCollectionConverter.ToReadOnlyCollection(lastName),
                         firstName: ReadOnlyCollectionConverter.ToReadOnlyCollection(firstName),
                         middleName: ReadOnlyCollectionConverter.ToReadOnlyCollection(middleName),
                         prefix: ReadOnlyCollectionConverter.ToReadOnlyCollection(prefix),
                         suffix: ReadOnlyCollectionConverter.ToReadOnlyCollection(suffix));
    }


    /// <summary>  Initializes a new <see cref="NameProperty" /> object. </summary>
    /// <param name="lastName" />
    /// <param name="firstName" />
    /// <param name="middleName" />
    /// <param name="prefix" />
    /// <param name="suffix" />
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="ToDisplayName" />
    public NameProperty(
        string? lastName,
        string? firstName = null,
        string? middleName = null,
        string? prefix = null,
        string? suffix = null,
        string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
    {
        Value = new Name(lastName: ReadOnlyCollectionConverter.ToReadOnlyCollection(lastName),
                         firstName: ReadOnlyCollectionConverter.ToReadOnlyCollection(firstName),
                         middleName: ReadOnlyCollectionConverter.ToReadOnlyCollection(middleName),
                         prefix: ReadOnlyCollectionConverter.ToReadOnlyCollection(prefix),
                         suffix: ReadOnlyCollectionConverter.ToReadOnlyCollection(suffix));
    }


    internal NameProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.DecodeQuotedPrintable();

        Value = string.IsNullOrWhiteSpace(vcfRow.Value) ? new Name() : new Name(vcfRow.Value, vcfRow.Info, version);
    }

    /// <summary> The data provided by the <see cref="NameProperty" />.
    /// </summary>
    public new Name Value
    {
        get;
    }


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;


    /// <summary>Formats the data encapsulated by the instance into a human-readable
    /// form.</summary>
    /// <returns>The data encapsulated by the instance in human-readable form
    /// or <c>null</c> if the instance <see cref="IsEmpty"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? ToDisplayName() => Value.ToDisplayName();


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = ValueEncoding.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null

        StringBuilder builder = serializer.Builder;
        int valueStartIndex = builder.Length;


        Value.AppendVCardString(serializer);

        if (Parameters.Encoding == ValueEncoding.QuotedPrintable)
        {
            string toEncode = builder.ToString(valueStartIndex, builder.Length - valueStartIndex);
            builder.Length = valueStartIndex;

            _ = builder.Append(QuotedPrintable.Encode(toEncode, valueStartIndex));
        }
    }

    IEnumerator<NameProperty> IEnumerable<NameProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<NameProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new NameProperty(this);
}
