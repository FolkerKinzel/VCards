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

    public VCardBuilder ClearDisplayNames()
    {
        _vCard.DisplayNames = null;
        return this;
    }

    public VCardBuilder RemoveDisplayName(TextProperty? prop)
    {
        _vCard.DisplayNames = _vCard.DisplayNames.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveDisplayName(Func<TextProperty?, bool> predicate)
    {
        _vCard.DisplayNames = _vCard.DisplayNames.Remove(predicate);
        return this;
    }

    public VCardBuilder AddEMail() => throw new NotImplementedException();

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

    public VCardBuilder RemoveEMail(Func<TextProperty?, bool> predicate)
    {
        _vCard.EMails = _vCard.EMails.Remove(predicate);
        return this;
    }

    public VCardBuilder AddExpertise() => throw new NotImplementedException();

    public VCardBuilder ClearExpertises()
    {
        _vCard.Expertises = null;
        return this;
    }

    public VCardBuilder RemoveExpertise(TextProperty? prop)
    {
        _vCard.Expertises = _vCard.Expertises.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveExpertise(Func<TextProperty?, bool> predicate)
    {
        _vCard.Expertises = _vCard.Expertises.Remove(predicate);
        return this;
    }

    public VCardBuilder AddFreeOrBusyUrl() => throw new NotImplementedException();

    public VCardBuilder ClearFreeOrBusyUrls()
    {
        _vCard.FreeOrBusyUrls = null;
        return this;
    }

    public VCardBuilder RemoveFreeOrBusyUrl(TextProperty? prop)
    {
        _vCard.FreeOrBusyUrls = _vCard.FreeOrBusyUrls.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveFreeOrBusyUrl(Func<TextProperty?, bool> predicate)
    {
        _vCard.FreeOrBusyUrls = _vCard.FreeOrBusyUrls.Remove(predicate);
        return this;
    }

    public VCardBuilder AddGenderView() => throw new NotImplementedException();

    public VCardBuilder ClearGenderViews()
    {
        _vCard.GenderViews = null;
        return this;
    }

    public VCardBuilder RemoveGenderView(GenderProperty? prop)
    {
        _vCard.GenderViews = _vCard.GenderViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveGenderView(Func<GenderProperty?, bool> predicate)
    {
        _vCard.GenderViews = _vCard.GenderViews.Remove(predicate);
        return this;
    }

    public VCardBuilder AddGeoCoordinate() => throw new NotImplementedException();

    public VCardBuilder ClearGeoCoordinates()
    {
        _vCard.GeoCoordinates = null;
        return this;
    }

    public VCardBuilder RemoveGeoCoordinate(GeoProperty? prop)
    {
        _vCard.GeoCoordinates = _vCard.GeoCoordinates.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveGeoCoordinate(Func<GeoProperty?, bool> predicate)
    {
        _vCard.GeoCoordinates = _vCard.GeoCoordinates.Remove(predicate);
        return this;
    }

    public VCardBuilder AddHobby() => throw new NotImplementedException();

    public VCardBuilder ClearHobbies()
    {
        _vCard.Hobbies = null;
        return this;
    }

    public VCardBuilder RemoveHobby(TextProperty? prop)
    {
        _vCard.Hobbies = _vCard.Hobbies.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveHobby(Func<TextProperty?, bool> predicate)
    {
        _vCard.Hobbies = _vCard.Hobbies.Remove(predicate);
        return this;
    }

    public VCardBuilder AddMessenger() => throw new NotImplementedException();

    public VCardBuilder ClearMessengers()
    {
        _vCard.Messengers = null;
        return this;
    }

    public VCardBuilder RemoveMessenger(TextProperty? prop)
    {
        _vCard.Messengers = _vCard.Messengers.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveMessenger(Func<TextProperty?, bool> predicate)
    {
        _vCard.Messengers = _vCard.Messengers.Remove(predicate);
        return this;
    }

    public VCardBuilder AddInterest() => throw new NotImplementedException();

    public VCardBuilder ClearInterests()
    {
        _vCard.Interests = null;
        return this;
    }

    public VCardBuilder RemoveInterest(TextProperty? prop)
    {
        _vCard.Interests = _vCard.Interests.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveInterest(Func<TextProperty?, bool> predicate)
    {
        _vCard.Interests = _vCard.Interests.Remove(predicate);
        return this;
    }

    public VCardBuilder AddKey() => throw new NotImplementedException();

    public VCardBuilder ClearKeys()
    {
        _vCard.Keys = null;
        return this;
    }

    public VCardBuilder RemoveKey(DataProperty? prop)
    {
        _vCard.Keys = _vCard.Keys.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveKey(Func<DataProperty?, bool> predicate)
    {
        _vCard.Keys = _vCard.Keys.Remove(predicate);
        return this;
    }

    public VCardBuilder SetKind() => throw new NotImplementedException();

    public VCardBuilder ClearKind()
    {
        _vCard.Kind = null;
        return this;
    }

    public VCardBuilder AddLanguage() => throw new NotImplementedException();

    public VCardBuilder ClearLanguages()
    {
        _vCard.Languages = null;
        return this;
    }

    public VCardBuilder RemoveLanguage(TextProperty? prop)
    {
        _vCard.Languages = _vCard.Languages.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveLanguage(Func<TextProperty?, bool> predicate)
    {
        _vCard.Languages = _vCard.Languages.Remove(predicate);
        return this;
    }

    public VCardBuilder AddLogo() => throw new NotImplementedException();

    public VCardBuilder ClearLogos()
    {
        _vCard.Logos = null;
        return this;
    }

    public VCardBuilder RemoveLogo(DataProperty? prop)
    {
        _vCard.Logos = _vCard.Logos.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveLogo(Func<DataProperty?, bool> predicate)
    {
        _vCard.Logos = _vCard.Logos.Remove(predicate);
        return this;
    }

    public VCardBuilder SetMailer() => throw new NotImplementedException();

    public VCardBuilder ClearMailer()
    {
        _vCard.Mailer = null;
        return this;
    }

    public VCardBuilder AddMember() => throw new NotImplementedException();

    public VCardBuilder ClearMembers()
    {
        _vCard.Members = null;
        return this;
    }

    public VCardBuilder RemoveMember(RelationProperty? prop)
    {
        _vCard.Members = _vCard.Members.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveMember(Func<RelationProperty?, bool> predicate)
    {
        _vCard.Members = _vCard.Members.Remove(predicate);
        return this;
    }

    public VCardBuilder AddNameView() => throw new NotImplementedException();

    public VCardBuilder ClearNameViews()
    {
        _vCard.NameViews = null;
        return this;
    }

    public VCardBuilder RemoveNameView(NameProperty? prop)
    {
        _vCard.NameViews = _vCard.NameViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveNameView(Func<NameProperty?, bool> predicate)
    {
        _vCard.NameViews = _vCard.NameViews.Remove(predicate);
        return this;
    }

    public VCardBuilder AddNickName() => throw new NotImplementedException();

    public VCardBuilder ClearNickNames()
    {
        _vCard.NickNames = null;
        return this;
    }

    public VCardBuilder RemoveNickName(StringCollectionProperty? prop)
    {
        _vCard.NickNames = _vCard.NickNames.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveNickName(Func<StringCollectionProperty?, bool> predicate)
    {
        _vCard.NickNames = _vCard.NickNames.Remove(predicate);
        return this;
    }

    public VCardBuilder AddNonStandard() => throw new NotImplementedException();

    public VCardBuilder ClearNonStandards()
    {
        _vCard.NonStandards = null;
        return this;
    }

    public VCardBuilder RemoveNonStandard(NonStandardProperty? prop)
    {
        _vCard.NonStandards = _vCard.NonStandards.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveNonStandard(Func<NonStandardProperty?, bool> predicate)
    {
        _vCard.NonStandards = _vCard.NonStandards.Remove(predicate);
        return this;
    }

    public VCardBuilder AddNote() => throw new NotImplementedException();

    public VCardBuilder ClearNotes()
    {
        _vCard.Notes = null;
        return this;
    }

    public VCardBuilder RemoveNote(TextProperty? prop)
    {
        _vCard.Notes = _vCard.Notes.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveNote(Func<TextProperty?, bool> predicate)
    {
        _vCard.Notes = _vCard.Notes.Remove(predicate);
        return this;
    }

    public VCardBuilder AddOrganization() => throw new NotImplementedException();

    public VCardBuilder ClearOrganizations()
    {
        _vCard.Organizations = null;
        return this;
    }

    public VCardBuilder RemoveOrganization(OrgProperty? prop)
    {
        _vCard.Organizations = _vCard.Organizations.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveOrganization(Func<OrgProperty?, bool> predicate)
    {
        _vCard.Organizations = _vCard.Organizations.Remove(predicate);
        return this;
    }

    public VCardBuilder AddOrgDirectory() => throw new NotImplementedException();

    public VCardBuilder ClearOrgDirectories()
    {
        _vCard.OrgDirectories = null;
        return this;
    }

    public VCardBuilder RemoveOrgDirectory(TextProperty? prop)
    {
        _vCard.OrgDirectories = _vCard.OrgDirectories.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveOrgDirectory(Func<TextProperty?, bool> predicate)
    {
        _vCard.OrgDirectories = _vCard.OrgDirectories.Remove(predicate);
        return this;
    }

    public VCardBuilder AddPhone() => throw new NotImplementedException();

    public VCardBuilder ClearPhones()
    {
        _vCard.Phones = null;
        return this;
    }

    public VCardBuilder RemovePhone(TextProperty? prop)
    {
        _vCard.Phones = _vCard.Phones.Remove(prop);
        return this;
    }

    public VCardBuilder RemovePhone(Func<TextProperty?, bool> predicate)
    {
        _vCard.Phones = _vCard.Phones.Remove(predicate);
        return this;
    }

    public VCardBuilder AddPhoto() => throw new NotImplementedException();

    public VCardBuilder ClearPhotos()
    {
        _vCard.Photos = null;
        return this;
    }

    public VCardBuilder RemovePhoto(DataProperty? prop)
    {
        _vCard.Photos = _vCard.Photos.Remove(prop);
        return this;
    }

    public VCardBuilder RemovePhoto(Func<DataProperty?, bool> predicate)
    {
        _vCard.Photos = _vCard.Photos.Remove(predicate);
        return this;
    }

    public VCardBuilder SetProductID() => throw new NotImplementedException();

    public VCardBuilder ClearProductID()
    {
        _vCard.ProductID = null;
        return this;
    }

    public VCardBuilder SetProfile() => throw new NotImplementedException();

    public VCardBuilder ClearProfile()
    {
        _vCard.Profile = null;
        return this;
    }

    public VCardBuilder AddRelation() => throw new NotImplementedException();

    public VCardBuilder ClearRelations()
    {
        _vCard.Relations = null;
        return this;
    }

    public VCardBuilder RemoveRelation(RelationProperty? prop)
    {
        _vCard.Relations = _vCard.Relations.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveRelation(Func<RelationProperty?, bool> predicate)
    {
        _vCard.Relations = _vCard.Relations.Remove(predicate);
        return this;
    }

    public VCardBuilder AddRole() => throw new NotImplementedException();

    public VCardBuilder ClearRoles()
    {
        _vCard.Roles = null;
        return this;
    }

    public VCardBuilder RemoveRole(TextProperty? prop)
    {
        _vCard.Roles = _vCard.Roles.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveRole(Func<TextProperty?, bool> predicate)
    {
        _vCard.Roles = _vCard.Roles.Remove(predicate);
        return this;
    }

    public VCardBuilder AddSound() => throw new NotImplementedException();

    public VCardBuilder ClearSounds()
    {
        _vCard.Sounds = null;
        return this;
    }

    public VCardBuilder RemoveSound(DataProperty? prop)
    {
        _vCard.Sounds = _vCard.Sounds.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveSound(Func<DataProperty?, bool> predicate)
    {
        _vCard.Sounds = _vCard.Sounds.Remove(predicate);
        return this;
    }

    public VCardBuilder AddSource() => throw new NotImplementedException();

    public VCardBuilder ClearSources()
    {
        _vCard.Sources = null;
        return this;
    }

    public VCardBuilder RemoveSource(TextProperty? prop)
    {
        _vCard.Sources = _vCard.Sources.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveSource(Func<TextProperty?, bool> predicate)
    {
        _vCard.Sources = _vCard.Sources.Remove(predicate);
        return this;
    }

    public VCardBuilder SetTimeStamp() => throw new NotImplementedException();

    public VCardBuilder ClearTimeStamp()
    {
        _vCard.TimeStamp = null;
        return this;
    }

    public VCardBuilder AddTimeZone() => throw new NotImplementedException();

    public VCardBuilder ClearTimeZones()
    {
        _vCard.TimeZones = null;
        return this;
    }

    public VCardBuilder RemoveTimeZone(TimeZoneProperty? prop)
    {
        _vCard.TimeZones = _vCard.TimeZones.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveTimeZone(Func<TimeZoneProperty?, bool> predicate)
    {
        _vCard.TimeZones = _vCard.TimeZones.Remove(predicate);
        return this;
    }


    public VCardBuilder AddTitle() => throw new NotImplementedException();

    public VCardBuilder ClearTitles()
    {
        _vCard.Titles = null;
        return this;
    }

    public VCardBuilder RemoveTitle(TextProperty? prop)
    {
        _vCard.Titles = _vCard.Titles.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveTitle(Func<TextProperty?, bool> predicate)
    {
        _vCard.Titles = _vCard.Titles.Remove(predicate);
        return this;
    }

    public VCardBuilder SetUniqueIdentifier() => throw new NotImplementedException();

    public VCardBuilder ClearUniqueIdentifier()
    {
        _vCard.UniqueIdentifier = null;
        return this;
    }

    public VCardBuilder AddUrl() => throw new NotImplementedException();

    public VCardBuilder ClearUrls()
    {
        _vCard.Urls = null;
        return this;
    }

    public VCardBuilder RemoveUrl(TextProperty? prop)
    {
        _vCard.Urls = _vCard.Urls.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveUrl(Func<TextProperty?, bool> predicate)
    {
        _vCard.Urls = _vCard.Urls.Remove(predicate);
        return this;
    }

    public VCardBuilder AddXml() => throw new NotImplementedException();

    public VCardBuilder ClearXmls()
    {
        _vCard.Xmls = null;
        return this;
    }

    public VCardBuilder RemoveXml(XmlProperty? prop)
    {
        _vCard.Xmls = _vCard.Xmls.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveXml(Func<XmlProperty?, bool> predicate)
    {
        _vCard.Xmls = _vCard.Xmls.Remove(predicate);
        return this;
    }

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
