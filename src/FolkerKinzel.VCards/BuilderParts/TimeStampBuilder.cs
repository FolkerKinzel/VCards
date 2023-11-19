using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TimeStampBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal TimeStampBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(DateTimeOffset value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
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
