using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class ContactIDString : ContactID
{
    internal ContactIDString(string text) => String = text;

    public override Guid? Guid => null;

    public override Uri? Uri => null;

    [NotNull]
    public override string? String { get; }

    public override TResult Convert<TResult>(Func<Guid, TResult> guidFunc,
                                             Func<Uri, TResult> uriFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(stringFunc, nameof(stringFunc));
        return stringFunc(String);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<Guid, TArg, TResult> guidFunc,
                                                   Func<Uri, TArg, TResult> uriFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(stringFunc, nameof(stringFunc));
        return stringFunc(String, arg);
    }

    public override void Switch(Action<Guid>? guidAction = null,
                                Action<Uri>? uriAction = null,
                                Action<string>? stringAction = null) => stringAction?.Invoke(String);

    public override void Switch<TArg>(TArg arg,
                                      Action<Guid, TArg>? guidAction = null,
                                      Action<Uri, TArg>? uriAction = null,
                                      Action<string, TArg>? stringAction = null) => stringAction?.Invoke(String, arg);

    public override bool Equals(ContactID? other) => StringComparer.Ordinal.Equals(String, other?.String);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(String);

    public override string ToString() => String.Length == 0 ? "<Empty>" : $"String: {String}";
}
