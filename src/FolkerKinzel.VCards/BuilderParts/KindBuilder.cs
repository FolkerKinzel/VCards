using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct KindBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal KindBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(Kind value,
                            Action<ParameterSection>? parameters = null, 
                            Func<VCard, string?>? group = null)
    {
        var vc = Builder.VCard;
        var property = new KindProperty(value, group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.Kind, property);
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Kind, null);
        return _builder;
    }
}
