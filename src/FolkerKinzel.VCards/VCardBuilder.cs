using System.Collections.Generic;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed class VCardBuilder
{
    private readonly VCard _vCard;

    private VCardBuilder(VCard vCard)
    {
        _vCard = vCard;
    }

    public static VCardBuilder Create(bool setUniqueIdentifier = true)
        => new(new VCard(setUniqueIdentifier));

    public static VCardBuilder Create(VCard vCard)
        => new(vCard ?? throw new ArgumentNullException(nameof(vCard)));

    public VCard Build() => _vCard;

    public VCardBuilder SetAccess(Access access)
    {
       _vCard.Access = new AccessProperty(access);
       return this;
    }

    public VCardBuilder AddAddress(string? street,
                                   string? locality,
                                   string? region,
                                   string? postalCode,
                                   string? country = null,
                                   string? group = null,
                                   Action<ParameterSection>? parameters = null,
                                   bool pref = false,
                                   bool autoLabel = true)
    {
        _vCard.Addresses = AddProperty(new AddressProperty(street, locality, region, postalCode, country, group, autoLabel),
                                          _vCard.Addresses,
                                          parameters,
                                          pref);

        return this;
    }

    public VCardBuilder AddAddress(IEnumerable<string?>? street,
                                   IEnumerable<string?>? locality,
                                   IEnumerable<string?>? region,
                                   IEnumerable<string?>? postalCode,
                                   IEnumerable<string?>? country = null,
                                   string? group = null,
                                   Action<ParameterSection>? parameters = null,
                                   bool pref = false,
                                   bool autoLabel = true)
    {
        _vCard.Addresses = AddProperty(new AddressProperty(street, locality, region, postalCode, country, group, autoLabel),
                                          _vCard.Addresses,
                                          parameters,
                                          pref);

        return this;
    }

    public VCardBuilder ClearAddresses()
    {
        _vCard.Addresses = null;
        return this;
    }

    public VCardBuilder RemoveAddress(AddressProperty? prop)
    {
        _vCard.Addresses = _vCard.Addresses.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveAddress(Func<AddressProperty?, bool> predicate)
    {
        _vCard.Addresses = _vCard.Addresses.Remove(predicate);
        return this;
    }

    public VCardBuilder AddAnniversaryView() => throw new NotImplementedException();

    public VCardBuilder ClearAnniversaryViews()
    {
        _vCard.AnniversaryViews = null;
        return this;
    }

    public VCardBuilder RemoveAnniversaryView(DateAndOrTimeProperty? prop)
    {
        _vCard.AnniversaryViews = _vCard.AnniversaryViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveAnniversaryView(Func<DateAndOrTimeProperty?, bool> predicate)
    {
        _vCard.AnniversaryViews = _vCard.AnniversaryViews.Remove(predicate);
        return this;
    }

    public VCardBuilder AddBirthDayView() => throw new NotImplementedException();

    public VCardBuilder ClearBirthDayViews()
    {
        _vCard.BirthDayViews = null;
        return this;
    }

    public VCardBuilder RemoveBirthDayView(DateAndOrTimeProperty? prop)
    {
        _vCard.BirthDayViews = _vCard.BirthDayViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveBirthDayView(Func<DateAndOrTimeProperty?, bool> predicate)
    {
        _vCard.BirthDayViews = _vCard.BirthDayViews.Remove(predicate);
        return this;
    }

    public VCardBuilder AddBirthPlaceView() => throw new NotImplementedException();

    public VCardBuilder ClearBirthPlaceViews()
    {
        _vCard.BirthPlaceViews = null;
        return this;
    }

    public VCardBuilder RemoveBirthPlaceView(TextProperty? prop)
    {
        _vCard.BirthPlaceViews = _vCard.BirthPlaceViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveBirthPlaceView(Func<TextProperty?, bool> predicate)
    {
        _vCard.BirthPlaceViews = _vCard.BirthPlaceViews.Remove(predicate);
        return this;
    }

    public VCardBuilder AddCalendarAddress() => throw new NotImplementedException();

    public VCardBuilder ClearCalendarAddresses()
    {
        _vCard.CalendarAddresses = null;
        return this;
    }

    public VCardBuilder RemoveCalendarAddress(TextProperty? prop)
    {
        _vCard.CalendarAddresses = _vCard.CalendarAddresses.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveCalendarAddress(Func<TextProperty?, bool> predicate)
    {
        _vCard.CalendarAddresses = _vCard.CalendarAddresses.Remove(predicate);
        return this;
    }

    public VCardBuilder AddCalendarUserAddress() => throw new NotImplementedException();

    public VCardBuilder ClearCalendarUserAddresses()
    {
        _vCard.CalendarUserAddresses = null;
        return this;
    }

    public VCardBuilder RemoveCalendarUserAddress(TextProperty? prop)
    {
        _vCard.CalendarUserAddresses = _vCard.CalendarUserAddresses.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveCalendarUserAddress(Func<TextProperty?, bool> predicate)
    {
        _vCard.CalendarUserAddresses = _vCard.CalendarUserAddresses.Remove(predicate);
        return this;
    }

    public VCardBuilder AddCategory() => throw new NotImplementedException();

    public VCardBuilder ClearCategories()
    {
        _vCard.Categories = null;
        return this;
    }

    public VCardBuilder RemoveCategory(StringCollectionProperty? prop)
    {
        _vCard.Categories = _vCard.Categories.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveCategory(Func<StringCollectionProperty?, bool> predicate)
    {
        _vCard.Categories = _vCard.Categories.Remove(predicate);
        return this;
    }

    public VCardBuilder AddDeathDateView() => throw new NotImplementedException();

    public VCardBuilder ClearDeathDateViews()
    {
        _vCard.DeathDateViews = null;
        return this;
    }

    public VCardBuilder RemoveDeathDateView(StringCollectionProperty? prop)
    {
        _vCard.DeathDateViews = _vCard.DeathDateViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveDeathDateView(Func<DateAndOrTimeProperty?, bool> predicate)
    {
        _vCard.DeathDateViews = _vCard.DeathDateViews.Remove(predicate);
        return this;
    }

    public VCardBuilder AddDeathPlaceView() => throw new NotImplementedException();

    public VCardBuilder ClearDeathPlaceViews()
    {
        _vCard.DeathPlaceViews = null;
        return this;
    }

    public VCardBuilder RemoveDeathPlaceView(TextProperty? prop)
    {
        _vCard.DeathPlaceViews = _vCard.DeathPlaceViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveDeathPlaceView(Func<TextProperty?, bool> predicate)
    {
        _vCard.DeathPlaceViews = _vCard.DeathPlaceViews.Remove(predicate);
        return this;
    }

    public VCardBuilder SetDirectoryName() => throw new NotImplementedException();

    public VCardBuilder ClearDirectoryName()
    {
        _vCard.DirectoryName = null;
        return this;
    }

    public VCardBuilder AddDisplayName() => throw new NotImplementedException();

    public VCardBuilder AddEMail() => throw new NotImplementedException();

    public VCardBuilder AddExpertise() => throw new NotImplementedException();

    public VCardBuilder AddFreeOrBusyUrl() => throw new NotImplementedException();

    public VCardBuilder AddGenderView() => throw new NotImplementedException();

    public VCardBuilder AddGeoCoordinate() => throw new NotImplementedException();

    public VCardBuilder AddHobby() => throw new NotImplementedException();

    public VCardBuilder AddMessenger() => throw new NotImplementedException();

    public VCardBuilder AddInterest() => throw new NotImplementedException();

    public VCardBuilder AddKey() => throw new NotImplementedException();

    public VCardBuilder SetKind() => throw new NotImplementedException();

    public VCardBuilder AddLanguage() => throw new NotImplementedException();

    public VCardBuilder AddLogo() => throw new NotImplementedException();

    public VCardBuilder SetMailer() => throw new NotImplementedException();

    public VCardBuilder AddMember() => throw new NotImplementedException();

    public VCardBuilder AddNameView() => throw new NotImplementedException();

    public VCardBuilder AddNickName() => throw new NotImplementedException();

    public VCardBuilder AddNonStandard() => throw new NotImplementedException();

    public VCardBuilder AddNote() => throw new NotImplementedException();

    public VCardBuilder AddOrganization() => throw new NotImplementedException();

    public VCardBuilder AddOrgDirectory() => throw new NotImplementedException();

    public VCardBuilder AddPhone() => throw new NotImplementedException();

    public VCardBuilder AddPhoto() => throw new NotImplementedException();

    public VCardBuilder SetProdID() => throw new NotImplementedException();

    public VCardBuilder SetProfile() => throw new NotImplementedException();

    public VCardBuilder AddRelation() => throw new NotImplementedException();

    public VCardBuilder AddRole() => throw new NotImplementedException();

    public VCardBuilder AddSound() => throw new NotImplementedException();

    public VCardBuilder AddSource() => throw new NotImplementedException();

    public VCardBuilder SetTimeStamp() => throw new NotImplementedException();

    public VCardBuilder AddTimeZone() => throw new NotImplementedException();

    public VCardBuilder AddTitle() => throw new NotImplementedException();

    public VCardBuilder SetUniqueIdentifier() => throw new NotImplementedException();

    public VCardBuilder AddUrl() => throw new NotImplementedException();

    public VCardBuilder AddXml() => throw new NotImplementedException();

    private IEnumerable<TSource?> AddProperty<TSource>(TSource prop,
                                                          IEnumerable<TSource?>? coll,
                                                          Action<ParameterSection>? parameters,
                                                          bool pref)
        where TSource : VCardProperty, IEnumerable<TSource>
    {
        parameters?.Invoke(prop.Parameters);

        coll = coll is null ? prop
                            : pref ? prop.Concat(coll.OrderByPref(false))
                                   : coll.Concat(prop);
        if (pref)
        {
            coll.SetPreferences(false);
        }

        return coll;
    }
}
