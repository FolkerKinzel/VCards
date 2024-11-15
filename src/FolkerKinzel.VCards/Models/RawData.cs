using System.ComponentModel;
using System.Security.Cryptography;
using FolkerKinzel.DataUrls;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Resources;
using OneOf;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates external data in a <see cref="VCard"/>.
/// This can be either an array of <see cref="byte"/>s,
/// an absolute <see cref="System.Uri"/>, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DataProperty"/>
public sealed class RawData
{
    internal RawData(object obj, string? mediaType)
    {
        Object = obj;
        MediaType = mediaType;
    }

    /// <summary>
    /// Creates a new <see cref="RawData"/> instance from the binary content of a file. </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the file content,
    /// or <c>null</c> to get the <paramref name="mediaType"/> automatically from the
    /// file type extension.</param>
    /// 
    /// <returns>The newly created <see cref="RawData"/> instance.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public static RawData FromFile(string filePath,
                                   string? mediaType = null)
    {
        byte[] bytes = LoadFile(filePath);

        return mediaType is null
                ? new(bytes, MimeString.FromFileName(filePath))
                : FromBytes(bytes, mediaType);
    }

    /// <summary>
    /// Creates a new <see cref="RawData"/> instance from an array of 
    /// <see cref="byte"/>s.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to embed.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the <paramref name="bytes"/>.
    /// </param>
    /// 
    /// <returns>The newly created <see cref="RawData"/> instance.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="bytes"/> or <paramref name="mediaType"/>
    /// is <c>null</c>.</exception>
    public static RawData FromBytes(byte[] bytes,
                                    string mediaType = MimeString.OctetStream)
    {
        _ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        _ArgumentNullException.ThrowIfNull(mediaType, nameof(mediaType));

        mediaType = mediaType.Equals(MimeString.OctetStream, StringComparison.Ordinal)
                      ? mediaType
                      : MimeTypeInfo.TryParse(mediaType, out MimeTypeInfo mimeInfo)
                            ? mimeInfo.ToString()
                            : MimeString.OctetStream;

        return new(bytes, mediaType);
    }

    /// <summary>
    /// Creates a new <see cref="RawData"/> instance from text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the <paramref name="text"/>,
    /// or <c>null</c>.</param>
    /// 
    /// <returns>The newly created <see cref="RawData"/> instance.</returns>
    /// <remarks>
    /// <para>
    /// The vCard standard only allows to write a password as plain text to the <c>KEY</c> property.
    /// <see cref="VCard.Keys">(See VCard.Keys.)</see>
    /// </para>
    /// 
    /// </remarks>
    /// <seealso cref="VCard.Keys"/>
    public static RawData FromText(string text,
                                   string? mediaType = null)
    {
        _ArgumentNullException.ThrowIfNull(text, nameof(text));

        mediaType =
            MimeTypeInfo.TryParse(mediaType, out MimeTypeInfo mimeInfo)
                           ? mimeInfo.ToString()
                           : null;

        return new(text, mediaType);
    }

    /// <summary>
    /// Creates a new <see cref="RawData"/> instance from an absolute <see cref="Uri"/>, 
    /// which references external data.
    /// </summary>
    /// <param name="uri">An absolute <see cref="Uri"/>.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the 
    /// data the <paramref name="uri"/> points to, or <c>null</c>.</param>
    /// 
    /// <returns>The newly created <see cref="RawData"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is not
    /// an absolute <see cref="Uri"/>.</exception>
    public static RawData FromUri(Uri uri,
                                  string? mediaType = null)
    {
        _ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        mediaType = MimeTypeInfo.TryParse(mediaType, out MimeTypeInfo mimeInfo)
                     ? mimeInfo.ToString()
                     : null;

        return uri.IsAbsoluteUri
                ? new(uri, mediaType)
                : throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)),
                                              nameof(uri));
    }

    internal static RawData FromDataUrlInfo(in DataUrlInfo dataUrlInfo)
    {
        string mediaType = dataUrlInfo.MimeType.ToString();
        Debug.Assert(mediaType.Length > 0);

        return dataUrlInfo.TryGetEmbeddedData(out OneOf<string, byte[]> data)
            ? data.IsT0 ? new(data.AsT0, mediaType)
                        : new(data.AsT1, mediaType)
            : new RawData(Array.Empty<byte>(), mediaType);
    }

    /// <summary>
    /// <c>true</c> if the instance doesn't contain any data, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => Object switch
        {
            byte[] bytes => bytes.Length == 0,
            string text => text.Length == 0,
            _ => false
        };

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

    /// <summary>Specifies the MIME type for the data.</summary>
    /// <value>Internet Media Type (&quot;MIME type&quot;) according to RFC&#160;2046.</value>
    /// <example><code language="none">text/plain</code></example>
    public string? MediaType { get; }

    /// <summary>
    /// Gets an appropriate file type extension.
    /// </summary>
    /// <returns>The file type extension.</returns>
    /// <remarks>
    /// If <see cref="Object"/> is a <see cref="Uri"/>, the file type extension is for
    /// the data the <see cref="System.Uri"/> references.
    /// </remarks>
    public string GetFileTypeExtension()
        => MediaType is not null
            ? MimeString.ToFileTypeExtension(MediaType)
            : Object is Uri uri
                ? UriConverter.GetFileTypeExtensionFromUri(uri)
                : ".txt";

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="bytesAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is an array of <see cref="byte"/>s, or <c>null</c>.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>, or <c>null</c>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>, or <c>null</c>.</param>
    /// 
    public void Switch(Action<byte[]>? bytesAction,
                       Action<Uri>? uriAction,
                       Action<string>? stringAction)
    {
        if (Object is byte[] bytes)
        {
            bytesAction?.Invoke(bytes);
        }
        else if (Object is Uri uri)
        {
            uriAction?.Invoke(uri);
        }
        else
        {
            stringAction?.Invoke((string)Object);
        }
    }

    ///// <summary>
    ///// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    ///// encapsulated value and allows to pass an argument to the delegates.
    ///// </summary>
    ///// <typeparam name="TArg">Generic type parameter.</typeparam>
    ///// <param name="bytesAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    ///// is an array of <see cref="byte"/>s, or <c>null</c>.</param>
    ///// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    ///// value is a <see cref="System.Uri"/>, or <c>null</c>.</param>
    ///// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    ///// value is a <see cref="string"/>, or <c>null</c>.</param>
    ///// <param name="arg">An argument to pass to the delegates.</param>
    //public void Switch<TArg>(Action<byte[], TArg>? bytesAction,
    //                   Action<Uri, TArg>? uriAction,
    //                   Action<string, TArg>? stringAction,
    //                   TArg arg)
    //{
    //    if (Object is byte[] bytes)
    //    {
    //        bytesAction?.Invoke(bytes, arg);
    //    }
    //    else if (Object is Uri uri)
    //    {
    //        uriAction?.Invoke(uri, arg);
    //    }
    //    else
    //    {
    //        stringAction?.Invoke((string)Object, arg);
    //    }
    //}

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
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public TResult Convert<TResult>(Func<byte[], TResult>? bytesFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc)
    {
        return Object switch
        {
            byte[] bytes => bytesFunc is null ? throw new ArgumentNullException(nameof(bytesFunc)) : bytesFunc(bytes),
            Uri uri => uriFunc is null ? throw new ArgumentNullException(nameof(uriFunc)) : uriFunc(uri),
            _ => stringFunc is null ? throw new ArgumentNullException(nameof(stringFunc)) : stringFunc((string)Object)
        };
    }

    /// <inheritdoc/>
    public override string ToString()
        => IsEmpty ? "<Empty>"
                   : Object is byte[] bytes
                        ? $"{bytes}: {bytes.Length} Bytes"
                        : Object.ToString() ?? string.Empty;

    /// <summary>
    /// Loads the file referenced by <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    [ExcludeFromCodeCoverage]
    private static byte[] LoadFile(string filePath)
    {
        try
        {
            return File.ReadAllBytes(filePath);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (NotSupportedException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (System.Security.SecurityException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (PathTooLongException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (Exception e)
        {
            throw new IOException(e.Message, e);
        }
    }

}

