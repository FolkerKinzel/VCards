using FolkerKinzel.VCards.Intls.Extensions;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed class Relation
{
    private readonly OneOf<string, VCard, Guid, Uri> _oneOf;

    internal Relation(OneOf<string, VCard, Guid, Uri> oneOf) => _oneOf = oneOf;


    public string? String => IsString ? AsStringIntl : null;

    public VCard? VCard => IsVCard ? AsVCardIntl : null;

    public Guid? Guid => IsGuid ? AsGuidIntl : null;

    public Uri? Uri => IsUri ? AsUriIntl : null;

    public object Value => this._oneOf.Value;


    public bool TryAsString([NotNullWhen(true)] out string? str)
    {
        str = Match(
            static s => s,
            static vc => vc.DisplayNames?.WhereNotEmpty()
                                         .OrderByPref()
                                         .FirstOrDefault()?
                                         .Value ??
                          vc.NameViews?.WhereNotEmpty()
                                       .FirstOrDefault()?.ToDisplayName() ??
                          vc.Organizations?.WhereNotEmpty()
                                           .OrderByPref()
                                           .FirstOrDefault()?.Value.OrganizationName,
            static guid => null,
            static uri => uri.ToString()
            );
        return str != null;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<string>? f0,
                       Action<VCard>? f1,
                       Action<Guid>? f2,
                       Action<Uri>? f3)
        => _oneOf.Switch(f0, f1, f2, f3);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<string, TResult>? f0,
                                  Func<VCard, TResult>? f1,
                                  Func<Guid, TResult>? f2,
                                  Func<Uri, TResult>? f3)
        => _oneOf.Match(f0, f1, f2, f3);


    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _oneOf.ToString();



    [MemberNotNullWhen(true, nameof(String))]
    private bool IsString => _oneOf.IsT0;

    [MemberNotNullWhen(true, nameof(VCard))]
    private bool IsVCard => _oneOf.IsT1;

    [MemberNotNullWhen(true, nameof(Guid))]
    private bool IsGuid => _oneOf.IsT2;

    [MemberNotNullWhen(true, nameof(Uri))]
    private bool IsUri => _oneOf.IsT3;

    private string AsStringIntl => _oneOf.AsT0;

    private VCard AsVCardIntl => _oneOf.AsT1;
    
    private Guid AsGuidIntl => _oneOf.AsT2;

    private Uri AsUriIntl => _oneOf.AsT3;
}

