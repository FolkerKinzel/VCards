using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents a vCard property that is not defined by the official 
/// standards.</summary>
/// <remarks>
/// <note type="important">
/// <para>
/// To write <see cref="NonStandardProperty" /> objects into a vCard, the flag 
/// <see cref="VcfOpts.WriteNonStandardProperties" /> must be set. 
/// </para>
/// <para>
/// Please note that when using the class, yourself is responsible for the 
/// standard-compliant masking, unmasking, encoding and decoding of the data.
/// </para>
/// </note>
/// </remarks>
/// <seealso cref="VCard.NonStandards"/>
public sealed class NonStandardProperty : VCardProperty, IEnumerable<NonStandardProperty>
{
    #region Remove with 8.0.1

    [Obsolete("Use Key instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string XName => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="NonStandardProperty"/> instance to clone.</param>
    private NonStandardProperty(NonStandardProperty prop) : base(prop)
    {
        Key = prop.Key;
        Value = prop.Value;
    }

    /// <summary>Initializes a new <see cref="NonStandardProperty" /> object.</summary>
    /// <param name="xName">The key ("name") of the non-standard vCard property
    /// (format: <c>X-NAME</c>).</param>
    /// <param name="value">The value of the vCard property: any data encoded as <see
    /// cref="string" />, or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="xName" /> is
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="xName" /> is not
    /// a valid X-NAME.</exception>
    public NonStandardProperty(string xName, string? value, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(xName, nameof(xName));


        /* Unmerged change from project 'FolkerKinzel.VCards (netstandard2.0)'
        Before:
                Key = IsXName(xName) ? xName
                                       : throw new ArgumentException(Res.NoXName, nameof(xName));
        After:
                Key = XNameValidator.IsXName(xName) ? xName
                                       : throw new ArgumentException(Res.NoXName, nameof(xName));
        */

        /* Unmerged change from project 'FolkerKinzel.VCards (net8.0)'
        Before:
                Key = Properties.XNameValidator.IsXName(xName) ? xName
                                       : throw new ArgumentException(Res.NoXName, nameof(xName));
        After:
                Key = XNameValidator.IsXName(xName) ? xName
                                       : throw new ArgumentException(Res.NoXName, nameof(xName));
        */
        Key = Intls.XNameValidator.IsXName(xName) ? xName
                               : throw new ArgumentException(Res.NoXName, nameof(xName));
        Value = value ?? "";
    }

    internal NonStandardProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Key = vcfRow.Key;
        Value = vcfRow.Value.ToString();
    }

    /// <summary>The key ("name") of the vCard property.</summary>
    public string Key { get; }

    /// <summary>The data provided by the <see cref="NonStandardProperty" /> (raw <see
    /// cref="string" /> data).</summary>
    public new string Value
    {
        get;
    }

    /// <inheritdoc/>
    public override bool IsEmpty => Value.Length == 0;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        _ = sb.Append($"{nameof(Key)}: ").AppendLine(Key);
        _ = sb.Append("Value: ").Append(Value);

        return sb.ToString();
    }

    /// <inheritdoc />
    public override object Clone() => new NonStandardProperty(this);

    /// <inheritdoc />
    IEnumerator<NonStandardProperty> IEnumerable<NonStandardProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<NonStandardProperty>)this).GetEnumerator();

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        // MUST not call the base class implementation!
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        _ = serializer.Builder.Append(Value);
    }

    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    internal bool IsXNameProperty() => XNameValidator.IsXName(Key);
}
