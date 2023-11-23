using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct DataBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal DataBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    public VCardBuilder AddFile(string filePath,
                                string? mimeType = null,
                                bool pref = false,
                                Action<ParameterSection>? parameters = null,
                                Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromFile(filePath, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder AddBytes(byte[]? bytes,
                                 string? mimeType = MimeString.OctetStream,
                                 bool pref = false,
                                 Action<ParameterSection>? parameters = null,
                                 Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromBytes(bytes, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder AddText(string? passWord,
                                string? mimeType = null,
                                bool pref = false,
                                Action<ParameterSection>? parameters = null,
                                Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromText(passWord, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder AddUri(Uri? uri,
                               string? mimeType = null,
                               bool pref = false, 
                               Action<ParameterSection>? parameters = null, 
                               Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromUri(uri, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<DataProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop, _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop).Remove(predicate));
        return _builder;
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}

