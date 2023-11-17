using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetMailer(string? value,
                                string? group = null,
                                           Action<ParameterSection>? parameters = null)
    {
        var prop = new TextProperty(value, group);
        parameters?.Invoke(prop.Parameters);

        _vCard.Mailer = prop;
        return this;
    }

    public VCardBuilder ClearMailer()
    {
        _vCard.Mailer = null;
        return this;
    }
}
