using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetKind(Kind value)
    {
        _vCard.Kind = new KindProperty(value);
        return this;
    }

    public VCardBuilder ClearKind()
    {
        _vCard.Kind = null;
        return this;
    }
}
