using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetTimeStamp(string? group = null, Action<ParameterSection>? parameters = null)
    {
        var prop = new TimeStampProperty(group);
        parameters?.Invoke(prop.Parameters);
        _vCard.TimeStamp = prop;
        return this;
    }

    public VCardBuilder SetTimeStamp(DateTimeOffset value, string? group = null, Action<ParameterSection>? parameters = null)
    {
        var prop = new TimeStampProperty(value, group);
        parameters?.Invoke(prop.Parameters);
        _vCard.TimeStamp = prop;
        return this;
    }

    public VCardBuilder ClearTimeStamp()
    {
        _vCard.TimeStamp = null;
        return this;
    }
}
