using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct AccessBuilder
{
    
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal AccessBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(Access value,
                            string? group = null)
    {
        Builder.VCard.Set(Prop.Access, new AccessProperty(value, group));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Access, null);
        return _builder;
    }
}
