using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed class Relation
{
    private readonly OneOf<string, VCard, Guid, Uri> _oneOf;

    internal Relation(OneOf<string, VCard, Guid, Uri> oneOf) => _oneOf = oneOf;


    public string? String => IsString ? AsString : null;

    public VCard? VCard => IsVCard ? AsVCard : null;

    public Guid? Guid => IsGuid ? AsGuid : null;

    public Uri? Uri => IsUri ? AsUri : null;

    public object Value => this._oneOf.Value;


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

    private string AsString => _oneOf.AsT0;

    private VCard AsVCard => _oneOf.AsT1;
    
    private Guid AsGuid => _oneOf.AsT2;

    private Uri AsUri => _oneOf.AsT3;
}

