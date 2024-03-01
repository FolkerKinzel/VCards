using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

//[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct DataBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal DataBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly 
    /// initialized using the binary content 
    /// of a file, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="filePath">Path to the file whose content is to embed.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file content
    /// or <c>null</c> to get the <paramref name="mimeType"/> automatically from the
    /// file type extension.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> the highest preference <c>(1)</c>
    /// and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the <see cref="ParameterSection"/> of the newly 
    /// created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called with the <see cref="VCardBuilder.VCard"/>
    /// instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder AddFile(string filePath,
                                string? mimeType = null,
                                bool pref = false,
                                Action<ParameterSection>? parameters = null,
                                Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromFile(filePath, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly 
    /// initialized to embed the content of an array of 
    /// <see cref="byte"/>s, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to embed or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the <paramref name="bytes"/>
    /// or <c>null</c> for <c>application/octet-stream</c>.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> the highest preference <c>(1)</c>
    /// and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the <see cref="ParameterSection"/> of the newly 
    /// created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called with the <see cref="VCardBuilder.VCard"/>
    /// instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder AddBytes(byte[]? bytes,
                                 string? mimeType = MimeString.OctetStream,
                                 bool pref = false,
                                 Action<ParameterSection>? parameters = null,
                                 Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromBytes(bytes, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly 
    /// initialized using the specified text, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="passWord">The text to embed or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the <paramref name="passWord"/>
    /// or <c>null</c>.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> the highest preference <c>(1)</c>
    /// and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the <see cref="ParameterSection"/> of the newly 
    /// created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called with the <see cref="VCardBuilder.VCard"/>
    /// instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/> to be able to chain calls.</returns>
    /// <remarks>
    /// The vCard standard only allows to write a password as plain text to the <c>KEY</c> property.
    /// <see cref="VCard.Keys">(See VCard.Keys.)</see>
    /// </remarks>
    /// <seealso cref="VCard.Keys"/>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder AddText(string? passWord,
                                string? mimeType = null,
                                bool pref = false,
                                Action<ParameterSection>? parameters = null,
                                Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromText(passWord, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DataProperty"/> instance, which is newly 
    /// initialized from an absolute <see cref="Uri"/> 
    /// that references external data, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="uri">An absolute <see cref="Uri"/> or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the 
    /// data the <paramref name="uri"/> points to, or <c>null</c>.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> the highest preference <c>(1)</c>
    /// and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the <see cref="ParameterSection"/> of the newly 
    /// created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called with the <see cref="VCardBuilder.VCard"/>
    /// instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is neither <c>null</c> nor
    /// an absolute <see cref="Uri"/>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder AddUri(Uri? uri,
                               string? mimeType = null,
                               bool pref = false, 
                               Action<ParameterSection>? parameters = null, 
                               Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(DataProperty.FromUri(uri, mimeType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    /// <summary>
    /// Sets the specified property of the <see cref="VCardBuilder.VCard"/> to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="DataProperty"/> objects that match a specified predicate from the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="DataProperty"/> objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<DataProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop, _builder.VCard.Get<IEnumerable<DataProperty?>?>(_prop).Remove(predicate));
        return _builder;
    }

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override int GetHashCode() => base.GetHashCode();

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override string ToString() => base.ToString()!;

}

