using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct UuidBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal UuidBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(Guid value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var property = new UuidProperty(value, group);
        parameters?.Invoke(property.Parameters);

        Builder.VCard.Set(Prop.ID, property);
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.ID, null);
        return _builder!;
    }
}
