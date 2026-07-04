using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class ContactIDString : ContactID
{
    internal ContactIDString(string text, ContactID? comparer)
    {
        _ArgumentNullException.ThrowIfNull(text, nameof(text));

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(string.Format(Res.Whitespace, nameof(text)));
        }

        String = text;
        Comparer = comparer ?? this;
    }

    internal ContactIDString()
    {
        String = "";
        Comparer = this;
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch(Action<Guid>? guidAction = null,
                                Action<Uri>? uriAction = null,
                                Action<string>? stringAction = null) => stringAction?.Invoke(String);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch<TArg>(TArg arg,
                                      Action<Guid, TArg>? guidAction = null,
                                      Action<Uri, TArg>? uriAction = null,
                                      Action<string, TArg>? stringAction = null) => stringAction?.Invoke(String, arg);

    public override bool Equals([NotNullWhen(true)] ContactID? other)
        => other is not null &&
            (ReferenceEquals(this, Comparer)
                ? other.Comparer is ContactIDString comparer && StringComparer.Ordinal.Equals(String, comparer.String)
                : Comparer.Equals(other.Comparer));
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(String);

    public override string ToString() => String.Length == 0 ? "<Empty>" : $"String: {String}";
}
