using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>N</c>, which stores the name of the
/// vCard subject.</summary>
/// <seealso cref="VCard.NameViews"/>
/// <seealso cref="Name"/>
public sealed class NameProperty : VCardProperty, IEnumerable<NameProperty>, ICompoundProperty
{
    #region Remove this code with version 8.0.1

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use NameFormatter instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? ToDisplayName() => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use NameProperty(Name, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public NameProperty(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        IEnumerable<string?>? familyNames = null,
        IEnumerable<string?>? givenNames = null,
        IEnumerable<string?>? additionalNames = null,
        IEnumerable<string?>? prefixes = null,
        IEnumerable<string?>? suffixes = null,
        string? group = null) : base(new ParameterSection(), group) => throw new NotImplementedException();


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use NameProperty(Name, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public NameProperty(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        string? familyName,
        string? givenName = null,
        string? additionalName = null,
        string? prefix = null,
        string? suffix = null,
        string? group = null) : base(new ParameterSection(), group) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use NameProperty(Name, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public NameProperty(NameBuilder builder, string? group = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        : base(new ParameterSection(), group) => throw new NotImplementedException();

    #endregion

    /// <summary>
    /// Initializes a new <see cref="NameProperty"/> instance with a 
    /// specified <see cref="Name"/>.
    /// </summary>
    /// <param name="name">The <see cref="Name"/> instance used as <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
    public NameProperty(Name name, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(name, nameof(name));
        Value = name;
    }

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="NameProperty"/> instance to clone.</param>
    private NameProperty(NameProperty prop) : base(prop)
        => Value = prop.Value;

    internal NameProperty(VcfRow vcfRow, VCdVersion version)
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
            ? new Name()
            : new Name(in val, version);
    }

    /// <summary> The data provided by the <see cref="NameProperty" />.
    /// </summary>
    public new Name Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc/>
    int ICompoundProperty.Count => ((IReadOnlyList<IReadOnlyList<string>>)Value).Count;

    /// <inheritdoc/>
    IReadOnlyList<string> ICompoundProperty.this[int index]
        => ((IReadOnlyList<IReadOnlyList<string>>)Value)[index];

    /// <inheritdoc />
    public override object Clone() => new NameProperty(this);

    /// <inheritdoc />
    IEnumerator<NameProperty> IEnumerable<NameProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<NameProperty>)this).GetEnumerator();

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
