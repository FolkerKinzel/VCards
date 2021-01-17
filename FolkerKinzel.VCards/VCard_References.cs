using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Ersetzt bei den in <paramref name="vCardList"/> gespeicherten <see cref="VCard"/>-Objekten die <see cref="RelationVCardProperty"/>-Objekte 
        /// in den Eigenschaften <see cref="VCard.Members"/> und
        /// <see cref="VCard.Relations"/> durch <see cref="RelationUuidProperty"/>-Objekte und fügt die in den ersetzten <see cref="RelationVCardProperty"/>-Objekten
        /// gespeicherten <see cref="VCard"/>-Objekte als separate Items an <paramref name="vCardList"/> an, falls sie nicht schon in der Liste enthalten waren. Falls
        /// die zu referenzierenden <see cref="VCard"/>-Objekte noch keine <see cref="VCard.UniqueIdentifier"/>-Eigenschaft hatten, wird ihnen dabei automatisch
        /// eine zugewiesen.
        /// </summary>
        /// <param name="vCardList">Auflistung von <see cref="VCard"/>-Objekten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        public static void SetReferences(List<VCard?> vCardList)
        {
            if (vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }

            for (int i = 0; i < vCardList.Count; i++)
            {
                VCard? vcard = vCardList[i];

                if (vcard is null)
                {
                    continue;
                }

                if (vcard.Members != null)
                {
                    List<RelationProperty?> members = vcard.Members as List<RelationProperty?> ?? vcard.Members.ToList();
                    vcard.Members = members;

                    SetReferences(vCardList, members);
                }

                if (vcard.Relations != null)
                {
                    List<RelationProperty?> relations = vcard.Relations as List<RelationProperty?> ?? vcard.Relations.ToList();
                    vcard.Relations = relations;

                    SetReferences(vCardList, relations);
                }
            }


            static void SetReferences(List<VCard?> vCardList, List<RelationProperty?> members)
            {
                RelationVCardProperty[] vcdProps = members
                                .Select(x => x as RelationVCardProperty)
                                .Where(x => x != null)
                                .ToArray()!;


                foreach (RelationVCardProperty vcdProp in vcdProps)
                {
                    Debug.Assert(vcdProp != null);

                    _ = members.Remove(vcdProp);

                    VCard? vc = vcdProp.Value;

                    if (vc != null)
                    {
                        if (!vCardList.Contains(vc))
                        {
                            vCardList.Add(vc);
                        }

                        if (vc.UniqueIdentifier is null)
                        {
                            vc.UniqueIdentifier = new UuidProperty();
                        }


                        var relationUuid = new RelationUuidProperty(vc.UniqueIdentifier.Value, propertyGroup: vcdProp.Group);
                        relationUuid.Parameters.Assign(vcdProp.Parameters);
                        members.Add(relationUuid);
                    }
                }
            }

        }


        
        /// <summary>
        /// Ersetzt die <see cref="RelationUuidProperty"/>-Objekte der in 
        /// <paramref name="vCardList"/> enthaltenen <see cref="VCard"/>-Objekte durch 
        /// <see cref="RelationVCardProperty"/>-Objekte, die die <see cref="VCard"/>s enthalten,
        /// die durch die <see cref="Guid"/>s der <see cref="RelationUuidProperty"/>-Objekte 
        /// referenziert wurden. Das geschieht nur, wenn sich die referenzierten <see cref="VCard"/>-Objekte in
        /// <paramref name="vCardList"/> befinden.
        /// </summary>
        /// <param name="vCardList">Eine Liste mit <see cref="VCard"/>-Objekten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        public static void Dereference(List<VCard?> vCardList)
        {
            if(vCardList is null)
            {
                throw new ArgumentNullException(nameof(vCardList));
            }

            foreach (VCard? vcard in vCardList)
            {
                if (vcard != null)
                {
                    if (vcard.Relations != null)
                    {
                        List<RelationProperty?> relations = vcard.Relations as List<RelationProperty?> ?? vcard.Relations.ToList();
                        vcard.Relations = relations;
                        SetRelationReferences(relations, vCardList);
                    }

                    if (vcard.Members != null)
                    {
                        List<RelationProperty?> members = vcard.Members as List<RelationProperty?> ?? vcard.Members.ToList();
                        vcard.Members = members;
                        SetRelationReferences(members, vCardList);
                    }
                }
            }


            static void SetRelationReferences(List<RelationProperty?> relations, List<VCard?> vCardList)
            {
                IEnumerable<RelationUuidProperty> guidProps = relations
                    .Select(x => x as RelationUuidProperty)
                    .Where(x => x != null && !x.IsEmpty).ToArray()!;

                foreach (RelationUuidProperty guidProp in guidProps)
                {
                    VCard? referencedVCard =
                        vCardList.Where(x => x?.UniqueIdentifier != null).FirstOrDefault(x => x!.UniqueIdentifier!.Value == guidProp.Value);

                    if (referencedVCard != null)
                    {
                        var vcardProp = new RelationVCardProperty(
                                            referencedVCard,
                                            propertyGroup: guidProp.Group);
                        vcardProp.Parameters.Assign(guidProp.Parameters);

                        _ = relations.Remove(guidProp);
                        relations.Add(vcardProp);
                    }
                }
            }
        }



    }
}
