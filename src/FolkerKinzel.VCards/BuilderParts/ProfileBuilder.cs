using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct ProfileBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal ProfileBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(string? group = null)
    {
        Builder._vCard.Set(Prop.Profile, new ProfileProperty(group));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.Profile, null);
        return _builder!;
    }
}
