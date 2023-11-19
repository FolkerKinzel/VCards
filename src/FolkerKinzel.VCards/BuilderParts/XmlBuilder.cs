using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct XmlBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal XmlBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(XElement? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Xmls,
                           VCardBuilder.Add(new XmlProperty(value, group?.Invoke(_builder.VCard)),
                                            _builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls),
                                            parameters,
                                            pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Xmls, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<XmlProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Xmls,
                          _builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls).Remove(predicate));
        return _builder;
    }
}

