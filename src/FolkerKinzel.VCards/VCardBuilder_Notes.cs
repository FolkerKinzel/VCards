using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddNote(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Notes = VCardBuilder.Add(new TextProperty(value, group),
                                              _vCard.Notes,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearNotes()
    {
        _vCard.Notes = null;
        return this;
    }

    //public VCardBuilder RemoveNote(TextProperty? prop)
    //{
    //    _vCard.Notes = _vCard.Notes.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveNote(Func<TextProperty, bool> predicate)
    {
        _vCard.Notes = _vCard.Notes.Remove(predicate);
        return this;
    }

}
