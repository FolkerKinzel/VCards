using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetProfile(string? group = null)
    {
        _vCard.Profile = new ProfileProperty(group);
        return this;
    }

    public VCardBuilder ClearProfile()
    {
        _vCard.Profile = null;
        return this;
    }
}
