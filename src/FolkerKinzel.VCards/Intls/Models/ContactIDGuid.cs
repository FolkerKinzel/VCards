using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class ContactIDGuid : ContactID
{
    internal ContactIDGuid(Guid guid) => Guid = guid;

    [NotNull]
    public override Guid? Guid { get; }

    public override Uri? Uri => null;

    public override string? String => null;

    public override TResult Convert<TResult>(Func<Guid, TResult> guidFunc,
                                             Func<Uri, TResult> uriFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(guidFunc, nameof(guidFunc));
        return guidFunc(Guid.Value);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<Guid, TArg, TResult> guidFunc,
                                                   Func<Uri, TArg, TResult> uriFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(guidFunc, nameof(guidFunc));
        return guidFunc(Guid.Value, arg);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch(Action<Guid>? guidAction = null,
                                Action<Uri>? uriAction = null,
                                Action<string>? stringAction = null) => guidAction?.Invoke(Guid.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch<TArg>(TArg arg,
                                      Action<Guid, TArg>? guidAction = null,
                                      Action<Uri, TArg>? uriAction = null,
                                      Action<string, TArg>? stringAction = null) => guidAction?.Invoke(Guid.Value, arg);

    public override bool Equals(ContactID? other) => other is ContactIDGuid ctGuid && Guid.Value.Equals(ctGuid.Guid.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Guid.Value.GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"Guid: {Guid.Value}";
}
