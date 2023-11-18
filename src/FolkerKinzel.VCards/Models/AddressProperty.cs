using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the data of the vCard property <c>ADR</c> that contains 
/// information about the postal address.</summary>
/// <remarks>See <see cref="VCard.Addresses"/>.</remarks>
/// <seealso cref="VCard.Addresses"/>
public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="AddressProperty" /> object to clone.</param>
    private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

    /// <summary> Initializes a new <see cref="AddressProperty" /> object.</summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="AttachLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressProperty(IEnumerable<string?>? street,
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country = null,
                           bool autoLabel = true,
                           string? group = null)
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete

    /// <summary> Initializes a new <see cref="AddressProperty" /> object. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this parameter!)</param>
    /// <param name="extendedAddress">The extended address (e.g. apartment or suite
    /// number). (Don't use this parameter!)</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="AttachLabel" />
    [Obsolete("Don't use this constructor.", false)]
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
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
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
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete

    /// <summary> Initializes a new <see cref="AddressProperty" /> object. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this parameter!)</param>
    /// <param name="extendedAddress">The extended address (e.g. apartment or suite
    /// number). (Don't use this parameter!)</param>
    /// <param name="autoLabel">Pass <c>false</c> to prevent a mailing label from being 
    /// automatically added to the <see cref="ParameterSection.Label" /> parameter.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="AttachLabel" />
    [Obsolete("Don't use this constructor.", false)]
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

    internal AddressProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.DecodeQuotedPrintable();

        Value = string.IsNullOrWhiteSpace(vcfRow.Value) ? new Address()
                                                        : new Address(vcfRow.Value, vcfRow.Info, version);
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
    /// <para>
    /// The behavior of the property has changed since version 5.0.0-beta.2:
    /// </para>
    /// <para>
    /// If only the <see cref="ParameterSection.Label" /> parameter is not equal
    /// to <c>null</c>, the property returns <c>false</c>.
    /// </para>
    /// </note>
    /// </remarks>
    public override bool IsEmpty => Value.IsEmpty && Parameters.Label is null;

    /// <summary>Converts the data encapsulated in the instance to formatted text
    /// for a mailing label.</summary>
    /// <returns>The data encapsulated in the instance, converted to formatted text
    /// for a mailing label.</returns>
    /// <seealso cref="AttachLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLabel() => Value.ToLabel();

    /// <summary> Assigns an automatical generated mailing label to the 
    /// <see cref="ParameterSection.Label" /> property. Any previously stored data in the 
    /// <see cref="ParameterSection.Label" /> property will be overwritten.</summary>
    /// <seealso cref="ToLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AttachLabel() => Parameters.Label = ToLabel();

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

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = Enc.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null


        StringBuilder builder = serializer.Builder;
        int startIndex = builder.Length;

        Value.AppendVCardString(serializer);

        if (Parameters.Encoding == Enc.QuotedPrintable)
        {
            string toEncode = builder.ToString(startIndex, builder.Length - startIndex);
            builder.Length = startIndex;

            _ = builder.Append(QuotedPrintable.Encode(toEncode, startIndex));
        }
    }
}
