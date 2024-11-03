using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>UID</c>, which stores a unique identifier
/// for the vCard subject.</summary>
/// <seealso cref="VCard.ID"/>
/// <seealso cref="RelationProperty"/>
public sealed class IDProperty : VCardProperty, IEquatable<IDProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="IDProperty"/> instance
    /// to clone.</param>
    private IDProperty(IDProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="IDProperty" /> object with a
    /// new <see cref="Guid" />. </summary>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public IDProperty(string? group = null)
        : base(new ParameterSection(), group) => Value = ContactID.Create();

    /// <summary> Initializes a new <see cref="IDProperty" /> object with a 
    /// specified <see cref="Guid"/>. </summary>
    /// <param name="uuid">A <see cref="Guid" /> value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public IDProperty(Guid uuid, string? group = null)
        : base(new ParameterSection(), group) => Value = ContactID.Create(uuid);

    /// <summary> Initializes a new <see cref="IDProperty" /> object with a 
    /// specified absolute <see cref="Uri"/>. </summary>
    /// <param name="uri">An absolute <see cref="Uri" />.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is not an absolute <see cref="Uri"/>.</exception>
    public IDProperty(Uri uri, string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = ContactID.Create(uri);
        Parameters.DataType = Data.Uri;
    }

    /// <summary> Initializes a new <see cref="IDProperty" /> object with
    /// specified free-form text. </summary>
    /// <param name="text">Free-form text that can be used as identifier.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> is an empty <see cref="string"/> 
    /// or consists only of white space.</exception>
    public IDProperty(string text, string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = ContactID.Create(text);
        Parameters.DataType = Data.Text;
    }

    internal IDProperty(VcfRow vcfRow, VCdVersion version) : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (Parameters.DataType == Data.Text)
        {
            string val = vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? vcfRow.Value.Span.UnMaskAndDecodeValue(vcfRow.Parameters.CharSet)
                : vcfRow.Value.Span.UnMaskValue(version);

            Value = string.IsNullOrWhiteSpace(val) ? ContactID.Empty : ContactID.Create(val);
            return;
        }

        if (UuidConverter.TryAsGuid(vcfRow.Value.Span, out Guid uuid))
        {
            Value = ContactID.Create(uuid);
        }

        string uriString = vcfRow.Value.Span.UnMaskValue(version);

        if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uri))
        {
            Value = ContactID.Create(uri);
        }

        Value = string.IsNullOrWhiteSpace(uriString) ? ContactID.Empty : ContactID.Create(uriString);
    }

    /// <summary> The <see cref="ContactID"/> provided by the <see cref="IDProperty" />.
    /// </summary>
    public new ContactID Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new IDProperty(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as IDProperty);

    /// <inheritdoc />
    public bool Equals(IDProperty? other)
        => (other is not null) && Value.Equals(other.Value);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Value);

    /// <summary>
    /// Overloads the equality operator for <see cref="IDProperty"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the <see cref="Value"/> of <paramref name="left"/> and 
    /// <paramref name="right"/> is equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(IDProperty? left, IDProperty? right)
        => object.ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="IDProperty"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the <see cref="Value"/> of <paramref name="left"/> and 
    /// <paramref name="right"/> is not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(IDProperty? left, IDProperty? right)
        => !(left == right);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        if (IsEmpty || Parameters.DataType == Data.Uri)
        {
            // Valid URIs consist of ASCII characters and don't include
            // line breaks.
            return;
        }

        if (serializer.Version == VCdVersion.V2_1 && Value.IsString && Value.String.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = Enc.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        if (Value.IsGuid)
        {
            _ = serializer.Builder.AppendUuid(this.Value.Guid.Value, serializer.Version);
        }
        else if (Value.IsUri)
        {
            // URIs are not masked according to the "Verifier notes" in
            // https://www.rfc-editor.org/errata/eid3845
            // It says that "the ABNF does not support escaping for URIs."
            _ = serializer.Builder.Append(Value.Uri.AbsoluteUri);
        }
        else
        {
            StringBuilder builder = serializer.Builder;

            _ = serializer.Version == VCdVersion.V2_1
                ? this.Parameters.Encoding == Enc.QuotedPrintable
                    ? builder.AppendQuotedPrintable(Value.String.AsSpan(), builder.Length)
                    : builder.Append(Value)
                : builder.AppendValueMasked(Value.String, serializer.Version);
        }
    }
}
