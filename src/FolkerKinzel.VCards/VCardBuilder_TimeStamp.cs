using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetTimeStamp()
    {
        _vCard.TimeStamp = new TimeStampProperty();
        return this;
    }

    public VCardBuilder SetTimeStamp(DateTimeOffset value)
    {
        _vCard.TimeStamp = new TimeStampProperty(value);
        return this;
    }

    public VCardBuilder ClearTimeStamp()
    {
        _vCard.TimeStamp = null;
        return this;
    }
}
