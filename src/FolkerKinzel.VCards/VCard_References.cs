﻿using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>
    /// Returns a collection of <see cref="VCard" /> objects containing both the
    /// <see cref = "VCard" /> objects passed as a collection as well as those which
    /// had been embedded in their <see cref="VCard.Relations"/> property. The previously 
    /// embedded <see cref="VCard"/> objects are now referenced by <see cref = "RelationProperty" /> 
    /// objects that are initialized with the value of the <see cref="VCard.UniqueIdentifier"/>
    /// property of these previously embedded <see cref="VCard"/>s.</summary>
    /// 
    /// <param name="vCards">A collection of <see cref="VCard" /> objects. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// 
    /// <returns> 
    /// A collection of <see cref="VCard" /> objects in which the <see cref="VCard"/> 
    /// objects previously embedded in the <see cref="VCard.Relations"/> property are appended 
    /// separately and referenced through their <see cref="VCard.UniqueIdentifier"/> property. 
    /// (If the appended <see cref="VCard" /> objects did not already have a 
    /// <see cref="VCard.UniqueIdentifier" /> property, the method automatically assigns them 
    /// a new one.)</returns>
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <note type="important">
    /// Never use this method, if you want to save a VCF file as vCard&#160;2.1 or vCard&#160;3.0!
    /// </note>
    /// <note type="tip">
    /// You can pass a single <see cref="VCard" /> object to the method, since the <see
    /// cref="VCard" /> class has an explicit implementation of 
    /// <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see>.
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
    /// vCard&#160;4.0 if it is intended that a VCF file should only contain one single vCard.
    /// The example may also show that this approach is generally not advantageous,
    /// as it endangers referential integrity.
    /// </para>
    /// <para>
    /// The example uses the extension method <see cref="Extensions.IEnumerableExtension.ReferenceVCards"
    /// />, which calls <see cref="Reference(IEnumerable{VCard})" />.
    /// </para>
    /// <note type="note">
    /// For the sake of easier readability, exception handling has not been used in
    /// the example.
    /// </note>
    /// <code language="cs" source="..\Examples\VCard40Example.cs" />
    /// </example>
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.</exception>
    public static IEnumerable<VCard> Reference(IEnumerable<VCard?> vCards)
    {
        if(vCards is null)
        {
            throw new ArgumentNullException(nameof(vCards));
        }

        var list = vCards.WhereNotNull().ToList();
        ReferenceIntl(list);
        return list;
    }


    private static void ReferenceIntl(List<VCard> vCards)
    {
        for (int i = vCards.Count - 1; i >= 0; i--)
        {
            VCard vcard = vCards[i];

            if (vcard.Members != null || vcard.Relations != null)
            {
                vcard = (VCard)vcard.Clone();
                vCards[i] = vcard;

                if (vcard.Members != null)
                {
                    List<RelationProperty?> members = vcard.Members.ToList();
                    vcard.Members = members;

                    DoSetReferences(vCards, members);
                }

                if (vcard.Relations != null)
                {
                    List<RelationProperty?> relations = vcard.Relations.ToList();
                    vcard.Relations = relations;

                    DoSetReferences(vCards, relations);
                }
            }
        }

        static void DoSetReferences(List<VCard> vCardList, List<RelationProperty?> relations)
        {
            Debug.Assert(relations.Where(x => x is RelationVCardProperty).All(x => !x!.IsEmpty));

            IEnumerable<RelationVCardProperty> vcdProps = relations
                            .WhereNotNullAnd(static x => x is RelationVCardProperty)
                            .Cast<RelationVCardProperty>()
                            .ToArray(); // We need ToArray here because relations
                                        // might change.

            foreach (RelationVCardProperty vcdProp in vcdProps)
            {
                Debug.Assert(vcdProp != null);

                _ = relations.Remove(vcdProp);

                VCard vc = vcdProp.Value;

                if (vc.UniqueIdentifier is null || vc.UniqueIdentifier.IsEmpty)
                {
                    vc.UniqueIdentifier = new UuidProperty();
                }

                if (!vCardList.Any(c => vc.UniqueIdentifier == c.UniqueIdentifier))
                {
                    vCardList.Add(vc);
                }

                if (relations.Any(x => x is RelationUuidProperty xUid
                                       && xUid.Value == vc.UniqueIdentifier.Value
                                       && xUid.Parameters.Relation == vcdProp.Parameters.Relation))
                {
                    continue;
                }

                var relationUuid = new RelationUuidProperty(
                    vc.UniqueIdentifier.Value,
                    group: vcdProp.Group);

                relationUuid.Parameters.Assign(vcdProp.Parameters);
                relations.Add(relationUuid);
            }
        }
    }

    /// <summary> 
    /// Returns a collection of <see cref="VCard" /> objects in which the <see cref="VCard"/>s 
    /// referenced by their <see cref="VCard.UniqueIdentifier"/> property are embedded in 
    /// <see cref ="RelationProperty"/> objects, provided that <paramref name="vCards"/>
    /// contains these <see cref="VCard"/> objects.
    /// </summary>
    /// <param name="vCards">A collection of <see cref="VCard" /> objects. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// <returns> 
    ///  A collection of <see cref="VCard" /> objects in which the <see cref="VCard"/>s 
    /// referenced by their <see cref="VCard.UniqueIdentifier"/> property are embedded in 
    /// <see cref ="RelationProperty"/> objects, provided that <paramref name="vCards"/>
    /// contains these <see cref="VCard"/> objects.
    /// </returns>
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
    /// refers to other VCF files. The example uses the extension method 
    /// <see cref="Extensions.IEnumerableExtension.DereferenceVCards(IEnumerable{VCard?})" />, 
    /// which calls <see cref="Dereference(IEnumerable{VCard?})" />.
    /// </para>
    /// <note type="note">
    /// For the sake of easier readability, exception handling has not been used in
    /// the example.
    /// </note>
    /// <code language="cs" source="..\Examples\VCard40Example.cs" />
    /// </example>
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<VCard> Dereference(IEnumerable<VCard?> vCards)
        => vCards is null ? throw new ArgumentNullException(nameof(vCards)) 
                          : Dereference(vCards, true);


    private static IEnumerable<VCard> Dereference(IEnumerable<VCard?> vCards, bool clone)
    {
        Debug.Assert(vCards != null);

        foreach (VCard? vcard in vCards)
        {
            if (vcard != null)
            {
                if (vcard.Relations != null || vcard.Members != null)
                {
                    VCard vc = clone ? (VCard)vcard.Clone() : vcard;

                    if (vc.Relations != null)
                    {
                        List<RelationProperty?> relations = vc.Relations.ToList();
                        vc.Relations = relations;
                        DoDereference(relations, vCards);
                    }

                    if (vc.Members != null)
                    {
                        List<RelationProperty?> members = vc.Members.ToList();
                        vc.Members = members;
                        DoDereference(members, vCards);
                    }

                    yield return vc;
                }
                else
                {
                    yield return vcard;
                }
            }
        }

        static void DoDereference(List<RelationProperty?> relations, IEnumerable<VCard?> vCards)
        {
            IEnumerable<RelationUuidProperty> guidProps = relations
                .Select(x => x as RelationUuidProperty)
                .WhereNotEmpty()
                .ToArray(); // We need ToArray here because relations
                            // might change.

            foreach (RelationUuidProperty guidProp in guidProps)
            {
                VCard? referencedVCard =
                    vCards.Where(x => x?.UniqueIdentifier != null)
                          .FirstOrDefault(x => x!.UniqueIdentifier!.Value == guidProp.Value);

                if (referencedVCard != null)
                {
                    if (relations.Any(x => x is RelationVCardProperty xVc &&
                                           xVc.Value.UniqueIdentifier == referencedVCard.UniqueIdentifier))
                    {
                        continue;
                    }

                    // Use the constructor here in order NOT to clone referenced VCard
                    var vcardProp = new RelationVCardProperty(
                                        referencedVCard,
                                        guidProp.Parameters.Relation,
                                        group: guidProp.Group);
                    vcardProp.Parameters.Assign(guidProp.Parameters);

                    _ = relations.Remove(guidProp);
                    relations.Add(vcardProp);
                }
            }
        }
    }
}
