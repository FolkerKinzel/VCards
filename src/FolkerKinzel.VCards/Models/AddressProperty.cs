using System.Collections;
using System.Text;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Kapselt die Daten einer vCard-Property, die Informationen über die Postanschrift enthält.
/// </summary>
public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="AddressProperty"/> object to clone.</param>
    private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
    /// </summary>
    /// <param name="street">Straße</param>
    /// <param name="locality">Ort</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="region">Bundesland</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <remarks>
    /// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
    /// eine formatierte Darstellung der Adresse zuzuweisen.
    /// </remarks>
    public AddressProperty(IEnumerable<string?>? street = null,
                           IEnumerable<string?>? locality = null,
                           IEnumerable<string?>? postalCode = null,
                           IEnumerable<string?>? region = null,
                           IEnumerable<string?>? country = null,
                           IEnumerable<string?>? postOfficeBox = null,
                           IEnumerable<string?>? extendedAddress = null,
                           string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
    {
        Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                            locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                            postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                            region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                            country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                            postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                            extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));
    }


    /// <summary>
    /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
    /// </summary>
    /// <param name="street">Straße</param>
    /// <param name="locality">Ort</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="region">Bundesland</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <remarks>
    /// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
    /// eine formatierte Darstellung der Adresse zuzuweisen.
    /// </remarks>
    public AddressProperty(
        string? street,
        string? locality,
        string? postalCode,
        string? region = null,
        string? country = null,
        string? postOfficeBox = null,
        string? extendedAddress = null,
        string? propertyGroup = null)
        : base(new ParameterSection(), propertyGroup)
    {
        Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                            locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                            postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                            region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                            country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                            postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                            extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));
    }

    /// <summary>
    /// Gibt an, ob die Instanz auf eine Postanschrift in den USA verweist.
    /// </summary>
    /// <returns><c>true</c>, wenn die Instanz auf eine Adresse in den USA verweist, andernfalls <c>false</c>.</returns>
    /// <remarks>
    /// Die Methode untersucht die Eigenschaft <see cref="Country"/>. Wenn diese Eigenschaft leer ist, gibt sie <c>false</c> zurück.
    /// </remarks>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
#endif
    public bool IsUSAddress() => Value.IsUSAddress();


    internal AddressProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.DecodeQuotedPrintable();

        if (vcfRow.Value is null)
        {
            Value = new Address();
        }
        else
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(vcfRow.Value));
            Value = new Address(vcfRow.Value, vcfRow.Info, version);
        }
    }


    [InternalProtected]
    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();

        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = VCdEncoding.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }


    [InternalProtected]
    internal override void AppendValue(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();

        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null


        StringBuilder builder = serializer.Builder;
        int startIndex = builder.Length;

        Value.AppendVCardString(serializer);

        if (Parameters.Encoding == VCdEncoding.QuotedPrintable)
        {
            string toEncode = builder.ToString(startIndex, builder.Length - startIndex);
            builder.Length = startIndex;

            _ = builder.Append(QuotedPrintableConverter.Encode(toEncode, startIndex));
        }
    }


    /// <summary>
    /// Die von der <see cref="AddressProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new Address Value
    {
        get;
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <remarks>
    /// <note type="important">
    /// <para>
    /// Das Verhalten der Eigenschaft hat sich seit Version 5.0.0-beta.2 geändert:
    /// </para>
    /// <para>
    /// Wenn lediglich der Parameter <see cref="ParameterSection.Label"/>
    /// ungleich <c>null</c> ist, wird <c>false</c> zurückgegeben.
    /// </para>
    /// </note>
    /// </remarks>
    public override bool IsEmpty => Value.IsEmpty && Parameters.Label is null;


    /// <inheritdoc/>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    protected override object? GetVCardPropertyValue() => Value;


    IEnumerator<AddressProperty> IEnumerable<AddressProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<AddressProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    public override object Clone() => new AddressProperty(this);
}
