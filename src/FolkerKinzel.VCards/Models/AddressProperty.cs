using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Encapsulates the data of a vCard property that contains information
    /// about the postal address.</summary>
public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>
{
    /// <summary />
    /// <param name="prop">The <see cref="AddressProperty" /> object to clone.</param>
    private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

    /// <summary> Initialisiert ein neues <see cref="AddressProperty" />-Objekt. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass
    /// dem Parameter <see cref="ParameterSection.Label" /> automatisch ein Adressetikett
    /// hinzugefügt wird.</param>
    /// <seealso cref="AppendLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressProperty(IEnumerable<string?>? street,
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country = null,
                           string? propertyGroup = null,
                           bool appendLabel = true)
#pragma warning disable CS0618 // Typ oder Element ist veraltet
        : this(street: street,
               locality: locality,
               region: region,
               postalCode: postalCode,
               country: country,
               postOfficeBox: null,
               extendedAddress: null,
               propertyGroup: propertyGroup,
               appendLabel: appendLabel)
    { }
#pragma warning restore CS0618 // Typ oder Element ist veraltet




    /// <summary> Initialisiert ein neues <see cref="AddressProperty" />-Objekt. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this property!)</param>
    /// <param name="extendedAddress">The extended address (e.g. apartment or suite
    /// number). (Don't use this parameter!)</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass
    /// dem Parameter <see cref="ParameterSection.Label" /> automatisch ein Adressetikett
    /// hinzugefügt wird.</param>
    /// <seealso cref="AppendLabel" />
    [Obsolete("Don't use this constructor.", false)]
    public AddressProperty(IEnumerable<string?>? street,
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country,
                           IEnumerable<string?>? postOfficeBox,
                           IEnumerable<string?>? extendedAddress,
                           string? propertyGroup = null,
                           bool appendLabel = true) : base(new ParameterSection(), propertyGroup)
    {
        Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                            locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                            region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                            postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                            country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                            postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                            extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));

        if(appendLabel)
        {
            AppendLabel();
        }
    }


    /// <summary> Initialisiert ein neues <see cref="AddressProperty" />-Objekt. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass
    /// dem Parameter <see cref="ParameterSection.Label" /> automatisch ein Adressetikett
    /// hinzugefügt wird.</param>
    /// <seealso cref="AppendLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressProperty(
        string? street,
        string? locality,
        string? region,
        string? postalCode,
        string? country = null,
        string? propertyGroup = null,
        bool appendLabel = true)
#pragma warning disable CS0618 // Typ oder Element ist veraltet
        : this(street: street,
              locality: locality,
              region: region,
              postalCode: postalCode,
              country: country,
              postOfficeBox: null,
              extendedAddress: null,
              propertyGroup: propertyGroup,
              appendLabel: appendLabel)
    { }
#pragma warning restore CS0618 // Typ oder Element ist veraltet


    /// <summary> Initialisiert ein neues <see cref="AddressProperty" />-Objekt. </summary>
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this property!)</param>
    /// <param name="extendedAddress">The extended address (e.g. apartment or suite
    /// number). (Don't use this parameter!)</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass
    /// dem Parameter <see cref="ParameterSection.Label" /> automatisch ein Adressetikett
    /// hinzugefügt wird.</param>
    /// <seealso cref="AppendLabel" />
    [Obsolete("Don't use this constructor.", false)]
    public AddressProperty(
        string? street,
        string? locality,
        string? region,
        string? postalCode,
        string? country,
        string? postOfficeBox,
        string? extendedAddress,
        string? propertyGroup = null,
        bool appendLabel = true)
        : base(new ParameterSection(), propertyGroup)
    {
        Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                            locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                            region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                            postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                            country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                            postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                            extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));
        
        if (appendLabel)
        {
            AppendLabel();
        }
    }

    /// <summary>Converts the data encapsulated in the instance into formatted text
    /// for a mailing label.</summary>
    /// <returns>The data encapsulated in the instance, converted to formatted text
    /// for a mailing label.</returns>
    /// <seealso cref="AppendLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
    public string ToLabel() => Value.ToLabel();


    /// <summary> Fügt der Eigenschaft <see cref="ParameterSection.Label" /> ein Adressetikett
    /// hinzu, das aus den Daten der Instanz generiert wird. Evtl. vorher in der Eigenschaft
    /// <see cref="ParameterSection.Label" /> gespeicherte Daten werden dabei überschrieben.
    /// </summary>
    /// <seealso cref="ToLabel" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLabel() => Parameters.Label = ToLabel();


    internal AddressProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.DecodeQuotedPrintable();

        Value = string.IsNullOrWhiteSpace(vcfRow.Value) ? new Address()
                                                        : new Address(vcfRow.Value, vcfRow.Info, version);
    }


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
        int startIndex = builder.Length;

        Value.AppendVCardString(serializer);

        if (Parameters.Encoding == ValueEncoding.QuotedPrintable)
        {
            string toEncode = builder.ToString(startIndex, builder.Length - startIndex);
            builder.Length = startIndex;

            _ = builder.Append(QuotedPrintable.Encode(toEncode, startIndex));
        }
    }


    /// <summary> Die von der <see cref="AddressProperty" /> zur Verfügung gestellten
    /// Daten. </summary>
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
    /// Das Verhalten der Eigenschaft hat sich seit Version 5.0.0-beta.2 geändert:
    /// </para>
    /// <para>
    /// Wenn lediglich der Parameter <see cref="ParameterSection.Label" /> ungleich
    /// <c>null</c> ist, wird <c>false</c> zurückgegeben.
    /// </para>
    /// </note>
    /// </remarks>
    public override bool IsEmpty => Value.IsEmpty && Parameters.Label is null;


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    IEnumerator<AddressProperty> IEnumerable<AddressProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<AddressProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new AddressProperty(this);
}
