using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddEMail(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.EMails = Add(new TextProperty(value, group),
                                              _vCard.EMails,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearEMails()
    {
        _vCard.EMails = null;
        return this;
    }

    public VCardBuilder RemoveEMail(TextProperty? prop)
    {
        _vCard.EMails = _vCard.EMails.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveEMail(Func<TextProperty, bool> predicate)
    {
        _vCard.EMails = _vCard.EMails.Remove(predicate);
        return this;
    }
}
