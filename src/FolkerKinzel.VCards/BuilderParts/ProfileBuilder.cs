using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct ProfileBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal ProfileBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Profile, new ProfileProperty(group?.Invoke(_builder.VCard)));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Profile, null);
        return _builder;
    }
}
