using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct GeoBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal GeoBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(double latitude,
                            double longitude,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, VCardBuilder.Add(new GeoProperty(latitude, longitude, group),
                                                  Builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Add(GeoCoordinate? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, VCardBuilder.Add(new GeoProperty(value, group),
                                                  Builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.GeoCoordinates, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<GeoProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, Builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates).Remove(predicate));
        return _builder!;
    }
}
