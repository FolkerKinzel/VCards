using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TimeStampBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal TimeStampBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(DateTimeOffset value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var property = new TimeStampProperty(value, group);
        parameters?.Invoke(property.Parameters);

        Builder._vCard.Set(Prop.TimeStamp, property);
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.TimeStamp, null);
        return _builder!;
    }
}
