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
/// You can use the <see cref="NameProperty.ToDisplayName" />  method to generate 
/// formatted representations from the structured name representations that are readable 
/// by the users of the application.
/// </note>
/// </remarks>
/// <seealso cref="VCard.NameViews"/>
/// <seealso cref="Name"/>
/// <seealso cref="NameProperty.ToDisplayName"/>
public sealed class NameProperty : VCardProperty, IEnumerable<NameProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="NameProperty"/> instance to clone.</param>
    private NameProperty(NameProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="NameProperty" /> object. </summary>
    /// <param name="lastName">Family Name(s) (also known as surname(s))</param>
    /// <param name="firstName">Given Name(s) (first name(s))</param>
    /// <param name="middleName">Additional Name(s)</param>
    /// <param name="prefix">Honorific Prefix(es)</param>
    /// <param name="suffix">Honorific Suffix(es)</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="ToDisplayName" />
    public NameProperty(
        IEnumerable<string?>? lastName = null,
        IEnumerable<string?>? firstName = null,
        IEnumerable<string?>? middleName = null,
        IEnumerable<string?>? prefix = null,
        IEnumerable<string?>? suffix = null,
        string? group = null) : base(new ParameterSection(), group)
    {
        Value = new Name(lastName: ReadOnlyCollectionConverter.ToReadOnlyCollection(lastName),
                         firstName: ReadOnlyCollectionConverter.ToReadOnlyCollection(firstName),
                         middleName: ReadOnlyCollectionConverter.ToReadOnlyCollection(middleName),
                         prefix: ReadOnlyCollectionConverter.ToReadOnlyCollection(prefix),
                         suffix: ReadOnlyCollectionConverter.ToReadOnlyCollection(suffix));
    }

    /// <summary>  Initializes a new <see cref="NameProperty" /> object. </summary>
    /// <param name="lastName">Family Name (also known as surname)</param>
    /// <param name="firstName">Given Name (first name)</param>
    /// <param name="middleName">Additional Name</param>
    /// <param name="prefix">Honorific Prefix</param>
    /// <param name="suffix">Honorific Suffix</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="ToDisplayName" />
    public NameProperty(
        string? lastName,
        string? firstName = null,
        string? middleName = null,
        string? prefix = null,
        string? suffix = null,
        string? group = null) : base(new ParameterSection(), group)
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
    public override bool IsEmpty => Value.IsEmpty;

    /// <summary>Formats the data encapsulated by the instance into a human-readable
    /// form.</summary>
    /// <returns>The data encapsulated by the instance in human-readable form
    /// or <c>null</c> if the instance <see cref="IsEmpty"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? ToDisplayName() => Value.ToDisplayName();

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
        int valueStartIndex = builder.Length;

        Value.AppendVCardString(serializer);

        if (Parameters.Encoding == Enc.QuotedPrintable)
        {
            string toEncode = builder.ToString(valueStartIndex, builder.Length - valueStartIndex);
            builder.Length = valueStartIndex;

            _ = builder.Append(QuotedPrintable.Encode(toEncode, valueStartIndex));
        }
    }
}
