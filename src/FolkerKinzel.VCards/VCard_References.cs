using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Gibt eine Sammlung von <see cref="VCard"/>-Objekten zurück, in der die <see cref="RelationVCardProperty"/>-Objekte 
        /// der als Argument übergebenen
        /// Sammlung von <see cref="VCard"/>-Objekten durch 
        /// <see cref="RelationUuidProperty"/>-Objekte ersetzt sind und in der die in den
        /// <see cref="RelationVCardProperty"/>-Objekten referenzierten <see cref="VCard"/>-Objekte als 
        /// separate Elemente angefügt sind.
        /// </summary>
        /// 
        /// <param name="vCards">Sammlung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein und <c>null</c>-Werte
        /// enthalten.</param>
        /// 
        /// <returns>
        /// Eine Sammlung von <see cref="VCard"/>-Objekten, in der die <see cref="RelationVCardProperty"/>-Objekte 
        /// der als Argument übergebenen
        /// Sammlung von <see cref="VCard"/>-Objekten durch 
        /// <see cref="RelationUuidProperty"/>-Objekte ersetzt sind und in der die in den
        /// <see cref="RelationVCardProperty"/>-Objekten referenzierten <see cref="VCard"/>-Objekte als 
        /// separate Elemente angefügt sind.
        /// </returns>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// <note type="important">
        /// Verwenden Sie diese Methode niemals, wenn Sie eine VCF-Datei als vCard 2.1 oder vCard 3.0 speichern möchten. Es droht Datenverlust.
        /// </note>
        /// <note type="tip">
        /// Sie können der Methode auch ein einzelnes <see cref="VCard"/>-Objekt übergeben, da die <see cref="VCard"/>-Klasse
        /// <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see> explizit implementiert.
        /// </note>
        /// <para>
        /// Die Methode wird bei Bedarf von den Serialisierungsmethoden von <see cref="VCard"/> automatisch verwendet. Die Verwendung in eigenem 
        /// Code ist
        /// nur dann sinnvoll, wenn ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden soll und wenn dabei jede VCF-Datei nur
        /// eine einzige vCard enthalten soll. (Dieses Vorgehen ist i.d.R. nicht vorteilhaft, da es die referentielle Integrität gefährdet.)
        /// </para>
        /// 
        /// 
        /// 
        /// <para>Wenn die angefügten <see cref="VCard"/>-Objekte noch keine <see cref="VCard.UniqueIdentifier"/>-Eigenschaft hatten, 
        /// wird ihnen 
        /// von der Methode
        /// automatisch eine neue zugewiesen.
        /// </para>
        /// </remarks>
        /// 
        /// <example>
        /// <para>
        /// Das Beispiel demonstriert, wie ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden kann, wenn beabsichtigt ist,
        /// dass eine VCF-Datei jeweils nur eine einzige vCard enthalten soll. Das Beispiel zeigt möglicherweise auch, dass dieses Vorgehen i.d.R.
        /// nicht vorteilhaft ist, da es die referentielle Integrität gefährdet.
        /// </para>
        /// <para>In dem Beispiel wird die Erweiterungsmethode <see cref="VCardCollectionExtension.ReferenceVCards"/> verwendet, die 
        /// <see cref="Reference(List{VCard})"/>
        /// aufruft.</para>
        /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
        /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
        /// </example>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCards"/> ist <c>null</c>.</exception>
        public static IEnumerable<VCard> Reference(IEnumerable<VCard?> vCards)
        {
            if (vCards is null)
            {
                throw new ArgumentNullException(nameof(vCards));
            }

            List<VCard> list = vCards.Where(x => x is not null).ToList()!;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                VCard vcard = list[i];

                if (vcard.Members != null || vcard.Relations != null)
                {
                    vcard = vcard.Clone();
                    list[i] = vcard;

                    if (vcard.Members != null)
                    {
                        List<RelationProperty?> members = vcard.Members as List<RelationProperty?> ?? vcard.Members.ToList();
                        vcard.Members = members;

                        DoSetReferences(list, members);
                    }

                    if (vcard.Relations != null)
                    {
                        List<RelationProperty?> relations = vcard.Relations as List<RelationProperty?> ?? vcard.Relations.ToList();
                        vcard.Relations = relations;

                        DoSetReferences(list, relations);
                    }
                }
            }

            return list;

            static void DoSetReferences(List<VCard> vCardList, List<RelationProperty?> relations)
            {
                RelationVCardProperty[] vcdProps = relations
                                .Select(x => x as RelationVCardProperty)
                                .Where(x => x != null && x.IsEmpty)
                                .ToArray()!;


                foreach (RelationVCardProperty vcdProp in vcdProps)
                {
                    Debug.Assert(vcdProp != null);

                    _ = relations.Remove(vcdProp);

                    VCard vc = vcdProp.Value!;

                    if (!vCardList.Contains(vc))
                    {
                        vCardList.Add(vc);
                    }

                    if (vc.UniqueIdentifier is null || vc.UniqueIdentifier.IsEmpty)
                    {
                        vc.UniqueIdentifier = new UuidProperty();
                    }

                    if (relations.Any(x => x is RelationUuidProperty xUid
                    && xUid.Value == vc.UniqueIdentifier.Value
                    && xUid.Parameters.RelationType == vcdProp.Parameters.RelationType))
                    {
                        continue;
                    }

                    var relationUuid = new RelationUuidProperty(
                        vc.UniqueIdentifier.Value,
                        vcdProp.Parameters.RelationType,
                        propertyGroup: vcdProp.Group);

                    relationUuid.Parameters.Assign(vcdProp.Parameters);
                    relations.Add(relationUuid);
                }
            }
        }



        /// <summary>
        /// Gibt eine Sammlung von <see cref="VCard"/>-Objekten zurück, in der <see cref="RelationUuidProperty"/>-Objekte der als 
        /// Argument übergebenen Sammlung von
        /// <see cref="VCard"/>-Objekten durch
        /// <see cref="RelationVCardProperty"/>-Objekte ersetzt worden sind, falls sich die referenzierten <see cref="VCard"/>-Objekte
        /// in der als Argument übergebenen Sammlung befinden.
        /// </summary>
        /// 
        /// <param name="vCards">Auflistung von <see cref="VCard"/>-Objekten. Die Auflistung darf leer sein und <c>null</c>-Werte
        /// enthalten.</param>
        /// 
        /// <returns>
        /// Eine Sammlung von <see cref="VCard"/>-Objekten, in der <see cref="RelationUuidProperty"/>-Objekte der als 
        /// Argument übergebenen Sammlung von
        /// <see cref="VCard"/>-Objekten durch
        /// <see cref="RelationVCardProperty"/>-Objekte ersetzt worden sind, falls sich die referenzierten <see cref="VCard"/>-Objekte
        /// in der als Argument übergebenen Sammlung befinden.
        /// </returns>
        /// 
        /// <remarks>
        /// <note type="caution">
        /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen 
        /// <see cref="VCard"/>-Objekte nicht. Sperren Sie den lesenden und schreibenden Zugriff auf diese
        /// <see cref="VCard"/>-Objekte während der Ausführung dieser Methode!
        /// </note>
        /// <note type="tip">
        /// Sie können der Methode auch ein einzelnes <see cref="VCard"/>-Objekt übergeben, da die <see cref="VCard"/>-Klasse
        /// <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see> explizit implementiert.
        /// </note>
        /// 
        /// <para>Die Methode wird von den Deserialisierungsmethoden von <see cref="VCard"/> automatisch aufgerufen. Die Verwendung in 
        /// eigenem Code kann z.B. nützlich sein, wenn <see cref="VCard"/>-Objekte aus verschiedenen Quellen in einer gemeinsamen Liste 
        /// zusammengeführt werden, um ihre Daten durchsuchbar zu machen.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <example>
        /// <para>
        /// Das Beispiel zeigt das Deserialisieren und Auswerten einer VCF-Datei, deren Inhalt auf andere VCF-Dateien verweist. In dem 
        /// Beispiel wird die Erweiterungsmethode <see cref="VCardCollectionExtension.DereferenceVCards(List{VCard})"/> verwendet, 
        /// die <see cref="Dereference(List{VCard})"/> aufruft.
        /// </para>
        /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
        /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
        /// </example>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCards"/> ist <c>null</c>.</exception>
        public static IEnumerable<VCard> Dereference(IEnumerable<VCard?> vCards)
        {
            if (vCards is null)
            {
                throw new ArgumentNullException(nameof(vCards));
            }

            foreach (VCard? vcard in vCards)
            {
                if (vcard != null)
                {
                    if (vcard.Relations != null || vcard.Members != null)
                    {
                        var clone = vcard.Clone();
                        if (clone.Relations != null)
                        {
                            List<RelationProperty?> relations = clone.Relations.ToList();
                            clone.Relations = relations;
                            DoDereference(relations, vCards);
                        }

                        if (clone.Members != null)
                        {
                            List<RelationProperty?> members = clone.Members.ToList();
                            clone.Members = members;
                            DoDereference(members, vCards);
                        }

                        yield return clone;
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
                    .Where(x => x != null && !x.IsEmpty).ToArray()!;

                foreach (RelationUuidProperty guidProp in guidProps)
                {
                    VCard? referencedVCard =
                        vCards.Where(x => x?.UniqueIdentifier != null).FirstOrDefault(x => x!.UniqueIdentifier!.Value == guidProp.Value);

                    if (referencedVCard != null)
                    {
                        if (relations.Any(x => x is RelationVCardProperty xVc && xVc.Value == referencedVCard))
                        {
                            continue;
                        }

                        var vcardProp = new RelationVCardProperty(
                                            referencedVCard,
                                            guidProp.Parameters.RelationType,
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
