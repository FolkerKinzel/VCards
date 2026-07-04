using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class ContactIDUri : ContactID
{
    internal ContactIDUri(Uri uri, ContactID? comparer)
    {
        Debug.Assert(uri != null);
        Debug.Assert(uri.IsAbsoluteUri);

        Uri = uri;
        Comparer = comparer ?? this;
    }

    public override Guid? Guid => null;

    [NotNull]
    public override Uri? Uri { get; }

    public override string? String => null;

    public override TResult Convert<TResult>(Func<Guid, TResult> guidFunc,
                                             Func<Uri, TResult> uriFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(uriFunc, nameof(uriFunc));
        return uriFunc(Uri);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<Guid, TArg, TResult> guidFunc,
                                                   Func<Uri, TArg, TResult> uriFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(uriFunc, nameof(uriFunc));
        return uriFunc(Uri, arg);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch(Action<Guid>? guidAction = null,
                                Action<Uri>? uriAction = null,
                                Action<string>? stringAction = null) => uriAction?.Invoke(Uri);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch<TArg>(TArg arg,
                                      Action<Guid, TArg>? guidAction = null,
                                      Action<Uri, TArg>? uriAction = null,
                                      Action<string, TArg>? stringAction = null) => uriAction?.Invoke(Uri, arg);

    public override bool Equals([NotNullWhen(true)] ContactID? other)
        => other is not null &&
            (ReferenceEquals(this, Comparer)
                ? other.Comparer is ContactIDUri comparer && Uri.Equals(comparer.Uri)
                : Comparer.Equals(other.Comparer));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
        => ReferenceEquals(this, Comparer) ? Uri.GetHashCode() : Comparer.GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"Uri: {Uri}";
}
