using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Abstract base class of all classes that represent vCard properties.</summary>
public abstract class VCardProperty : ICloneable
{
    private string? _group;

    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The <see cref="VCardProperty" /> object to clone.</param>
    protected VCardProperty(VCardProperty prop)
    {
        Parameters = (ParameterSection)prop.Parameters.Clone();
        Group = prop.Group;
    }

    /// <summary>Constructor called by derived classes.</summary>
    /// <param name="parameters">A <see cref="ParameterSection" /> object that represents
    /// the parameter part of a vCard property.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="parameters" /> is <c>null</c>.</exception>
    protected VCardProperty(ParameterSection parameters, string? group)
    {
        _ArgumentNullException.ThrowIfNull(parameters, nameof(parameters));

        Parameters = parameters;
        Group = group;
    }

    /// <summary>The data provided by the <see cref="VCardProperty" />.</summary>
    public object Value => GetVCardPropertyValue();

    /// <summary>Abstract access method to get the data from <see cref="VCardProperty"
    /// />.</summary>
    /// <returns>The data provided by the <see cref="VCardProperty" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract object GetVCardPropertyValue();

    /// <summary>Corresponds to the group identifier of a vCard property, or is <c>null</c>
    /// if the <see cref="VCardProperty"/> does not belong to any group.</summary>
    public string? Group
    {
        get => _group;
        set => _group = string.IsNullOrWhiteSpace(value) ? null : value.Replace(" ", "");
    }

    /// <summary>Gets the data of the parameter section of a vCard property.</summary>
    public ParameterSection Parameters { get; }

    /// <summary>Returns <c>true</c>, if the <see cref="VCardProperty" /> object does
    /// not contain any usable data, otherwise <c>false</c>.</summary>
    public abstract bool IsEmpty {  get; }

    /// <summary>
    /// Returns a value that indicates whether <paramref name="prop"/>
    /// is <c>null</c> or does not contain any usable data.
    /// </summary>
    /// <param name="prop">The <see cref="VCardProperty"/> reference to examine.
    /// The argument may be <c>null</c>.</param>
    /// <returns><c>true</c> if <paramref name="prop"/> is <c>null</c> or if its
    /// <see cref="IsEmpty"/> property
    /// returns <c>true</c>, otherwise <c>false</c>.</returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)] VCardProperty? prop)
        => prop is null || prop.IsEmpty;

    /// <inheritdoc />
    public abstract object Clone();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => IsEmpty ? "<Empty>" : Value.ToString() ?? "";

    /// <summary>
    /// Fills <see cref="VcfSerializer.Builder"/> with the serialized <see cref="VCardProperty"/>.
    /// </summary>
    /// <param name="serializer">The <see cref="VcfSerializer"/>.</param>
    /// <returns><c>true</c> if line wrapping should be applied to the content of
    /// <see cref="VcfSerializer.Builder"/>, <c>false</c> if the content in builder 
    /// is already wrapped.</returns>
    internal bool BuildProperty(VcfSerializer serializer)
    {
        Asserts(serializer);

        StringBuilder builder = serializer.Builder;
        _ = builder.Clear();

        PrepareForVcfSerialization(serializer);

        if (serializer.Options.IsSet(VcfOpts.WriteGroups) && Group is not null)
        {
            _ = builder.Append(Group);
            _ = builder.Append('.');
        }
        _ = builder.Append(serializer.PropertyKey);

        serializer.ParameterSerializer
                  .AppendTo(builder, Parameters, serializer.PropertyKey, serializer.IsPref);

        _ = builder.Append(':');
        AppendValue(serializer);

        // Avoids line-wrapping in vCard 2.1 if it has already been done with
        // QuotedPrintable or Base64:
        return !(serializer.Version == VCdVersion.V2_1 &&
            (Parameters.Encoding ==
                Enc.QuotedPrintable || Parameters.Encoding == Enc.Base64));
    }

    internal virtual void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        Parameters.Encoding = null;
        Parameters.CharSet = null;
    }

    internal abstract void AppendValue(VcfSerializer serializer);

    [ExcludeFromCodeCoverage]
    [Conditional("DEBUG")]
    private void Asserts(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);
        Debug.Assert(serializer.PropertyKey is not null);
        Debug.Assert(!IsEmpty || serializer.Options.IsSet(VcfOpts.WriteEmptyProperties));
        Debug.Assert(serializer.Builder is not null);
    }
}
