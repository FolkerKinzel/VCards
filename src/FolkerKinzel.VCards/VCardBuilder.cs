using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
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
  

    public VCardBuilder SetDirectoryName() => throw new NotImplementedException();

    public VCardBuilder ClearDirectoryName()
    {
        _vCard.DirectoryName = null;
        return this;
    }


    public VCardBuilder AddKey() => throw new NotImplementedException();

    public VCardBuilder ClearKeys()
    {
        _vCard.Keys = null;
        return this;
    }

    //public VCardBuilder RemoveKey(DataProperty? prop)
    //{
    //    _vCard.Keys = _vCard.Keys.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveKey(Func<DataProperty, bool> predicate)
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


    public VCardBuilder AddLogo() => throw new NotImplementedException();

    public VCardBuilder ClearLogos()
    {
        _vCard.Logos = null;
        return this;
    }

    //public VCardBuilder RemoveLogo(DataProperty? prop)
    //{
    //    _vCard.Logos = _vCard.Logos.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveLogo(Func<DataProperty, bool> predicate)
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

    //public VCardBuilder RemoveMember(RelationProperty? prop)
    //{
    //    _vCard.Members = _vCard.Members.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveMember(Func<RelationProperty, bool> predicate)
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

    //public VCardBuilder RemoveNameView(NameProperty? prop)
    //{
    //    _vCard.NameViews = _vCard.NameViews.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveNameView(Func<NameProperty, bool> predicate)
    {
        _vCard.NameViews = _vCard.NameViews.Remove(predicate);
        return this;
    }

 
    public VCardBuilder AddNonStandard() => throw new NotImplementedException();

    public VCardBuilder ClearNonStandards()
    {
        _vCard.NonStandards = null;
        return this;
    }

    //public VCardBuilder RemoveNonStandard(NonStandardProperty? prop)
    //{
    //    _vCard.NonStandards = _vCard.NonStandards.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveNonStandard(Func<NonStandardProperty, bool> predicate)
    {
        _vCard.NonStandards = _vCard.NonStandards.Remove(predicate);
        return this;
    }


    public VCardBuilder AddOrganization() => throw new NotImplementedException();

    public VCardBuilder ClearOrganizations()
    {
        _vCard.Organizations = null;
        return this;
    }

    //public VCardBuilder RemoveOrganization(OrgProperty? prop)
    //{
    //    _vCard.Organizations = _vCard.Organizations.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveOrganization(Func<OrgProperty, bool> predicate)
    {
        _vCard.Organizations = _vCard.Organizations.Remove(predicate);
        return this;
    }


    public VCardBuilder AddPhoto() => throw new NotImplementedException();

    public VCardBuilder ClearPhotos()
    {
        _vCard.Photos = null;
        return this;
    }

    //public VCardBuilder RemovePhoto(DataProperty? prop)
    //{
    //    _vCard.Photos = _vCard.Photos.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemovePhoto(Func<DataProperty, bool> predicate)
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

    //public VCardBuilder RemoveRelation(RelationProperty? prop)
    //{
    //    _vCard.Relations = _vCard.Relations.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveRelation(Func<RelationProperty, bool> predicate)
    {
        _vCard.Relations = _vCard.Relations.Remove(predicate);
        return this;
    }


    public VCardBuilder AddSound() => throw new NotImplementedException();

    public VCardBuilder ClearSounds()
    {
        _vCard.Sounds = null;
        return this;
    }

    //public VCardBuilder RemoveSound(DataProperty? prop)
    //{
    //    _vCard.Sounds = _vCard.Sounds.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveSound(Func<DataProperty, bool> predicate)
    {
        _vCard.Sounds = _vCard.Sounds.Remove(predicate);
        return this;
    }


    


    


    

    public VCardBuilder AddXml() => throw new NotImplementedException();

    public VCardBuilder ClearXmls()
    {
        _vCard.Xmls = null;
        return this;
    }

    //public VCardBuilder RemoveXml(XmlProperty? prop)
    //{
    //    _vCard.Xmls = _vCard.Xmls.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveXml(Func<XmlProperty, bool> predicate)
    {
        _vCard.Xmls = _vCard.Xmls.Remove(predicate);
        return this;
    }

    private IEnumerable<TSource?> Add<TSource>(TSource prop,
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
