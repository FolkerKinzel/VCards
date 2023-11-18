using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;


public sealed partial class VCardBuilder
{
    

    public VCardBuilder SetID(Guid uuid, 
                              string? group = null,
                              Action<ParameterSection>? parameters = null)
    {
        var prop = new UuidProperty(uuid, group);
        parameters?.Invoke(prop.Parameters);
        _vCard.ID = prop;
        return this;
    }

    public VCardBuilder ClearID()
    {
        _vCard.ID = null;
        return this;
    }
}
