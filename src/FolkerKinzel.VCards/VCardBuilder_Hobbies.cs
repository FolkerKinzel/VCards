using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddHobby(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Hobbies = Add(new TextProperty(value, group),
                                              _vCard.Hobbies,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearHobbies()
    {
        _vCard.Hobbies = null;
        return this;
    }

    //public VCardBuilder RemoveHobby(TextProperty? prop)
    //{
    //    _vCard.Hobbies = _vCard.Hobbies.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveHobby(Func<TextProperty, bool> predicate)
    {
        _vCard.Hobbies = _vCard.Hobbies.Remove(predicate);
        return this;
    }
}
