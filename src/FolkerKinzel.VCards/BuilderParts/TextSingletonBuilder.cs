using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TextSingletonBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal TextSingletonBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Set(string? value,
                            Func<VCard, string?>? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var vc = Builder.VCard;
        var property = new TextProperty(value, group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop, property);
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder;
    }
}
