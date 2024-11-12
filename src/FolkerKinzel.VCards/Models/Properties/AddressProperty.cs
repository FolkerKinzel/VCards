using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Formatters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Encapsulates the data of the vCard property <c>ADR</c> that contains 
/// information about the postal address.</summary>
/// <remarks>See <see cref="VCard.Addresses"/>.</remarks>
/// <seealso cref="VCard.Addresses"/>
public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>, ICompoundProperty
{
    #region Remove this code with Version 8.0.0

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressFormatter instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public void AttachLabel() => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressFormatter instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? ToLabel() => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressProperty(Address, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressProperty(IEnumerable<string?>? street,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country = null,
                           bool autoLabel = true,
                           string? group = null) : base(new ParameterSection(), group) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressProperty(Address, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressProperty(IEnumerable<string?>? street,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                           IEnumerable<string?>? locality,
                           IEnumerable<string?>? region,
                           IEnumerable<string?>? postalCode,
                           IEnumerable<string?>? country,
                           IEnumerable<string?>? postOfficeBox,
                           IEnumerable<string?>? extendedAddress,
                           bool autoLabel = true,
                           string? group = null) : base(new ParameterSection(), group) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressProperty(Address, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressProperty(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        string? street,
        string? locality,
        string? region,
        string? postalCode,
        string? country = null,
        bool autoLabel = true,
        string? group = null) : base(new ParameterSection(), group) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressProperty(Address, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressProperty(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        string? street,
        string? locality,
        string? region,
        string? postalCode,
        string? country,
        string? postOfficeBox,
        string? extendedAddress,
        bool autoLabel = true,
        string? group = null) : base(new ParameterSection(), group) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use AddressProperty(Address, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressProperty(AddressBuilder builder, string? group = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        : base(new ParameterSection(), group) => throw new NotImplementedException();

    #endregion

    /// <summary>
    /// Initializes a new <see cref="AddressProperty"/> instance with a 
    /// specified <see cref="Address"/>.
    /// </summary>
    /// 
    /// <param name="address">The <see cref="Address"/> instance used as <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="address"/> is <c>null</c>.</exception>
    public AddressProperty(Address address, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(address, nameof(address));
        Value = address;
    }

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="AddressProperty" /> object to clone.</param>
    private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

    internal AddressProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        ReadOnlyMemory<char> val = vcfRow.Value;

        if (Parameters.Encoding == Enc.QuotedPrintable)
        {
            val = QuotedPrintable.Decode(
                    val.Span,
                    TextEncodingConverter.GetEncoding(Parameters.CharSet)).AsMemory(); // null-check not needed
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
            Parameters.Encoding = Enc.QuotedPrintable;
            Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal override void AppendValue(VcfSerializer serializer)
        => Value.AppendVcfString(serializer);
}
