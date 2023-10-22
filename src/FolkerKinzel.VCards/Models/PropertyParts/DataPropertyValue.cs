using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FolkerKinzel.Uris;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Encapsulates the value of a <see cref="DataProperty"/> object.
/// This can be either a <see cref="ReadOnlyCollection{T}">ReadOnlyCollection&lt;Byte&gt;</see>,
/// a <see cref="System.Uri"/>, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DataProperty"/>
public sealed partial class DataPropertyValue
{
    private readonly OneOf<ReadOnlyCollection<byte>, Uri, string> _oneOf;

    internal DataPropertyValue(OneOf<ReadOnlyCollection<byte>, Uri, string> oneOf)
        => _oneOf = oneOf;

    /// <summary>
    /// Gets the encapsulated <see cref="ReadOnlyCollection{T}">ReadOnlyCollection&lt;Byte&gt;</see>
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public ReadOnlyCollection<byte>? Bytes => IsBytes ? AsBytes : null;

    /// <summary>
    /// Gets the encapsulated <see cref="System.Uri"/>
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public Uri? Uri => IsUri ? AsUri : null;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => IsString ? AsString : null;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Value => this._oneOf.Value;

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="bytesAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is a <see cref="ReadOnlyCollection{T}">ReadOnlyCollection&lt;Byte&gt;</see>.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<ReadOnlyCollection<byte>> bytesAction,
                       Action<Uri> uriAction,
                       Action<string> stringAction)
        => _oneOf.Switch(bytesAction, uriAction, stringAction);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="bytesFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is a <see cref="ReadOnlyCollection{T}">ReadOnlyCollection&lt;Byte&gt;</see>.</param>
    /// <param name="uriFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<ReadOnlyCollection<byte>, TResult>? bytesFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc)
        => _oneOf.Match(bytesFunc, uriFunc, stringFunc);

    /// <inheritdoc/>
    public override string ToString()
     => Convert
        (
         bytes => $"{bytes}: {bytes.Count} Bytes",
         uri => _oneOf.ToString(),
         str => _oneOf.ToString()
        );

    private bool IsBytes => _oneOf.IsT0;

    private ReadOnlyCollection<byte> AsBytes => _oneOf.AsT0;

    private bool IsUri => _oneOf.IsT1;

    private Uri AsUri => _oneOf.AsT1;

    private bool IsString => _oneOf.IsT2;

    private string AsString => _oneOf.AsT2;
}

