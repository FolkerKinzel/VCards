using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Formatters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the data of the vCard property <c>ADR</c> that contains 
/// information about the postal address.</summary>
/// <remarks>See <see cref="VCard.Addresses"/>.</remarks>
/// <seealso cref="VCard.Addresses"/>
public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>, ICompoundProperty
{
    #region Remove this code with Version 8.0.0

    /// <summary> Assigns an automatical generated mailing label to the 
    /// <see cref="ParameterSection.Label" /> property. Any previously stored data in the 
    /// <see cref="ParameterSection.Label" /> property will be overwritten.</summary>
    /// <seealso cref="ToLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AttachLabel() => Parameters.Label = ToLabel();

    /// <summary>Converts the data encapsulated in the instance to formatted text
    /// for a mailing label.</summary>
    /// <returns>The data encapsulated in the instance, converted to formatted text
    /// for a mailing label.</returns>
    /// <seealso cref="AttachLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? ToLabel() => AddressFormatter.Default.ToLabel(this);

    /// <summary> Initializes a new <see cref="AddressProperty" /> object.</summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g., city).</param>
    /// <param name="region">The region (e.g., state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>
    /// <note type="tip">
    /// It's recommended to use the constructor overload that takes an <see cref="AddressBuilder"/>
    /// as argument.
    /// </note>
    /// </remarks>
    /// <seealso cref="AttachLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressProperty(IEnumerable<string?>? street,
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country = null,
                           bool autoLabel = true,
                           string? group = null)
        : this(street: street,
               locality: locality,
               region: region,
               postalCode: postalCode,
               country: country,
               postOfficeBox: null,
               extendedAddress: null,
               autoLabel: autoLabel,
               group: group)
    { }

    /// <summary> Initializes a new <see cref="AddressProperty" /> object. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this parameter!)</param>
    /// <param name="extendedAddress">The extended address (e.g., apartment or suite
    /// number). (Don't use this parameter!)</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>
    /// <note type="tip">
    /// It's recommended to use the constructor overload that takes an <see cref="AddressBuilder"/>
    /// as argument.
    /// </note>
    /// </remarks>
    /// <seealso cref="AttachLabel" />
    public AddressProperty(IEnumerable<string?>? street,
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country,
                           IEnumerable<string?>? postOfficeBox,
                           IEnumerable<string?>? extendedAddress,
                           bool autoLabel = true,
                           string? group = null) : base(new ParameterSection(), group)
    {
        Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                            locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                            region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                            postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                            country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                            postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                            extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));

        if (autoLabel)
        {
            AttachLabel();
        }
    }

    /// <summary>Initializes a new <see cref="AddressProperty" /> object. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g., city).</param>
    /// <param name="region">The region (e.g., state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>
    /// <note type="tip">
    /// It's recommended to use the constructor overload that takes an <see cref="AddressBuilder"/>
    /// as argument.
    /// </note>
    /// </remarks>
    /// <seealso cref="AttachLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressProperty(
        string? street,
        string? locality,
        string? region,
        string? postalCode,
        string? country = null,
        bool autoLabel = true,
        string? group = null)
        : this(street: street,
              locality: locality,
              region: region,
              postalCode: postalCode,
              country: country,
              postOfficeBox: null,
              extendedAddress: null,
              autoLabel: autoLabel,
              group: group)
    { }

    /// <summary> Initializes a new <see cref="AddressProperty" /> object. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g., city).</param>
    /// <param name="region">The region (e.g., state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this parameter!)</param>
    /// <param name="extendedAddress">The extended address (e.g., apartment or suite
    /// number). (Don't use this parameter!)</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>
    /// <note type="tip">
    /// It's recommended to use the constructor overload that takes an <see cref="AddressBuilder"/>
    /// as argument.
    /// </note>
    /// </remarks>
    /// <seealso cref="AttachLabel" />
    public AddressProperty(
        string? street,
        string? locality,
        string? region,
        string? postalCode,
        string? country,
        string? postOfficeBox,
        string? extendedAddress,
        bool autoLabel = true,
        string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                            locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                            region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                            postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                            country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                            postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                            extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));

        if (autoLabel)
        {
            AttachLabel();
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new <see cref="AddressProperty"/> instance with the content of a 
    /// specified <see cref="AddressBuilder"/>.
    /// </summary>
    /// <remarks>
    /// <note type="caution">
    /// The constructor does not <see cref="AddressBuilder.Clear"/> the <see cref="AddressBuilder"/>.
    /// </note>
    /// </remarks>
    /// <param name="builder">The <see cref="AddressBuilder"/> whose content is used.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="ToLabel" />
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    public AddressProperty(AddressBuilder builder, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        Value = new Address(builder);
    }

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="AddressProperty" /> object to clone.</param>
    private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

    internal AddressProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        ReadOnlyMemory<char> val = vcfRow.Value;

        if (this.Parameters.Encoding == Enc.QuotedPrintable)
        {
            val = QuotedPrintable.Decode(
                    val.Span,
                    TextEncodingConverter.GetEncoding(this.Parameters.CharSet)).AsMemory(); // null-check not needed
        }

        Value = val.Span.IsWhiteSpace()
            ? new Address()
            : new Address(in val, version);
    }

    /// <summary> The data provided by the <see cref="AddressProperty" />.</summary>
    public new Address Value
    {
        get;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <remarks>
    /// <note type="important">
    /// If only the <see cref="ParameterSection.Label" /> parameter or the <see cref="ParameterSection.GeoPosition"/> or
    /// the <see cref="ParameterSection.TimeZone"/> is not equal
    /// to <c>null</c>, <see cref="IsEmpty"/> returns <c>false</c>.
    /// </note>
    /// </remarks>
    public override bool IsEmpty => Value.IsEmpty
                                 && Parameters.Label is null
                                 && Parameters.GeoPosition is null
                                 && Parameters.TimeZone is null;

    /// <inheritdoc />
    public override object Clone() => new AddressProperty(this);

    /// <inheritdoc/>
    IEnumerator<AddressProperty> IEnumerable<AddressProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<AddressProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    int ICompoundProperty.Count
        => ((IReadOnlyList<IReadOnlyList<string>>)Value).Count;

    /// <inheritdoc/>
    IReadOnlyList<string> ICompoundProperty.this[int index]
        => ((IReadOnlyList<IReadOnlyList<string>>)Value)[index];

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer is not null);
        Debug.Assert(Value is not null); // value ist nie null

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = Enc.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal override void AppendValue(VcfSerializer serializer)
        => Value.AppendVcfString(serializer);
}
