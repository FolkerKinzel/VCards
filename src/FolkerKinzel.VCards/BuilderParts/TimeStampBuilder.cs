using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TimeStampBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal TimeStampBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(DateTimeOffset value,
                            Func<VCard, string?>? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var vc = Builder.VCard;
        var property = new TimeStampProperty(value, group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.TimeStamp, property);
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.TimeStamp, null);
        return _builder;
    }
}
