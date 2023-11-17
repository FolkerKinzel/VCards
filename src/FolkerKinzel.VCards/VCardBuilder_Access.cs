using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetAccess(Access access, string? group = null)
    {
        _vCard.Access = new AccessProperty(access, group);
        return this;
    }

    public VCardBuilder ClearAccess()
    {
        _vCard.Access = null;
        return this;
    }
}
