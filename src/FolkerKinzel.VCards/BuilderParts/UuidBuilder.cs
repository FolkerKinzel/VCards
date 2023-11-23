using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct UuidBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal UuidBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Set(Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        var vc = Builder.VCard;
        var property = new UuidProperty(group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.ID, property);
        return _builder;
    }

    public VCardBuilder Set(Guid value,
                            Func<VCard, string?>? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var vc = Builder.VCard;
        var property = new UuidProperty(value, group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.ID, property);
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.ID, null);
        return _builder;
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}
