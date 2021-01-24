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
        /// <note type="important">
        /// Verwenden Sie diese Methode niemals, wenn Sie eine VCF-Datei als vCard 2.1 oder vCard 3.0 speichern möchten!
        /// </note>
        /// <para>
        /// Die Methode wird bei Bedarf von den Serialisierungsmethoden von <see cref="VCard"/> automatisch verwendet. Die Verwendung in eigenem 
        /// Code ist
        /// nur dann sinnvoll, wenn ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden soll und wenn dabei jede VCF-Datei nur
        /// eine einzige vCard enthalten soll. (Dieses Vorgehen ist i.d.R. nicht vorteilhaft, da es die referentielle Integrität gefährdet.)
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
        /// <example>
        /// <para>
        /// Das Beispiel demonstriert, wie ein <see cref="VCard"/>-Objekt als vCard 4.0 gespeichert werden kann, wenn beabsichtigt ist,
        /// dass eine VCF-Datei jeweils nur eine einzige vCard enthalten soll. Das Beispiel zeigt möglicherweise auch, dass dieses Vorgehen i.d.R.
        /// nicht vorteilhaft ist, da es die referentielle Integrität gefährdet.
        /// </para>
        /// <para>In dem Beispiel wird die Erweiterungsmethode <see cref="VCardListExtension.ReferenceVCards"/> verwendet, die <see cref="Reference(List{VCard})"/>
        /// aufruft.</para>
        /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
        /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
        /// </example>
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
                        else if (relations.Any(x => x is RelationUuidProperty xUid && xUid.Value == vc.UniqueIdentifier.Value))
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
        /// </remarks>
        /// 
        /// <example>
        /// <para>
        /// Das Beispiel zeigt das Deserialisieren und Auswerten einer VCF-Datei, deren Inhalt auf andere VCF-Dateien verweist. In dem 
        /// Beispiel wird die Erweiterungsmethode <see cref="VCardListExtension.DereferenceVCards(List{VCard})"/> verwendet, 
        /// die <see cref="Dereference(List{VCard})"/> aufruft.
        /// </para>
        /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
        /// <code language="cs" source="..\Examples\VCard40Example.cs"/>
        /// </example>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="vCardList"/> ist <c>null</c>.</exception>
        public static void Dereference(List<
#nullable disable
            VCard
#nullable restore
            > vCardList)
        {
            if (vCardList is null)
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
