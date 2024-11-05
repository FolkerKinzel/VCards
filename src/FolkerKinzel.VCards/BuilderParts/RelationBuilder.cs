using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing <see cref="VCard"/> properties that contain 
/// <see cref="RelationProperty"/> instances.
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
public readonly struct RelationBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal RelationBuilder(VCardBuilder builder, Prop prop)
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
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
    /// allows to pass <paramref name="data"/> to this delegate.
    /// </summary>
    /// <typeparam name="TData">The type of <paramref name="data"/>.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// specified property and <paramref name="data"/> as arguments. Its return value 
    /// will be the new content of the specified property.</param>
    /// <param name="data">The data to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this 
    /// <see cref="RelationBuilder"/> to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TData>(
        Func<IEnumerable<RelationProperty>, TData, IEnumerable<RelationProperty?>?> func,
        TData data)
    {
        IEnumerable<RelationProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func(props, data));
        return _builder;
    }

    /// <summary>
    /// Edits the content of the specified <see cref="VCard"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the specified <see cref="VCard"/> property
    /// as argument.
    /// Its return value will be the new content of the specified <see cref="VCard"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<RelationProperty>, IEnumerable<RelationProperty?>?> func)
    {
        IEnumerable<RelationProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func(props));
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<RelationProperty> GetProperty() =>
        Builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop)?.WhereNotNull() ?? [];

    private void SetProperty(IEnumerable<RelationProperty?>? value)
    {
        Debug.Assert(_builder != null);
        _builder.VCard.Set(_prop, value);
    }

    /// <summary>
    /// Adds a <see cref="RelationProperty"/> instance, which is newly initialized using a 
    /// <see cref="Guid"/> that refers to the vCard of the person or organization via its 
    /// <see cref="VCard.ID"/> property (<c>UID</c>), to the specified property of the 
    /// <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="uuid">A <see cref="Guid"/> that refers to the vCard of the person
    /// or organization via its <see cref="VCard.ID"/> property (<c>UID</c>).</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization to whose vCard the <paramref name="uuid"/> refers. 
    /// The <see cref="ParameterSection.RelationType"/> property of the added instance will be
    /// set to this value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Add(Guid uuid,
                            Rel? relationType = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Add(Relation.Create(ContactID.Create(uuid)), relationType, parameters, group);

    /// <summary>
    /// Adds a <see cref="RelationProperty"/> instance, which is newly 
    /// initialized using text that represents a person or organization, to the specified property 
    /// of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="text">Text that represents a person or organization, e.g., the name
    /// of the person or organization, or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the person 
    /// or organization that the <paramref name="text"/> names.
    /// The <see cref="ParameterSection.RelationType"/> property of the added instance will be set to this 
    /// value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Add(string? text,
                            Rel? relationType = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Add(string.IsNullOrWhiteSpace(text) 
               ? Relation.Empty 
               : Relation.Create(ContactID.Create(text)), 
                relationType, parameters, group);

    /// <summary>
    /// Adds a <see cref="RelationProperty"/> instance, which is newly initialized using a 
    /// <see cref="Uri"/> that represents a person or organization, to the specified property 
    /// of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="uri">An absolute <see cref="Uri"/> or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the person 
    /// or organization that the <paramref name="uri"/> represents.
    /// The <see cref="ParameterSection.RelationType"/> property of the added instance will be set to this 
    /// value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Add(Uri? uri,
                            Rel? relationType = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Add(Relation.Create(IDBuilder.ContactIDFromUri(uri)), relationType, parameters, group);

    /// <summary>
    /// Adds a <see cref="RelationProperty"/> instance, which is newly initialized using the 
    /// <see cref="VCard"/>-object that represents a person or organization, to the specified 
    /// property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="vCard">The <see cref="VCard"/>-object that represents a person or organization
    /// to whom there is a relationship, or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the person 
    /// or organization that the <paramref name="vCard"/> represents.
    /// The <see cref="ParameterSection.RelationType"/> property of the added instance will be set to this 
    /// value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
    /// to be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// <note type="important">
    /// This method clones <paramref name="vCard"/> in order to avoid circular references.
    /// Changing the <paramref name="vCard"/> instance AFTER assigning it to this constructor 
    /// leads to unexpected results!
    /// </note>
    /// <para>
    /// vCard&#160;2.1 and vCard&#160;3.0 can embed nested vCards if the flag <see cref="Rel.Agent"/> is 
    /// set in their <see cref="ParameterSection.RelationType"/> property. When serializing a vCard&#160;4.0, 
    /// embedded <see cref="VCard"/>s will be automatically replaced by their <see cref="VCards.VCard.ID"/>
    /// references and appended as separate vCards to the VCF file.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\EmbeddedVCardExample.cs" />
    /// </example>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Add(VCard? vCard,
                            Rel? relationType = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Add(vCard is null ? Relation.Empty : Relation.Create(vCard), relationType, parameters, group);
    

    /// <summary>
    /// Adds a <see cref="RelationProperty"/> instance, which is newly initialized using a 
    /// <see cref="ContactID"/> that represents a person or organization, to the specified 
    /// property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="id">The <see cref="ContactID"/> that represents a person or organization
    /// to whom there is a relationship, or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the person 
    /// or organization that the <paramref name="id"/> references.
    /// The <see cref="ParameterSection.RelationType"/> property of the added instance will be set to this 
    /// value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Add(ContactID? id,
                            Rel? relationType = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Add(Relation.Create(id ?? ContactID.Empty), relationType, parameters, group);

    /// <summary>
    /// Adds a <see cref="RelationProperty"/> instance, which is newly initialized using a 
    /// <see cref="Relation"/> instance that encapsulates the data that describes a person or organization 
    /// with whom a relationship exists, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="relation">The <see cref="Relation"/> that describes a person or organization
    /// with whom there is a relationship, or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the person 
    /// or organization that the <paramref name="relation"/> represents.
    /// The <see cref="ParameterSection.RelationType"/> property of the added instance will be set to this 
    /// value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(Relation? relation,
                            Rel? relationType = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        VCard vc = Builder.VCard;
        var relProp = new RelationProperty(relation ?? Relation.Empty, group?.Invoke(_builder.VCard));
        relProp.Parameters.RelationType = relationType;

        vc.Set(_prop,
                          VCardBuilder.Add(relProp,
                                           _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop),
                                           parameters)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the specified property of the <see cref="VCardBuilder.VCard"/> to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="RelationProperty"/> objects that match a specified predicate from the 
    /// specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="RelationProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RelationBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<RelationProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop,
                          _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop)
                                        .Remove(predicate)
                         );
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

