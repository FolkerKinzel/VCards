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

