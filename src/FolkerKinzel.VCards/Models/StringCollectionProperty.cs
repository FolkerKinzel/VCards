using System.Collections;
using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents vCard properties that store a collection of <see cref="string" />s .</summary>
/// <seealso cref="VCard.NickNames"/>
/// <seealso cref="VCard.Categories"/>
public sealed class StringCollectionProperty : VCardProperty, IEnumerable<StringCollectionProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="StringCollectionProperty"/>
    /// instance to clone</param>
    private StringCollectionProperty(StringCollectionProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>Initializes a new <see cref="StringCollectionProperty" /> object.</summary>
    /// <param name="value">A collection of <see cref="string" />s or <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public StringCollectionProperty(IEnumerable<string?>? value, string? propertyGroup = null) 
        : base(new ParameterSection(), propertyGroup)
    {
        this.Value = ReadOnlyCollectionConverter.ToReadOnlyCollection(value);

        if (Value.Count == 0)
        {
            Value = null;
        }
    }

    /// <summary>Initializes a new <see cref="StringCollectionProperty" /> object.</summary>
    /// <param name="value">A <see cref="string" /> or <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public StringCollectionProperty(string? value, string? propertyGroup = null)
        : base(new ParameterSection(), propertyGroup)
    {
        this.Value = ReadOnlyCollectionConverter.ToReadOnlyCollection(value);

        if (Value.Count == 0)
        {
            Value = null;
        }
    }

    internal StringCollectionProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.DecodeQuotedPrintable();

        if (vcfRow.Value.Length == 0)
        {
            return;
        }

        var list = new List<string>();

        ValueSplitter? commaSplitter = vcfRow.Info.CommaSplitter;

        commaSplitter.ValueString = vcfRow.Value;

        foreach (string s in commaSplitter)
        {
            list.Add(s.UnMask(vcfRow.Info.Builder, version));
        }

        if (list.Count != 0)
        {
            this.Value = new ReadOnlyCollection<string>(list);
        }
    }

    /// <summary>The data provided by the <see cref="StringCollectionProperty" />.</summary>
    public new ReadOnlyCollection<string>? Value
    {
        get;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        string s = "";

        if (Value is null)
        {
            return s;
        }

        Debug.Assert(Value.Count != 0);

        for (int i = 0; i < Value.Count - 1; i++)
        {
            s += Value[i];
            s += ", ";
        }

        s += Value[Value.Count - 1];

        return s;
    }

    /// <inheritdoc />
    public override object Clone() => new StringCollectionProperty(this);

    /// <inheritdoc />
    IEnumerator<StringCollectionProperty> IEnumerable<StringCollectionProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<StringCollectionProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        if (Value is null)
        {
            return;
        }

        Debug.Assert(Value.Count != 0);

        StringBuilder worker = serializer.Worker;
        StringBuilder builder = serializer.Builder;
        string s;

        for (int i = 0; i < Value.Count - 1; i++)
        {
            s = Value[i];

            Debug.Assert(!string.IsNullOrEmpty(s));

            _ = worker.Clear().Append(s).Mask(serializer.Version);
            _ = builder.Append(worker).Append(',');
        }

        s = Value[Value.Count - 1];

        Debug.Assert(!string.IsNullOrEmpty(s));

        _ = worker.Clear().Append(s).Mask(serializer.Version);
        _ = builder.Append(worker);
    }
}
