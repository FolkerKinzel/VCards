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
        /// Ersetzt <see cref="RelationVCardProperty"/>-Objekte durch <see cref="RelationUuidProperty"/>-Objekte und fügt die 
        /// referenzierten <see cref="VCard"/>-Objekte als Elemente an <paramref name="vCardList"/> an.
        /// </summary>
        /// 
        /// <param name="vCardList">Auflistung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>
        /// Die Methode wird bei Bedarf von den Serialisierungsmethoden von <see cref="VCard"/> automatisch verwendet. Die Verwendung in eigenem Code kann z.B. 
        /// nützlich sein,
        /// wenn ein einzelnes <see cref="VCard"/>-Objekt aus einer Sammlung von <see cref="VCard"/>-Objekten als separate VCF-Datei gespeichert werden soll.
        /// </para>
        /// 
        /// <para>
        /// Auch durch mehrfachen Aufruf der Methode werden keine 
        /// Doubletten (im Sinne der mehrfachen Einfügung desselben <see cref="VCard"/>- oder <see cref="RelationUuidProperty"/>-Objekts) erzeugt.
        /// </para>
        /// 
        /// <para>Wenn die angefügten <see cref="VCard"/>-Objekte noch keine <see cref="VCard.UniqueIdentifier"/>-Eigenschaft hatten, wird ihnen von der Methode
        /// automatisch eine neue zugewiesen.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        public static void Reference(List<
#nullable disable
            VCard
#nullable restore
            > vCardList)
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

                    DoSetReferences(vCardList, members);
                }

                if (vcard.Relations != null)
                {
                    List<RelationProperty?> relations = vcard.Relations as List<RelationProperty?> ?? vcard.Relations.ToList();
                    vcard.Relations = relations;

                    DoSetReferences(vCardList, relations);
                }
            }


            static void DoSetReferences(List<VCard?> vCardList, List<RelationProperty?> relations)
            {
                RelationVCardProperty[] vcdProps = relations
                                .Select(x => x as RelationVCardProperty)
                                .Where(x => x != null)
                                .ToArray()!;


                foreach (RelationVCardProperty vcdProp in vcdProps)
                {
                    Debug.Assert(vcdProp != null);

                    _ = relations.Remove(vcdProp);

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
                        else if(relations.Any(x => x is RelationUuidProperty xUid && xUid.Value == vc.UniqueIdentifier.Value))
                        {
                            continue;
                        }

                        var relationUuid = new RelationUuidProperty(
                            vc.UniqueIdentifier.Value,
                            vcdProp.Parameters.RelationType ?? RelationTypes.Contact,
                            propertyGroup: vcdProp.Group);

                        relationUuid.Parameters.Assign(vcdProp.Parameters);
                        relations.Add(relationUuid);
                    }
                }
            }

        }


        
        /// <summary>
        /// Ersetzt <see cref="RelationUuidProperty"/>-Objekte durch
        /// <see cref="RelationVCardProperty"/>-Objekte - sofern sich die referenzierten <see cref="VCard"/>-Objekte
        /// in <paramref name="vCardList"/> befinden.
        /// </summary>
        /// 
        /// <param name="vCardList">Auflistung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein oder <c>null</c>-Werte
        /// enthalten.</param>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// 
        /// <para>Die Methode wird von den Deserialisierungsmethoden von <see cref="VCard"/> automatisch aufgerufen. Die Verwendung in 
        /// eigenem Code kann z.B. nützlich sein, wenn <see cref="VCard"/>-Objekte aus verschiedenen Quellen in einer gemeinsamen Liste 
        /// zusammengeführt werden, um ihre Daten durchsuchbar zu machen.
        /// </para>
        /// 
        /// <para>Die Methode entfernt keine Elemente aus <paramref name="vCardList"/> und erzeugt 
        /// auch bei mehrfachem Aufruf keine Doubletten (<see cref="RelationVCardProperty"/>-Objekte, die dasselbe <see cref="VCard"/>-Objekt 
        /// enthalten).
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        public static void Dereference(List<
#nullable disable
            VCard
#nullable restore
            > vCardList)
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
                        DoDereference(relations, vCardList);
                    }

                    if (vcard.Members != null)
                    {
                        List<RelationProperty?> members = vcard.Members as List<RelationProperty?> ?? vcard.Members.ToList();
                        vcard.Members = members;
                        DoDereference(members, vCardList);
                    }
                }
            }


            static void DoDereference(List<RelationProperty?> relations, List<VCard?> vCardList)
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
                        if(relations.Any(x => x is RelationVCardProperty xVc && xVc.Value == referencedVCard))
                        {
                            continue;
                        }

                        var vcardProp = new RelationVCardProperty(
                                            referencedVCard,
                                            guidProp.Parameters.RelationType ?? RelationTypes.Contact,
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
