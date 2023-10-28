using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>Extension methods that supports working with <see cref="IEnumerable{T}"/>
/// objects.</summary>
public static class IEnumerableExtension
{
    /// <summary> 
    /// Returns a collection of <see cref="VCard" /> objects containing both the
    /// <see cref = "VCard" /> objects passed as a collection as well as those which
    /// had been embedded in their <see cref="VCard.Relations"/> property. The previously 
    /// embedded <see cref="VCard"/> objects are now referenced by <see cref = "RelationProperty" /> 
    /// objects that are initialized with the value of the <see cref="VCard.UniqueIdentifier"/>
    /// property of these previously embedded <see cref="VCard"/>s.
    /// </summary>
    /// <param name="vCards">A collection of <see cref="VCard" /> objects. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// <returns> A collection of <see cref="VCard" /> objects in which the <see cref="VCard"/> 
    /// objects previously embedded in the <see cref="VCard.Relations"/> property are appended 
    /// separately and referenced through their <see cref="VCard.UniqueIdentifier"/> property. 
    /// (If the appended <see cref="VCard" /> objects did not already have a 
    /// <see cref="VCard.UniqueIdentifier" /> property, the method automatically assigns them 
    /// a new one.)
    /// </returns>
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <note type="important">
    /// Never use this method, if you want to save a VCF file as vCard&#160;2.1 or vCard&#160;3.0!
    /// </note>
    /// <para>
    /// The method is - if necessary - automatically called by the serialization methods
    /// of <see cref="VCard" />. It only makes sense to use it in your own code, if
    /// a <see cref="VCard" /> object is to be saved as vCard&#160;4.0 and if each VCF file
    /// should only contain a single vCard. (As a rule, this approach is not advantageous
    /// as it endangers referential integrity.)
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The example demonstrates how a <see cref="VCard" /> object can be saved as a
    /// vCard&#160;4.0, if it is intended that a VCF file should only contain one single
    /// vCard. The example may also show that this approach is generally not advantageous,
    /// because it endangers referential integrity.
    /// </para>
    /// <note type="note">
    /// For the sake of easier readability, exception handling has not been used in
    /// the example.
    /// </note>
    /// <code language="cs" source="..\Examples\VCard40Example.cs" />
    /// </example>
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<VCard> ReferenceVCards(this IEnumerable<VCard?> vCards)
        => VCard.Reference(vCards);

    /// <summary>
    /// Returns a collection of <see cref="VCard" /> objects in which the <see cref="VCard"/>s 
    /// referenced by their <see cref="VCard.UniqueIdentifier"/> property are embedded in 
    /// <see cref ="RelationProperty"/> objects, provided that <paramref name="vCards"/> 
    /// contains these <see cref="VCard"/> objects.</summary>
    /// <param name="vCards">A collection of <see cref="VCard" /> objects. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// 
    /// <returns> A collection of <see cref="VCard" /> objects in which the <see cref="VCard"/>s 
    /// referenced by their <see cref="VCard.UniqueIdentifier"/> property are embedded in 
    /// <see cref ="RelationProperty"/> objects, provided that <paramref name="vCards"/>
    /// contains these <see cref="VCard"/> objects.</returns>
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method is automatically called by the deserialization methods of the <see
    /// cref="VCard" /> class. Using it in your own code can be useful, e.g., if <see
    /// cref="VCard" /> objects from different sources are combined in a common list
    /// in order to make their data searchable.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The example shows the deserialization and analysis of a VCF file whose content
    /// refers to other VCF files.
    /// </para>
    /// <note type="note">
    /// For the sake of easier readability, exception handling has not been used in
    /// the example.
    /// </note>
    /// <code language="cs" source="..\Examples\VCard40Example.cs" />
    /// </example>
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<VCard> DereferenceVCards(this IEnumerable<VCard?> vCards)
        => VCard.Dereference(vCards);

