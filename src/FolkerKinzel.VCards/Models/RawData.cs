using FolkerKinzel.DataUrls;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// A union that encapsulates raw data in a <see cref="VCard"/>.
/// This can be either an array of <see cref="byte"/>s,
/// an absolute <see cref="System.Uri"/>, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DataProperty"/>
public sealed class RawData
{
    private readonly object _object;

    internal RawData(object obj, string? mediaType, bool isEmpty)
    {
        _object = obj;
        MediaType = mediaType;
        IsEmpty = isEmpty;
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
                ? new(bytes, MimeString.FromFileName(filePath), bytes.Length == 0)
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

        return new(bytes, mediaType, bytes.Length == 0);
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

        return new(text, mediaType, text.Length == 0);
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
                ? new(uri, mediaType, false)
                : throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)),
                                              nameof(uri));
    }

    internal static RawData FromDataUrlInfo(in DataUrlInfo dataUrlInfo)
    {
        string mediaType = dataUrlInfo.MimeType.ToString();
        Debug.Assert(mediaType.Length > 0);

        return dataUrlInfo.TryGetData(out EmbeddedData data)
            ? data.Convert<string, RawData>( 
                mediaType,
                static (bytes, mediaType) => new(bytes, mediaType, bytes.Length == 0),
                static (str, mediaType) => new(str, mediaType, str.Length == 0)
                                            )
            : new RawData(Array.Empty<byte>(), mediaType, true);
    }

    /// <summary>
    /// <c>true</c> if the instance doesn't contain any data, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="byte"/> array,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public byte[]? Bytes => _object as byte[];

    /// <summary>
    /// Gets the encapsulated <see cref="System.Uri"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <value>An absolute <see cref="System.Uri"/> or <c>null</c>.</value>
    public Uri? Uri => _object as Uri;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => _object as string;

    /// <summary>Specifies the MIME type for the data.</summary>
    /// <value>Internet Media Type (&quot;MIME type&quot;) according to RFC&#160;2046.</value>
    /// <example><code language="none">text/plain</code></example>
    public string? MediaType { get; }

    /// <summary>
    /// Gets an appropriate file type extension.
    /// </summary>
    /// <returns>The file type extension.</returns>
    /// <remarks>
    /// If <see cref="_object"/> is a <see cref="Uri"/>, the file type extension is for
    /// the data the <see cref="System.Uri"/> references.
    /// </remarks>
    public string GetFileTypeExtension()
        => MediaType is not null
            ? MimeString.ToFileTypeExtension(MediaType)
            : _object is Uri uri
                ? UriConverter.GetFileTypeExtensionFromUri(uri)
                : ".txt";

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="bytesAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated value
    /// is an array of <see cref="byte"/>s.</param>
    /// <param name="uriAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// 
    public void Switch(Action<byte[]>? bytesAction = null,
                       Action<Uri>? uriAction = null,
                       Action<string>? stringAction = null)
    {
        if (_object is byte[] bytes)
        {
            bytesAction?.Invoke(bytes);
        }
        else if (_object is Uri uri)
        {
            uriAction?.Invoke(uri);
        }
        else
        {
            stringAction?.Invoke((string)_object);
        }
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value and allows to pass an argument to the delegates.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="bytesAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated value
    /// is an array of <see cref="byte"/>s.</param>
    /// <param name="uriAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    public void Switch<TArg>(TArg arg,
                             Action<byte[], TArg>? bytesAction = null,
                             Action<Uri, TArg>? uriAction = null,
                             Action<string, TArg>? stringAction = null)
    {
        if (_object is byte[] bytes)
        {
            bytesAction?.Invoke(bytes, arg);
        }
        else if (_object is Uri uri)
        {
            uriAction?.Invoke(uri, arg);
        }
        else
        {
            stringAction?.Invoke((string)_object, arg);
        }
    }

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
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
    public TResult Convert<TResult>(Func<byte[], TResult> bytesFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc)
    {
        return _object switch
        {
            byte[] bytes => bytesFunc is null ? throw new ArgumentNullException(nameof(bytesFunc)) : bytesFunc(bytes),
            Uri uri => uriFunc is null ? throw new ArgumentNullException(nameof(uriFunc)) : uriFunc(uri),
            _ => stringFunc is null ? throw new ArgumentNullException(nameof(stringFunc)) : stringFunc((string)_object)
        };
    }

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/> and allows to specify an
    /// argument for the conversion.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
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
    public TResult Convert<TArg, TResult>(TArg arg,
                                          Func<byte[], TArg, TResult> bytesFunc,
                                          Func<Uri, TArg, TResult> uriFunc,
                                          Func<string, TArg, TResult> stringFunc)
    {
        return _object switch
        {
            byte[] bytes => bytesFunc is null ? throw new ArgumentNullException(nameof(bytesFunc)) : bytesFunc(bytes, arg),
            Uri uri => uriFunc is null ? throw new ArgumentNullException(nameof(uriFunc)) : uriFunc(uri, arg),
            _ => stringFunc is null ? throw new ArgumentNullException(nameof(stringFunc)) : stringFunc((string)_object, arg)
        };
    }

    /// <inheritdoc/>
    public override string ToString()
        => _object is byte[] bytes
              ? $"{bytes}: {bytes.Length} Bytes"
              : _object.ToString() ?? string.Empty;

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

