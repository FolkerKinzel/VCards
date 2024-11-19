using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing <see cref="VCard"/> properties that contain 
/// <see cref="DataProperty"/> instances.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this struct in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
/// <example>
/// <code language="cs" source="..\Examples\VCardExample.cs"/>
/// </example>
#if !(NET462 || NETSTANDARD2_0 || NETSTANDARD2_1)
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
#endif
public readonly struct RawDataBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal RawDataBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    /// <summary>
    /// Sets the <see cref="ParameterSection.Preference"/> properties of 
    /// the items in the specified <see cref="VCard"/> property depending on their position
    /// in that collection and allows to specify whether to skip empty items in that process.
    /// (The first item gets the highest preference <c>1</c>.)
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to give empty <see cref="VCardProperty"/> 
    /// objects always the lowest <see cref="ParameterSection.Preference"/> (100), independently
    /// of their position in the collection, or <c>false</c> to treat empty <see cref="VCardProperty"/> 
    /// objects like any other. (<c>null</c> references are always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetPreferences(bool skipEmptyItems = true) =>
        Edit(static (props, skip) =>
        {
            props.SetPreferences(skip);
            return props;
        }, skipEmptyItems);

    /// <summary>
    /// Resets the <see cref="ParameterSection.Preference"/> properties of 
    /// the items in in the specified <see cref="VCard"/> property to the lowest value (100).
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetPreferences() =>
        Edit(static props =>
        {
            props.UnsetPreferences();
            return props;
        });

    /// <summary>
    /// Sets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the specified <see cref="VCard"/> property ascending depending on their 
    /// position in that collection and allows to specify whether to skip empty items in that 
    /// process.
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to reset the <see cref="ParameterSection.Index"/> 
    /// of empty <see cref="VCardProperty"/> objects to <c>null</c>, or <c>false</c> to treat 
    /// empty <see cref="VCardProperty"/> objects like any other. (<c>null</c> references are 
    /// always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetIndexes(bool skipEmptyItems = true) =>
        Edit(static (props, skip) =>
        {
            props.SetIndexes(skip);
            return props;
        }, skipEmptyItems);

    /// <summary>
    /// Resets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the specified <see cref="VCard"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetIndexes() =>
        Edit(static props =>
        {
            props.UnsetIndexes();
            return props;
        });

    /// <summary>
    /// Edits the content of the specified <see cref="VCard"/> property with a delegate and 
    /// allows to pass an argument to this delegate.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// specified <see cref="VCard"/> property and <paramref name="arg"/> as arguments. Its return value 
    /// will be the new content of the specified property.</param>
    /// <param name="arg">The argument to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TArg>(Func<IEnumerable<DataProperty>, TArg, IEnumerable<DataProperty?>?> func,
                                    TArg arg)
    {
        IEnumerable<DataProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func(props, arg));
        return _builder;
    }

    /// <summary>
    /// Edits the content of the specified <see cref="VCard"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the specified <see cref="VCard"/> property
    /// as argument.
    /// Its return value will be the new content of the specified <see cref="VCard"/> property.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<DataProperty>, IEnumerable<DataProperty?>?> func)
    {
        IEnumerable<DataProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func(props));
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<DataProperty> GetProperty() =>
        Builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop)?.OfType<DataProperty>() ?? [];

    private void SetProperty(IEnumerable<DataProperty?>? value)
    {
        Debug.Assert(_builder != null);
        _builder.VCard.Set(_prop, value);
    }

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly 
    /// initialized using the binary content 
    /// of a file, to the specified <see cref="VCard"/> property.
    /// </summary>
    /// <param name="filePath">Path to the file whose content is to embed, or <c>null</c>.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the file content
    /// or <c>null</c> to get the <paramref name="mediaType"/> automatically from the
    /// file type extension.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// If <paramref name="filePath"/> is <c>null</c>, invalid or references an empty file, or if the file could not be loaded,
    /// an empty <see cref="DataProperty"/> instance will be created.
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been 
    /// initialized using the default constructor.</exception>
    public VCardBuilder AddFile(string? filePath,
                                string? mediaType = null,
                                Action<ParameterSection>? parameters = null,
                                Func<VCard, string?>? group = null)
    {
        if(filePath is null)
        {
            return AddRawData(RawData.FromBytes([], mediaType ?? MimeString.OctetStream), parameters, group);
        }

        try
        {
            return AddRawData(RawData.FromFile(filePath, mediaType), parameters, group);
        }
        catch
        {
            return AddRawData(RawData.FromBytes([], mediaType ?? MimeString.OctetStream), parameters, group);
        }
    }

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly initialized to embed the 
    /// content of an array of <see cref="byte"/>s, to the specified property of the 
    /// <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to embed, or <c>null</c>.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the <paramref name="bytes"/>,
    /// or <c>null</c> for <c>application/octet-stream</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called 
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> to be 
    /// able to chain calls.</returns>
    /// 
    /// <remarks>
    /// If <paramref name="bytes"/> is <c>null</c> or an empty array, an empty <see cref="DataProperty"/> instance
    /// will be created.
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been 
    /// initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder AddBytes(byte[]? bytes,
                                 string? mediaType = null,
                                 Action<ParameterSection>? parameters = null,
                                 Func<VCard, string?>? group = null)
        => AddRawData(RawData.FromBytes(bytes ?? [], mediaType ?? MimeString.OctetStream), parameters, group);

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly initialized using the specified text,
    /// to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="text">The text to embed, or <c>null</c>.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the <paramref name="text"/>,
    /// or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> 
    /// to be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// The vCard standard only allows to write a password as plain text to the <c>KEY</c> property.
    /// <see cref="VCard.Keys">(See VCard.Keys.)</see>
    /// </para>
    /// </remarks>
    /// 
    /// <seealso cref="VCard.Keys"/>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been 
    /// initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder AddText(string? text,
                                string? mediaType = null,
                                Action<ParameterSection>? parameters = null,
                                Func<VCard, string?>? group = null)
        => AddRawData(RawData.FromText(text ?? "", mediaType), parameters, group);

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly initialized from a
    /// <see cref="Uri"/> that references external data, to the specified property of the 
    /// <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="uri">A <see cref="Uri"/>, or <c>null</c>.</param>
    /// <param name="mediaType">The Internet Media Type ("MIME type") of the 
    /// data the <paramref name="uri"/> points to, or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> 
    /// to be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// If <paramref name="uri"/> is not an absolute <see cref="Uri"/> or if it is <c>null</c>, a <see cref="DataProperty"/> 
    /// instance containing a <see cref="RawData"/> object that encapsulates a <see cref="string"/> will be created.
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder AddUri(Uri? uri,
                               string? mediaType = null,
                               Action<ParameterSection>? parameters = null,
                               Func<VCard, string?>? group = null)
        => uri is null
            ? AddRawData(RawData.FromText("", mediaType), parameters, group)
            : uri.IsAbsoluteUri
                ? AddRawData(RawData.FromUri(uri, mediaType), parameters, group)
                : AddRawData(RawData.FromText(uri.ToString(), mediaType), parameters, group);

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly initialized with a
    /// <see cref="RawData"/> instance, to the specified property of the 
    /// <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="value">A <see cref="RawData"/> instance, or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> 
    /// to be able to chain calls.</returns>
    /// 
    /// <remarks>If <paramref name="value"/> is <c>null</c>,
    /// an empty <see cref="DataProperty"/> containing an empty <see cref="RawData"/> instance with an empty
    /// <see cref="string"/> will be created.</remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder AddRawData(RawData? value,
                                   Action<ParameterSection>? parameters = null,
                                   Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop,
                          VCardBuilder.Add(new DataProperty(value ?? RawData.FromText(""),
                                                            group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                           parameters)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the specified property of the <see cref="VCardBuilder.VCard"/> to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="DataProperty"/> objects that match a specified predicate from the specified 
    /// property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="DataProperty"/> objects 
    /// that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<DataProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop,
                          _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop)
                                        .Remove(predicate));
        return _builder;
    }

    // Overriding Equals, GetHashCode and ToString to hide these methods in IntelliSense:

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}

