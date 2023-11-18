using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct KindBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal KindBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(Kind value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var property = new KindProperty(value, group);
        parameters?.Invoke(property.Parameters);

        Builder.VCard.Set(Prop.Kind, property);
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Kind, null);
        return _builder;
    }
}
