using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct XmlBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal XmlBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(XElement? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop.Xmls,
                           VCardBuilder.Add(new XmlProperty(value, group),
                                            Builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls),
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
        Builder.VCard.Set(Prop.Xmls, Builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls).Remove(predicate));
        return _builder;
    }
}