    /// <summary>Saves a collection of <see cref="VCard" /> objects in a common VCF
    /// file.</summary>
    /// <param name="vCards">The <see cref="VCard" /> objects to be saved. The collection
    /// may be empty or may contain <c>null</c> values. If the collection does not contain
    /// any <see cref="VCard" /> object, no file will be written.</param>
    /// <param name="fileName">The file path. If the file exists, it will be overwritten.</param>
    /// <param name="version">The vCard version of the VCF file to be written.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <param name="options">Options for writing the VCF file. The flags can be combined.</param>
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than the number of elements in the collection passed 
    /// to the <paramref name="vCards" /> parameter. This can happen if there are embedded 
    /// <see cref="VCard" /> objects in the <see cref="VCard.Members" /> or 
    /// <see cref="VCard.Relations" /> properties.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> or <paramref
    /// name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path or <paramref name="version" /> has an undefined value.</exception>
    /// <exception cref="IOException">The file could not be written.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SaveVcf(this IEnumerable<VCard?> vCards,
                               string fileName,
                               VCdVersion version = VCard.DEFAULT_VERSION,
                               ITimeZoneIDConverter? tzConverter = null,
                               VcfOptions options = VcfOptions.Default)
        => VCard.SaveVcf(fileName, vCards, version, tzConverter, options);

    /// <summary>Serializes a collection of <see cref="VCard" /> objects into a <see
    /// cref="Stream" /> using the VCF format.</summary>
    /// <param name="vCards">The <see cref="VCard" /> objects to be serialized. The
    /// collection may be empty or may contain <c>null</c> values.</param>
    /// <param name="stream">A <see cref="Stream" /> into which the serialized <see
    /// cref="VCard" /> objects are written.</param>
    /// <param name="version">The vCard version used for the serialization.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <param name="options">Options for serializing VCF. The flags can be combined.</param>
    /// <param name="leaveStreamOpen"> <c>true</c> means that the method does not close
    /// the underlying <see cref="Stream" />. The default value is <c>false</c>.</param>
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than the number of elements in the collection passed 
    /// to the <paramref name="vCards" /> parameter. This can happen if there are embedded 
    /// <see cref="VCard" /> objects in the <see cref="VCard.Members" /> or 
    /// <see cref="VCard.Relations" /> properties.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="stream" /> or <paramref
    /// name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="stream" /> does not support
    /// write operations or <paramref name="version" /> has an undefined value.</exception>
    /// <exception cref="IOException">I/O error.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="stream" /> was already
    /// closed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeVcf(this IEnumerable<VCard?> vCards,
                                    Stream stream,
                                    VCdVersion version = VCard.DEFAULT_VERSION,
                                    ITimeZoneIDConverter? tzConverter = null,
                                    VcfOptions options = VcfOptions.Default,
                                    bool leaveStreamOpen = false)
        => VCard.SerializeVcf(stream, vCards, version, tzConverter, options, leaveStreamOpen);

    /// <summary>Serializes <paramref name="vCards" /> as a <see cref="string" />, which
    /// represents the content of a VCF file.</summary>
    /// <param name="vCards">The <see cref="VCard" /> objects to be serialized. The
    /// collection may be empty or may contain <c>null</c> values.</param>
    /// <param name="version">The vCard version used for the serialization.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <param name="options">Options for serializing VCF. The flags can be combined.</param>
    /// <returns> <paramref name="vCards" />, serialized as a <see cref="string" />,
    /// which represents the content of a VCF file.</returns>
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than the number of elements in the collection passed 
    /// to the <paramref name="vCards" /> parameter. This can happen if there are embedded 
    /// <see cref="VCard" /> objects in the <see cref="VCard.Members" /> or 
    /// <see cref="VCard.Relations" /> properties.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="version" /> has an undefined
    /// value.</exception>
    /// <exception cref="OutOfMemoryException">The system is out of memory.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToVcfString(this IEnumerable<VCard?> vCards,
                                     VCdVersion version = VCard.DEFAULT_VERSION,
                                     ITimeZoneIDConverter? tzConverter = null,
                                     VcfOptions options = VcfOptions.Default)
        => VCard.ToVcfString(vCards, version, tzConverter, options);

    /// <summary>
    /// Gets the most preferred <see cref="VCardProperty"/> from a collection of
    /// <see cref="VCardProperty"/> objects and allows to specify whether or not
    /// to ignore empty items.
    /// </summary>
    /// <typeparam name="T">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to search. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// references.</param>
    /// <param name="ignoreEmptyItems"> Pass <c>false</c> to include empty items in the return value
    /// or <c>true</c> to ignore them. ("Empty" means, that <see cref="VCardProperty.IsEmpty"/> 
    /// returns <c>true</c>.) <c>null</c> values will always be ignored.</param>
    /// <returns>
    /// <para>
    /// The most preferred <see cref="VCardProperty"/> from <paramref name="values"/>,
    /// or <c>null</c> if <paramref name="values"/> is empty, contains only <c>null</c> references,
    /// or contains only empty items and <paramref name="ignoreEmptyItems"/> is <c>true</c>.
    /// </para>
    /// <para>"Most preferred" is defined as the <see cref="VCardProperty"/> with the lowest
    /// <see cref="ParameterSection.Preference"/> value.</para>
    /// </returns>
    /// <remarks>
    /// The method is especially useful to search <see cref="VCard"/> properties with plural names,
    /// but not those whose names end with <c>*views</c>. These properties should be searched with
    /// <see cref="FirstOrNull{TSource}(IEnumerable{TSource}?, bool)"/>
    /// because the <see cref="ParameterSection.Preference"/> has no meaning in such properties.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? PrefOrNull<T>(this IEnumerable<T?>? values,
                                   bool ignoreEmptyItems) where T : VCardProperty
        => values?.PrefOrNullIntl(ignoreEmptyItems);

    /// <summary>
    /// Gets the most preferred <see cref="VCardProperty"/> from a collection of
    /// <see cref="VCardProperty"/> objects and allows additional filtering of the items, and
    /// to specify whether or not to ignore empty items.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to search. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// references.</param>
    /// <param name="filter">A <see cref="Func{T, TResult}"/> which allows additional filtering 
    /// of the items, or <c>null</c> to not perform additional filtering. The arguments of the 
    /// delegate are guaranteed to not be <c>null</c>.</param>
    /// <param name="ignoreEmptyItems">Pass <c>false</c> to include empty items in the return value. 
    /// ("Empty" means that <see cref="VCardProperty.IsEmpty"/> returns <c>true</c>.) <c>null</c>
    /// values will always be ignored.</param>
    /// <returns>
    /// <para>
    /// The most preferred <see cref="VCardProperty"/> from <paramref name="values"/>
    /// that passes the <paramref name="filter"/>, or <c>null</c>
    /// </para>
    /// <list type="bullet">
    /// <item>if no such item could be found,</item>
    /// <item>if <paramref name="values"/> is empty,</item>
    /// <item>if <paramref name="values"/> contains only <c>null</c> references,</item>
    /// <item>or if <paramref name="values"/> contains only empty items and 
    /// <paramref name="ignoreEmptyItems"/> is <c>true</c>.</item>
    /// </list>
    /// <para>
    /// <para>"Most preferred" is defined as the <see cref="VCardProperty"/> with the lowest
    /// <see cref="ParameterSection.Preference"/> value.</para>
    /// </para>
    /// </returns>
    /// <remarks>
    /// The method is especially useful to search <see cref="VCard"/> properties with plural 
    /// names, but not those whose names end with <c>*views</c>. These properties should be 
    /// searched with <see cref="FirstOrNull{TSource}(IEnumerable{TSource}?, Func{TSource, bool}, bool)"/>
    /// because the <see cref="ParameterSection.Preference"/> has no meaning in such properties.
    /// </remarks>
    public static TSource? PrefOrNull<TSource>(
        this IEnumerable<TSource?>? values,
        Func<TSource, bool>? filter = null,
        bool ignoreEmptyItems = true) where TSource : VCardProperty
        => filter is null ? values?.PrefOrNullIntl(ignoreEmptyItems)
                          : values?.PrefOrNullIntl(filter, ignoreEmptyItems);

    /// <summary>
    /// Gets the first <see cref="VCardProperty"/> from a collection of <see cref="VCardProperty"/> 
    /// objects and allows to specify whether or not to ignore empty items.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to search. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// references.</param>
    /// <param name="ignoreEmptyItems">Pass <c>false</c> to include empty items in the return value
    /// or <c>true</c> to ignore them.
    /// ("Empty" means that <see cref="VCardProperty.IsEmpty"/> returns <c>true</c>.) <c>null</c>
    /// values will always be ignored.</param>
    /// <returns>
    /// <para>
    /// The first <see cref="VCardProperty"/> from <paramref name="values"/>,
    /// or <c>null</c> if <paramref name="values"/> is empty, contains only <c>null</c> references,
    /// or contains only empty items and <paramref name="ignoreEmptyItems"/> is <c>true</c>.
    /// </para>
    /// <para>
    /// "First" is defined as the item with the lowest <see cref="ParameterSection.Index"/>
    /// value. If <see cref="ParameterSection.Index"/> is <c>null</c>&#160;<see cref="int.MaxValue"/>
    /// is assumed.
    /// </para>
    /// </returns>
    /// <remarks>
    /// The method is especially useful to search <see cref="VCard"/> properties with names 
    /// that end with <c>*views</c>. For all other plural-named <see cref="VCard"/> properties use 
    /// <see cref="PrefOrNull{TSource}(IEnumerable{TSource}?, bool)"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource? FirstOrNull<TSource>(
        this IEnumerable<TSource?>? values, bool ignoreEmptyItems) where TSource : VCardProperty
        => values?.FirstOrNullIntl(ignoreEmptyItems);

    /// <summary>
    /// Gets the first <see cref="VCardProperty"/> from a collection of <see cref="VCardProperty"/>
    /// objects and allows filtering of the items, and to specify whether or not to ignore empty items.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to search. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// references.</param>
    /// <param name="filter">A <see cref="Func{T, TResult}"/> which allows additional filtering of
    /// the items, or <c>null</c> to not perform additional filtering. The arguments of the delegate 
    /// are guaranteed to not be <c>null</c>.</param>
    /// <param name="ignoreEmptyItems">Pass <c>false</c> to include empty items in the return value. 
    /// ("Empty" means that <see cref="VCardProperty.IsEmpty"/> returns <c>true</c>.) <c>null</c>
    /// values will always be ignored.</param>
    /// <returns>
    /// <para>
    /// The first <see cref="VCardProperty"/> from <paramref name="values"/> that passes the 
    /// <paramref name="filter"/>, or <c>null</c>
    /// </para>
    /// <list type="bullet">
    /// <item>if no such item could be found,</item>
    /// <item>if <paramref name="values"/> is empty,</item>
    /// <item>if <paramref name="values"/> contains only <c>null</c> references,</item>
    /// <item>or if <paramref name="values"/> contains only empty items and 
    /// <paramref name="ignoreEmptyItems"/> is <c>true</c>.</item>
    /// </list>
    /// <para>
    /// "First" is defined as the item with the lowest <see cref="ParameterSection.Index"/>
    /// value. If <see cref="ParameterSection.Index"/> is <c>null</c>&#160;<see cref="int.MaxValue"/>
    /// is assumed.
    /// </para>
    /// </returns>
    /// <remarks>
    /// The method is especially useful to search <see cref="VCard"/> properties with names that end 
    /// with <c>*views</c>. For all other plural-named <see cref="VCard"/> properties use 
    /// <see cref="PrefOrNull{TSource}(IEnumerable{TSource}?, Func{TSource, bool}, bool)"/>.
    /// </remarks>
    public static TSource? FirstOrNull<TSource>(
        this IEnumerable<TSource?>? values,
        Func<TSource, bool>? filter = null,
        bool ignoreEmptyItems = true) where TSource : VCardProperty
        => filter is null ? values?.FirstOrNullIntl(ignoreEmptyItems) 
                          : values?.FirstOrNullIntl(filter, ignoreEmptyItems);

    /// <summary>
    /// Sorts the elements in <paramref name="values"/> ascending by the value of their 
    /// <see cref="ParameterSection.Preference"/> property.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to sort. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// references.</param>
    /// <param name="discardEmptyItems">Pass <c>false</c> to include empty items in the return value. 
    /// ("Empty" means that <see cref="VCardProperty.IsEmpty"/> returns <c>true</c>.) <c>null</c>
    /// references are removed in any case.</param>
    /// <returns>A collection that contains the items of <paramref name="values"/> ordered ascending
    /// by the value of their <see cref="ParameterSection.Preference"/> property. The returned collection 
    /// won't contain <c>null</c> references. If <paramref name="values"/> is <c>null</c>
    /// an empty collection will be returned.</returns>
    /// <remarks>
    /// The method is useful to examine <see cref="VCard"/> properties with plural names but not
    /// those which end with <c>*views</c>. These properties should be sorted with
    /// <see cref="OrderByIndex{TSource}(IEnumerable{TSource}, bool)"/> because the 
    /// <see cref="ParameterSection.Preference"/> has no meaning in such properties.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TSource> OrderByPref<TSource>(
        this IEnumerable<TSource?>? values, bool discardEmptyItems = true) where TSource : VCardProperty
        => values is null ? Enumerable.Empty<TSource>()
                          : values.OrderByPrefIntl(discardEmptyItems);

    /// <summary>
    /// Sorts the elements in <paramref name="values"/> ascending by the value of their 
    /// <see cref="ParameterSection.Index"/> property.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to sort. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// values.</param>
    /// <param name="discardEmptyItems">Pass <c>false</c> to include empty items in the return value. 
    /// ("Empty" means that <see cref="VCardProperty.IsEmpty"/> returns <c>true</c>.) <c>null</c>
    /// references are removed in any case.</param>
    /// <returns>
    /// <para>
    /// A collection that contains the items of <paramref name="values"/> ordered ascending
    /// by the value of their <see cref="ParameterSection.Index"/> property. If the
    /// <see cref="ParameterSection.Index"/> property is <c>null</c>&#160;<see cref="int.MaxValue"/>
    /// is assumed.
    /// </para>
    /// <para>
    /// The returned collection 
    /// won't contain <c>null</c> references. If <paramref name="values"/> is <c>null</c>
    /// an empty collection will be returned. 
    /// </para>
    /// </returns>
    /// <remarks>
    /// The method is useful to examine <see cref="VCard"/> properties with names that ends with
    /// <c>*views</c>. For all other plural-named <see cref="VCard"/> properties use 
    /// <see cref="OrderByPref{TSource}(IEnumerable{TSource}?, bool)"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TSource> OrderByIndex<TSource>(
        this IEnumerable<TSource?>? values, bool discardEmptyItems = true) where TSource : VCardProperty
        => values is null ? Enumerable.Empty<TSource>()
                          : values.OrderByIndexIntl(discardEmptyItems);

    /// <summary>
    /// Groups the <see cref="VCardProperty"/> objects in <paramref name="values"/>
    /// by their <see cref="VCardProperty.Group"/> identifier.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to group. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// values.</param>
    /// <returns>
    /// <para>
    /// A collection of <see cref="IGrouping{TKey, TElement}"/> instances whose 
    /// <see cref="IGrouping{TKey, TElement}.Key"/>s are the <see cref="VCardProperty.Group"/> 
    /// identifiers. The Key <c>null</c> groups the <see cref="VCardProperty"/> instances that 
    /// don't belong to any group.
    /// </para>
    /// <para>
    /// The Values of the groups are guaranteed to be not <c>null</c>. If the method parameter 
    /// <paramref name="values"/> is <c>null</c> an empty collection is returned.
    /// </para>
    /// </returns>
    /// <remarks>
    /// The comparison of <see cref="VCardProperty.Group"/> identifiers is case-insensitive 
    /// (see RFC 6350, 3.3).
    /// </remarks>
    public static IEnumerable<IGrouping<string?, TSource>> GroupByVCardGroup<TSource>(
        this IEnumerable<TSource?>? values) where TSource : VCardProperty
        => values?.WhereNotNull()
                  .GroupBy(static x => x.Group, StringComparer.OrdinalIgnoreCase)
           ?? Enumerable.Empty<IGrouping<string?, TSource>>();

    /// <summary>
    /// Groups the <see cref="VCardProperty"/> objects in <paramref name="values"/>
    /// by their <see cref="ParameterSection.AltID"/>s.
    /// </summary>
    /// <typeparam name="TSource">Generic type parameter that's constrained to be a class that's 
    /// derived from <see cref="VCardProperty"/>.</typeparam>
    /// <param name="values">The <see cref="IEnumerable{T}"/> of <see cref="VCardProperty"/>
    /// objects to group. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// values.</param>
    /// <returns>
    /// <para>
    /// A collection of <see cref="IGrouping{TKey, TElement}"/> instances whose 
    /// <see cref="IGrouping{TKey, TElement}.Key"/>s are the <see cref="ParameterSection.AltID"/>s. A
    /// special group is that one with the Key <c>null</c>: This group collects all items that don't
    /// have any <see cref="ParameterSection.AltID"/>. 
    /// </para>
    /// <para>
    /// The Values of the groups are guaranteed to be not <c>null</c>. If the parameter 
    /// <paramref name="values"/> is <c>null</c> an empty collection is returned.
    /// </para>
    /// </returns>
    /// <remarks>
    /// The method performs an ordinal character comparison of the <see cref="ParameterSection.AltID"/>s.
    /// </remarks>
    public static IEnumerable<IGrouping<string?, TSource>> GroupByAltID<TSource>(
        this IEnumerable<TSource?>? values) where TSource : VCardProperty
        => values?.WhereNotNull()
                  .GroupBy(static x => x.Parameters.AltID, StringComparer.Ordinal)
           ?? Enumerable.Empty<IGrouping<string?, TSource>>();

    /// <summary>
    /// Generates a new value for the <see cref="ParameterSection.AltID"/> property that
    /// has not yet been used in <paramref name="values"/>.
    /// </summary>
    /// <param name="values">The collection of <see cref="VCardProperty"/> objects to
    /// examine. The collection may be <c>null</c>, empty, or may contain <c>null</c> 
    /// values.</param>
    /// <returns>A new, unused value for the <see cref="ParameterSection.AltID"/> properties
    /// of the objects in <paramref name="values"/>.</returns>
    public static string NewAltID(this IEnumerable<VCardProperty?>? values)
    {
        IEnumerable<string> numerable = values?.Select(x => x?.Parameters.AltID)
                                               .WhereNotNull()
                                               .Distinct()
                                         ?? Enumerable.Empty<string>();
        int i = -1;

        foreach (string altID in numerable)
        {
            if(int.TryParse(altID, out int result) && i < result)
            {
                i = result;
            }
        }

        return (++i).ToString();
    }


    public static TSource? FirstOrNullWithGroup<TSource>(
        this IEnumerable<TSource?>? values,
        string? groupID,
        bool ignoreEmptyItems = true) where TSource : VCardProperty 
        => values?.FirstOrNullIntl(x => StringComparer.OrdinalIgnoreCase.Equals(groupID, x.Group),
                                        ignoreEmptyItems);
}
