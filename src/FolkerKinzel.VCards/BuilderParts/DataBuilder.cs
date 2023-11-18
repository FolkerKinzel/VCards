using System.Xml.Linq;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct DataBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal DataBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder AddFile(string filePath,
                                string? mimeType = null,
                                Func<VCard, string?>? group = null,
                                Action<ParameterSection>? parameters = null,
                                bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DataProperty.FromFile(filePath, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder AddBytes(byte[]? bytes,
                                 string? mimeType = MimeString.OctetStream,
                                 Func<VCard, string?>? group = null,
                                 Action<ParameterSection>? parameters = null,
                                 bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DataProperty.FromBytes(bytes, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder AddText(string? text,
                                string? mimeType = null,
                                Func<VCard, string?>? group = null,
                                Action<ParameterSection>? parameters = null,
                                bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DataProperty.FromText(text, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder AddUri(Uri? uri,
                               string? mimeType = null,
                               Func<VCard, string?>? group = null,
                               Action<ParameterSection>? parameters = null,
                               bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DataProperty.FromUri(uri, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<DataProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop, _builder.VCard.Get<IEnumerable<DataProperty?>?>(Prop).Remove(predicate));
        return _builder;
    }
}

