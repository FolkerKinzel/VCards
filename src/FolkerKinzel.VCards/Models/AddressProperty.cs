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
    /// <param name="region">Bundesland</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass dem Parameter <see cref="ParameterSection.Label"/> 
    /// automatisch ein aus den gekapselten Daten erzeugtes Adressetikett hinzugefügt wird.</param>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public AddressProperty(IEnumerable<string?>? street = null,
                           IEnumerable<string?>? locality = null,
                           IEnumerable<string?>? region = null,
                           IEnumerable<string?>? postalCode = null,
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




    /// <summary>
    /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
    /// </summary>
    /// <param name="street">Straße</param>
    /// <param name="locality">Ort</param>
    /// <param name="region">Bundesland</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass dem Parameter <see cref="ParameterSection.Label"/> 
    /// automatisch ein aus den gekapselten Daten erzeugtes Adressetikett hinzugefügt wird.</param>
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


    /// <summary>
    /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
    /// </summary>
    /// <param name="street">Straße</param>
    /// <param name="locality">Ort</param>
    /// <param name="region">Bundesland</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass dem Parameter <see cref="ParameterSection.Label"/> 
    /// automatisch ein aus den gekapselten Daten erzeugtes Adressetikett hinzugefügt wird.</param>
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


    /// <summary>
    /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
    /// </summary>
    /// <param name="street">Straße</param>
    /// <param name="locality">Ort</param>
    /// <param name="region">Bundesland</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <param name="appendLabel">Geben Sie <c>false</c> an, um zu verhindern, dass dem Parameter <see cref="ParameterSection.Label"/> 
    /// automatisch ein aus den gekapselten Daten erzeugtes Adressetikett hinzugefügt wird.</param>
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

    /// <summary>
    /// Wandelt die in der Instanz gekapselten Daten in formatierten Text für ein Adressetikett um.
    /// </summary>
    /// <returns>Die in der Instanz gekapselten Daten, umgewandelt in formatierten Text für ein Adressetikett.</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
#endif
    public string ToLabel() => Value.ToLabel();


    /// <summary>
    /// Fügt der Eigenschaft <see cref="ParameterSection.Label"/> ein Adressetikett hinzu, das automatisch aus den
    /// Daten der Instanz generíert wird. Evtl. vorher in der Eigenschaft <see cref="ParameterSection.Label"/> gespeicherte
    /// Daten werden dabei überschrieben.
    /// </summary>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void AppendLabel() => Parameters.Label = ToLabel();


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
