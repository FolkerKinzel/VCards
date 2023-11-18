using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;


public sealed partial class VCardBuilder
{
    public VCardBuilder SetKind(Kind value, string? group = null, Action<ParameterSection>? parameters = null)
    {
        var prop = new KindProperty(value, group);
        parameters?.Invoke(prop.Parameters);

        _vCard.Kind = prop;
        return this;
    }

    public VCardBuilder ClearKind()
    {
        _vCard.Kind = null;
        return this;
    }
}
