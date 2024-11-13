using System.ComponentModel;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates external data in a <see cref="VCard"/>.
/// This can be either an array of <see cref="byte"/>s,
/// an absolute <see cref="System.Uri"/>, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DataProperty"/>
public sealed partial class RawData
{
    internal RawData(object obj) => Object = obj;

    /// <summary>
    /// Gets the encapsulated <see cref="byte"/> array,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public byte[]? Bytes => Object as byte[];

    /// <summary>
    /// Gets the encapsulated <see cref="System.Uri"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <value>An absolute <see cref="System.Uri"/> or <c>null</c>.</value>
    public Uri? Uri => Object as Uri;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => Object as string;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Object { get; }

    [Obsolete("Use Object instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public object Value => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="bytesAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is an array of <see cref="byte"/>s.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<byte[]> bytesAction,
                       Action<Uri> uriAction,
                       Action<string> stringAction)
    {
        if (Object is byte[] bytes)
        {
            if (bytesAction is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                bytesAction(bytes);
            }
        }
        else if (Object is Uri uri)
        {
            if (uriAction is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                uriAction(uri);
            }
        }
        else
        {
            if (stringAction is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                stringAction((string)Object);
            }
        }
    }

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="bytesFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is an array of <see cref="byte"/>s.</param>
    /// <param name="uriFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<byte[], TResult>? bytesFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc)
    {
        return Object switch
        {
            byte[] bytes => bytesFunc is null ? throw new InvalidOperationException() : bytesFunc(bytes),
            Uri uri => uriFunc is null ? throw new InvalidOperationException() : uriFunc(uri),
            _ => stringFunc is null ? throw new InvalidOperationException() : stringFunc((string)Object)
        };
    }

    /// <inheritdoc/>
    public override string ToString()
        => Object is byte[] bytes
            ? $"{bytes}: {bytes.Length} Bytes"
            : Object.ToString() ?? string.Empty;

}

