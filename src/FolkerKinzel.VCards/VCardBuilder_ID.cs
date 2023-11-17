using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetID()
    {
        _vCard.ID = new UuidProperty();
        return this;
    }

    public VCardBuilder SetID(Guid uuid)
    {
        _vCard.ID = new UuidProperty(uuid);
        return this;
    }

    public VCardBuilder ClearID()
    {
        _vCard.ID = null;
        return this;
    }
}
