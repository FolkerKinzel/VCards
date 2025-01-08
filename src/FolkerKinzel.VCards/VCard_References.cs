using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>
    /// Returns a collection of <see cref="VCard" /> objects containing both, the
    /// <see cref = "VCard" /> objects passed as a collection as well as those which
    /// had been embedded in their <see cref="VCard.Relations"/> property. The previously 
    /// embedded <see cref="VCard"/> objects are now referenced by <see cref = "RelationProperty" /> 
    /// objects that are initialized with the <see cref="Models.ContactID"/> instance of the <see cref="VCard.ContactID"/>
    /// property of these previously embedded <see cref="VCard"/>s.</summary>
    /// 
    /// <param name="vCards">A collection of <see cref="VCard" /> objects. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// 
    /// <returns> 
    /// A collection of <see cref="VCard" /> objects in which the <see cref="VCard"/> 
    /// objects previously embedded in the <see cref="VCard.Relations"/> property are appended 
    /// separately and referenced through their <see cref="VCard.ContactID"/> property. 
    /// (If the appended <see cref="VCard" /> objects did not already have a 
    /// <see cref="VCard.ContactID" /> property, the method automatically assigns them 
    /// a new one.)</returns>
    /// 
    /// <remarks>
    /// 
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
    /// The example uses the extension method <see cref="Extensions.IEnumerableExtension.Reference"
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
        _ArgumentNullException.ThrowIfNull(vCards, nameof(vCards));

        var list = vCards.OfType<VCard>().ToList();
        ReferenceIntl(list);
        return list;
    }

    /// <summary>
    /// After the method returns <paramref name="vCards"/> contains both, the
    /// <see cref = "VCard" /> objects passed as argument as well as those which
    /// had been embedded in their <see cref="VCard.Relations"/> or <see cref="VCard.Members"/> properties.
    /// The previously 
    /// embedded <see cref="VCard"/> objects are now referenced by <see cref = "RelationProperty" /> 
    /// objects that are initialized with the <see cref="Models.ContactID"/> instance of the <see cref="VCard.ContactID"/>
    /// property of these previously embedded <see cref="VCard"/>s.
    /// </summary>
    /// <param name="vCards">A <see cref="List{T}"/> of <see cref="VCard"/> instances to process.</param>
    internal static void ReferenceIntl(List<VCard> vCards)
    {
        for (int i = vCards.Count - 1; i >= 0; i--)
        {
            VCard vcard = vCards[i];

            if (HasRelationVCardProperty(vcard.Relations) || HasRelationVCardProperty(vcard.Members))
            {
                if (vcard.Members is not null)
                {
                    var members = vcard.Members.ToList();
                    vcard.Members = members;

                    DoSetReferences(vCards, members);
                }

                if (vcard.Relations is not null)
                {
                    var relations = vcard.Relations.ToList();
                    vcard.Relations = relations;

                    DoSetReferences(vCards, relations);
                }
            }

            static bool HasRelationVCardProperty(IEnumerable<RelationProperty?>? props)
                => props?.Any(static x => x?.Value.VCard is not null) ?? false;
        }

        static void DoSetReferences(List<VCard> vCardList, List<RelationProperty?> relations)
        {
            Debug.Assert(relations.OfType<RelationProperty>().Where(x => x.Value.VCard is not null).All(x => !x.IsEmpty));

            RelationProperty[] vcdProps = relations
                            .WhereNotNullAnd(static x => x.Value.VCard is not null)
                            .ToArray(); // We need ToArray here because relations
                                        // might change.

            foreach (RelationProperty vcdProp in vcdProps)
            {
                _ = relations.Remove(vcdProp);

                Debug.Assert(vcdProp.Value.VCard is not null);

                VCard vc = vcdProp.Value.VCard;
                ContactID? id = vc.ContactID?.Value;

                if (id is null || id.IsEmpty)
                {
                    id = Models.ContactID.Create();
                    vc.ContactID = new ContactIDProperty(id);
                }

                // Use reference comparison because several versions of a VCard with
                // the same VCard.ID can be in vCardList
                if (!vCardList.Contains(vc))
                {
                    vCardList.Add(vc);
                }

                if (relations.Any(x => x?.Value.ContactID == id
                                       && x.Parameters.RelationType == vcdProp.Parameters.RelationType))
                {
                    continue;
                }

                var relationUuid = new RelationProperty(
                    Relation.Create(id),
                    group: vcdProp.Group);

                relationUuid.Parameters.Assign(vcdProp.Parameters);
                relations.Add(relationUuid);
            }
        }
    }

    /// <summary>
    /// Replaces the <see cref="RelationProperty"/> instances that refer external vCards with their 
    /// <see cref="Models.ContactID"/> values by <see cref="RelationProperty"/> instances that contain 
    /// these <see cref="VCard"/> instances directly, provided that <paramref name="vCards"/> 
    /// contains these <see cref="VCard"/> instances.</summary>
    /// <param name="vCards">A collection of <see cref="VCard" /> objects. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// <returns> 
    ///  A collection of <see cref="VCard" /> objects in which the <see cref="VCard"/>s 
    /// referenced by their <see cref="VCard.ContactID"/> property are embedded in 
    /// <see cref ="RelationProperty"/> objects, provided that <paramref name="vCards"/>
    /// contains these <see cref="VCard"/> objects.
    /// </returns>
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// 
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
    /// <see cref="Extensions.IEnumerableExtension.Dereference(IEnumerable{VCard?})" />, 
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
    public static void Dereference(IEnumerable<VCard?> vCards)
    {
        VCard[] arr = vCards?
                  .OfType<VCard>()
                  .ToArray() ?? throw new ArgumentNullException(nameof(vCards));

        DereferenceIntl(arr);
    }

    internal static void DereferenceIntl(ReadOnlySpan<VCard> vCards)
    {
        foreach (VCard vc in vCards)
        {
            if (vc.Relations is not null)
            {
                var relations = vc.Relations.ToList();
                vc.Relations = relations;
                DoDereference(relations, vCards);
            }

            if (vc.Members is not null)
            {
                var members = vc.Members.ToList();
                vc.Members = members;
                DoDereference(members, vCards);
            }
        }
    }

    private static void DoDereference(List<RelationProperty?> relations, ReadOnlySpan<VCard> vCards)
    {
        ReadOnlySpan<RelationProperty> idProps = relations
            .Items()
            .Where(x => x.Value.ContactID is not null)
            .ToArray(); // We need ToArray here because relations
                        // might change.

        foreach (RelationProperty idProp in idProps)
        {
            Debug.Assert(idProp.Value.ContactID is not null);

            if (TryFindReferencedVCard(vCards, idProp.Value.ContactID, out VCard? referencedVCard))
            {
                if (relations.Any(x => x?.Value.VCard is VCard vc
                                       && vc.ContactID == referencedVCard.ContactID
                                       && x.Parameters.RelationType == idProp.Parameters.RelationType))
                {
                    continue;
                }

                var vcardProp = new RelationProperty(
                                    Relation.Create(referencedVCard),
                                    group: idProp.Group);
                vcardProp.Parameters.Assign(idProp.Parameters);

                _ = relations.Remove(idProp);
                relations.Add(vcardProp);
            }
        }//foreach

        static bool TryFindReferencedVCard(ReadOnlySpan<VCard> vCards, ContactID id, [NotNullWhen(true)] out VCard? referencedVCard)
        {
            foreach (VCard vCard in vCards)
            {
                if (vCard.ContactID?.Value == id)
                {
                    referencedVCard = vCard;
                    return true;
                }
            }

            referencedVCard = null;
            return false;
        }
    }
}
