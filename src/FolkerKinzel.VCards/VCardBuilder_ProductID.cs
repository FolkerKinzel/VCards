using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder SetProductID(string? value,
                                string? group = null,
                                           Action<ParameterSection>? parameters = null)
    {
        var prop = new TextProperty(value, group);
        parameters?.Invoke(prop.Parameters);

        _vCard.ProductID = prop;
        return this;
    }

    public VCardBuilder ClearProductID()
    {
        _vCard.ProductID = null;
        return this;
    }

}
