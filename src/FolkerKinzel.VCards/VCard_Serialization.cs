using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    #region static Methods

    /// <summary>Saves a collection of <see cref="VCard" /> objects in a common VCF
    /// file.</summary>
    /// <param name="vCards">The <see cref="VCard" /> objects to be saved. The collection
    /// may be empty or may contain <c>null</c> values. If the collection does not contain
    /// any <see cref="VCard" /> object, no file will be written.</param>
    /// <param name="fileName">The file path. If the file exists, it will be overwritten.</param>
    /// <param name="version">The vCard version of the VCF file to be written.</param>
    /// <param name="options">Options for writing the VCF file. The flags can be combined.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <remarks>
    /// <note type="caution">
    /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen
    /// <see cref="VCard" />-Objekte nicht. Sperren Sie den lesenden und schreibenden
    /// Zugriff auf diese <see cref="VCard" />-Objekte während der Ausführung dieser
    /// Methode!
    /// </note>
    /// <note type="tip">
    /// Sie können der Methode auch ein einzelnes <see cref="VCard" />-Objekt übergeben,
    /// da die <see cref="VCard" />-Klasse <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see>
    /// explizit implementiert.
    /// </note>
    /// <para>
    /// Die Methode serialisiert möglicherweise mehr vCards, als die Anzahl der Elemente
    /// in der Sammlung, die an den Parameter <paramref name="vCards" /> übergeben wird.
    /// Dies geschieht, wenn eine VCF-Datei als vCard 4.0 gespeichert wird und sich
    /// in den Eigenschaften <see cref="VCard.Members" /> oder <see cref="VCard.Relations"
    /// /> eines <see cref="VCard" />-Objekts weitere <see cref="VCard" />-Objekte in
    /// Form von <see cref="RelationVCardProperty" />-Objekten befinden.
    /// </para>
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option
    /// <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> serialisiert wird und
    /// wenn sich in der Eigenschaft <see cref="VCard.Relations" /> eines <see cref="VCard"
    /// />-Objekts ein <see cref="RelationVCardProperty" />-Objekt befindet, auf dessen
    /// <see cref="ParameterSection" /> in der Eigenschaft <see cref="ParameterSection.Relation"
    /// /> das Flag <see cref="RelationTypes.Agent" /> gesetzt ist.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> or <paramref
    /// name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path or <paramref name="version" /> has an undefined value.</exception>
    /// <exception cref="IOException">The file could not be written.</exception>
    public static void SaveVcf(
        string fileName,
        IEnumerable<VCard?> vCards,
        VCdVersion version = DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOptions options = VcfOptions.Default)
    {
        if (vCards is null)
        {
            throw new ArgumentNullException(nameof(vCards));
        }

        // verhindert, dass eine leere Datei geschrieben wird
        if (!vCards.Any(x => x != null))
        {
            //File.Delete(fileName);
            return;
        }

        using FileStream stream = InitializeFileStream(fileName);
        SerializeVcf(stream, vCards, version, tzConverter, options, false);
    }


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
    /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen
    /// <see cref="VCard" />-Objekte nicht. Sperren Sie den lesenden und schreibenden
    /// Zugriff auf diese <see cref="VCard" />-Objekte während der Ausführung dieser
    /// Methode!
    /// </note>
    /// <note type="tip">
    /// Sie können der Methode auch ein einzelnes <see cref="VCard" />-Objekt übergeben,
    /// da die <see cref="VCard" />-Klasse <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see>
    /// explizit implementiert.
    /// </note>
    /// <para>
    /// Die Methode serialisiert möglicherweise mehr vCards, als die Anzahl der Elemente
    /// in der Sammlung, die an den Parameter <paramref name="vCards" /> übergeben wird.
    /// Dies geschieht, wenn eine vCard 4.0 serialisiert wird und sich in den Eigenschaften
    /// <see cref="VCard.Members" /> oder <see cref="VCard.Relations" /> eines <see
    /// cref="VCard" />-Objekts weitere <see cref="VCard" />-Objekte in Form von <see
    /// cref="RelationVCardProperty" />-Objekten befanden.
    /// </para>
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option
    /// <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> serialisiert wird und
    /// wenn sich in der Eigenschaft <see cref="VCard.Relations" /> eines <see cref="VCard"
    /// />-Objekts ein <see cref="RelationVCardProperty" />-Objekt befindet, auf dessen
    /// <see cref="ParameterSection" /> in der Eigenschaft <see cref="ParameterSection.Relation"
    /// /> das Flag <see cref="RelationTypes.Agent" /> gesetzt ist.
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
    public static void SerializeVcf(Stream stream,
                                    IEnumerable<VCard?> vCards,
                                    VCdVersion version = DEFAULT_VERSION,
                                    ITimeZoneIDConverter? tzConverter = null,
                                    VcfOptions options = VcfOptions.Default,
                                    bool leaveStreamOpen = false)
    {
        DebugWriter.WriteMethodHeader($"{nameof(VCard)}.{nameof(SerializeVcf)}({nameof(Stream)}, IEnumerable<{nameof(VCard)}?>, {nameof(VCdVersion)}, {nameof(VcfOptions)}");

        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (!stream.CanWrite)
        {
            if (!leaveStreamOpen)
            {
                stream.Close();
            }

            throw new ArgumentException(Res.StreamNotWritable, nameof(stream));
        }

        if (vCards is null)
        {
            if (!leaveStreamOpen)
            {
                stream.Close();
            }
            throw new ArgumentNullException(nameof(vCards));
        }

        if (version < VCdVersion.V4_0)
        {
            if (options.IsSet(VcfOptions.IncludeAgentAsSeparateVCard))
            {
                List<VCard?> list = vCards.ToList();
                vCards = list;
                for (int i = 0; i < list.Count; i++)
                {
                    VCard? vCard = list[i];

                    if (vCard?.Relations is null)
                    {
                        continue;
                    }

                    RelationVCardProperty? agent = vCard.Relations
                        .Select(x => x as RelationVCardProperty)
                        .Where(x => x != null && !x.IsEmpty && x.Parameters.Relation.IsSet(RelationTypes.Agent))
                        .OrderBy(x => x!.Parameters.Preference)
                        .FirstOrDefault();

                    if (agent != null)
                    {
                        if (!list.Contains(agent.Value))
                        {
                            list.Add(agent.Value);
                        }
                    }

                }//for
            }//if
        }
        else
        {
            vCards = Reference(vCards);
        }

        // UTF-8 muss ohne BOM geschrieben werden, da sonst nicht lesbar
        // (vCard 2.1 kann UTF-8 verwenden, da nur ASCII-Zeichen geschrieben werden)
        var encoding = new UTF8Encoding(false);

        using StreamWriter? writer = leaveStreamOpen
                ? new StreamWriter(stream, encoding, 1024, true)
                : new StreamWriter(stream, encoding);


        var serializer = VcfSerializer.GetSerializer(writer, version, options, tzConverter);

        foreach (VCard? vCard in vCards)
        {
            if (vCard is null)
            {
                continue;
            }
            vCard.Version = version;
            serializer.Serialize(vCard);
        }
    }


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
    /// Obwohl die Methode selbst threadsafe ist, sind es die an die Methode übergebenen
    /// <see cref="VCard" />-Objekte nicht. Sperren Sie den lesenden und schreibenden
    /// Zugriff auf diese <see cref="VCard" />-Objekte während der Ausführung dieser
    /// Methode!
    /// </note>
    /// <note type="tip">
    /// Sie können der Methode auch ein einzelnes <see cref="VCard" />-Objekt übergeben,
    /// da die <see cref="VCard" />-Klasse <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see>
    /// explizit implementiert.
    /// </note>
    /// <para>
    /// Die Methode serialisiert möglicherweise mehr vCards, als sich ursprünglich Elemente
    /// in <paramref name="vCards" /> befanden. Dies geschieht, wenn eine vCard 4.0
    /// serialisiert wird und sich in den Eigenschaften <see cref="VCard.Members" />
    /// oder <see cref="VCard.Relations" /> eines <see cref="VCard" />-Objekts weitere
    /// <see cref="VCard" />-Objekte in Form von <see cref="RelationVCardProperty" />-Objekten
    /// befanden.
    /// </para>
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option
    /// <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> serialisiert wird und
    /// wenn sich in der Eigenschaft <see cref="VCard.Relations" /> eines <see cref="VCard"
    /// />-Objekts ein <see cref="RelationVCardProperty" />-Objekt befindet, auf dessen
    /// <see cref="ParameterSection" /> in der Eigenschaft <see cref="ParameterSection.Relation"
    /// /> das Flag <see cref="RelationTypes.Agent" /> gesetzt ist.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="version" /> has an undefined
    /// value.</exception>
    /// <exception cref="OutOfMemoryException">The system is out of memory.</exception>
    public static string ToVcfString(
        IEnumerable<VCard?> vCards,
        VCdVersion version = VCard.DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOptions options = VcfOptions.Default)
    {
        if (vCards is null)
        {
            throw new ArgumentNullException(nameof(vCards));
        }

        using var stream = new MemoryStream();

        VCard.SerializeVcf(stream, vCards, version, tzConverter, options, leaveStreamOpen: true);

        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }



    #endregion

    #region Instance Methods

    /// <summary>Saves the <see cref="VCard" /> instance as a VCF file.</summary>
    /// <param name="fileName">The file path. If the file exists, it will be overwritten.</param>
    /// <param name="version">The vCard version of the VCF file to be written.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <param name="options">Options for writing the VCF file. The flags can be combined.</param>
    /// <remarks>
    /// <para>
    /// Die Methode serialisiert möglicherweise mehrere vCards. Dies geschieht, wenn
    /// eine VCF-Datei als vCard 4.0 gespeichert wird und sich in den Eigenschaften
    /// <see cref="VCard.Members" /> oder <see cref="VCard.Relations" /> des <see cref="VCard"
    /// />-Objekts weitere <see cref="VCard" />-Objekte in Form von <see cref="RelationVCardProperty"
    /// />-Objekten befanden.
    /// </para>
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option
    /// <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> serialisiert wird und
    /// wenn sich in der Eigenschaft <see cref="VCard.Relations" /> des <see cref="VCard"
    /// />-Objekts ein <see cref="RelationVCardProperty" />-Objekt befindet, auf dessen
    /// <see cref="ParameterSection" /> in der Eigenschaft <see cref="ParameterSection.Relation"
    /// /> das Flag <see cref="RelationTypes.Agent" /> gesetzt ist.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path or <paramref name="version" /> has an undefined value.</exception>
    /// <exception cref="IOException">The file could not be written.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SaveVcf(
        string fileName,
        VCdVersion version = DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOptions options = VcfOptions.Default) => VCard.SaveVcf(fileName, this, version, tzConverter, options);


    /// <summary>Serializes the <see cref="VCard" /> instance into a <see cref="Stream"
    /// /> using the VCF format.</summary>
    /// <param name="stream">A <see cref="Stream" /> into which the serialized <see
    /// cref="VCard" /> object is written.</param>
    /// <param name="version">The vCard version used for the serialization.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <param name="options">Options for serializing VCF. The flags can be combined.</param>
    /// <param name="leaveStreamOpen"> <c>true</c> means that the method does not close
    /// the underlying <see cref="Stream" />. The default value is <c>false</c>.</param>
    /// <remarks>
    /// <para>
    /// Die Methode serialisiert möglicherweise mehrere vCards. Dies geschieht, wenn
    /// das <see cref="VCard" />-Objekt als vCard 4.0 serialisiert wird und sich in
    /// den Eigenschaften <see cref="VCard.Members" /> oder <see cref="VCard.Relations"
    /// /> des <see cref="VCard" />-Objekts weitere <see cref="VCard" />-Objekte in
    /// Form von <see cref="RelationVCardProperty" />-Objekten befanden.
    /// </para>
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option
    /// <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> serialisiert wird und
    /// wenn sich in der Eigenschaft <see cref="VCard.Relations" /> des <see cref="VCard"
    /// />-Objekts ein <see cref="RelationVCardProperty" />-Objekt befindet, auf dessen
    /// <see cref="ParameterSection" /> in der Eigenschaft <see cref="ParameterSection.Relation"
    /// /> das Flag <see cref="RelationTypes.Agent" /> gesetzt ist.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="stream" /> does not support
    /// write operations or <paramref name="version" /> has an undefined value.</exception>
    /// <exception cref="IOException">I/O error.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="stream" /> was already
    /// closed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SerializeVcf(Stream stream,
                          VCdVersion version = DEFAULT_VERSION,
                          ITimeZoneIDConverter? tzConverter = null,
                          VcfOptions options = VcfOptions.Default,
                          bool leaveStreamOpen = false)

        => VCard.SerializeVcf(stream, this, version, tzConverter, options, leaveStreamOpen);


    /// <summary>Serializes the <see cref="VCard" /> instance as a <see cref="string"
    /// />, which has the format of a VCF file.</summary>
    /// <param name="version">The vCard version used for the serialization.</param>
    /// <param name="tzConverter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <param name="options">Options for serializing VCF. The flags can be combined.</param>
    /// <returns>The <see cref="VCard" />, serialized as <see cref="string" />, which
    /// has the format of a VCF file.</returns>
    /// <remarks>
    /// <para>
    /// Die Methode serialisiert möglicherweise mehrere vCards. Dies geschieht, wenn
    /// das <see cref="VCard" />-Objekt als vCard 4.0 serialisiert wird und sich in
    /// den Eigenschaften <see cref="VCard.Members" /> oder <see cref="VCard.Relations"
    /// /> des <see cref="VCard" />-Objekts weitere <see cref="VCard" />-Objekte in
    /// Form von <see cref="RelationVCardProperty" />-Objekten befanden.
    /// </para>
    /// <para>
    /// Ebenso verhält sich die Methode, wenn eine vCard 2.1 oder 3.0 mit der Option
    /// <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> serialisiert wird und
    /// wenn sich in der Eigenschaft <see cref="VCard.Relations" /> des <see cref="VCard"
    /// />-Objekts ein <see cref="RelationVCardProperty" />-Objekt befindet, auf dessen
    /// <see cref="ParameterSection" /> in der Eigenschaft <see cref="ParameterSection.Relation"
    /// /> das Flag <see cref="RelationTypes.Agent" /> gesetzt ist.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// <exception cref="ArgumentException"> <paramref name="version" /> has an undefined
    /// value.</exception>
    /// <exception cref="OutOfMemoryException">The system is out of memory.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToVcfString(VCdVersion version = DEFAULT_VERSION, ITimeZoneIDConverter? tzConverter = null, VcfOptions options = VcfOptions.Default)
        => VCard.ToVcfString(this, version, tzConverter, options);

    #endregion


    [ExcludeFromCodeCoverage]
    private static FileStream InitializeFileStream(string fileName)
    {
        try
        {
            return new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(fileName));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message, nameof(fileName), e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (NotSupportedException e)
        {
            throw new ArgumentException(e.Message, nameof(fileName), e);
        }
        catch (System.Security.SecurityException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (PathTooLongException e)
        {
            throw new ArgumentException(e.Message, nameof(fileName), e);
        }
        catch (Exception e)
        {
            throw new IOException(e.Message, e);
        }
    }

}
