using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
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
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than were originally elements in the argument 
    /// <paramref name="vCards" />. This happens when a VCF file is saved as vCard 4.0 and 
    /// when in the properties <see cref="VCard.Members" /> or <see cref="VCard.Relations"
    /// /> of a <see cref="VCard" /> object further VCard objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard 2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.Relation" /> parameter the 
    /// <see cref="RelationTypes.Agent" /> flag is set. 
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

        // prevents an empty file from being written:
        if (!vCards.Any(x => x != null))
        {
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
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than were originally elements in the argument 
    /// <paramref name="vCards" />. This happens when a VCF file is saved as vCard 4.0 and 
    /// when in the properties <see cref="VCard.Members" /> or <see cref="VCard.Relations"
    /// /> of a <see cref="VCard" /> object further VCard objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard 2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.Relation" /> parameter the 
    /// <see cref="RelationTypes.Agent" /> flag is set. 
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
        DebugWriter.WriteMethodHeader(
            $"{nameof(VCard)}.{nameof(SerializeVcf)}({nameof(Stream)}, IEnumerable<{nameof(VCard)}?>");
        
        ValidateArguments(stream, vCards, leaveStreamOpen);
        using VcfSerializer serializer = VcfSerializer.GetSerializer(stream,
                                                                     leaveStreamOpen,
                                                                     version,
                                                                     options,
                                                                     tzConverter);

        var list = vCards.WhereNotNull().ToList();

        if (version < VCdVersion.V4_0)
        {
            if (options.IsSet(VcfOptions.IncludeAgentAsSeparateVCard))
            {
                AppendAgents(list);
            }
        }
        else
        {
            foreach (var vCard in list)
            {
                vCard.NormalizeMembers(serializer);
            }

            ReferenceIntl(list);
        }

        foreach (VCard vCard in list)
        {
            vCard.Version = version;
            serializer.Serialize(vCard);
        }

        static void ValidateArguments(Stream stream, IEnumerable<VCard?> vCards, bool leaveStreamOpen)
        {
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
        }

        static void AppendAgents(List<VCard> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                VCard vCard = list[i];

                if (vCard.Relations is null)
                {
                    continue;
                }

                if (vCard.Relations.PrefOrNullIntl(x => x is RelationVCardProperty &&
                                                        x.Parameters.Relation.IsSet(RelationTypes.Agent),
                                                        ignoreEmptyItems: true) is RelationVCardProperty agent)
                {
                    if (!list.Contains(agent.Value))
                    {
                        list.Add(agent.Value);
                    }
                }

            }//for
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
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than were originally elements in the argument 
    /// <paramref name="vCards" />. This happens when a VCF file is saved as vCard 4.0 and 
    /// when in the properties <see cref="VCard.Members" /> or <see cref="VCard.Relations"
    /// /> of a <see cref="VCard" /> object further VCard objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard 2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.Relation" /> parameter the 
    /// <see cref="RelationTypes.Agent" /> flag is set. 
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
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more the one vCard. This happens when a VCF file is saved 
    /// as vCard 4.0 and when in the properties <see cref="VCard.Members" /> or 
    /// <see cref="VCard.Relations" /> of a <see cref="VCard" /> object further VCard 
    /// objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard 2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.Relation" /> parameter the 
    /// <see cref="RelationTypes.Agent" /> flag is set. 
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
    ///<remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more the one vCard. This happens when a VCF file is saved 
    /// as vCard 4.0 and when in the properties <see cref="VCard.Members" /> or 
    /// <see cref="VCard.Relations" /> of a <see cref="VCard" /> object further VCard 
    /// objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard 2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.Relation" /> parameter the 
    /// <see cref="RelationTypes.Agent" /> flag is set. 
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
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more the one vCard. This happens when a VCF file is saved 
    /// as vCard 4.0 and when in the properties <see cref="VCard.Members" /> or 
    /// <see cref="VCard.Relations" /> of a <see cref="VCard" /> object further VCard 
    /// objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard 2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOptions.IncludeAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.Relation" /> parameter the 
    /// <see cref="RelationTypes.Agent" /> flag is set. 
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

    private void NormalizeMembers(VcfSerializer serializer)
    {
        RelationProperty[] members = Members?.WhereNotNull().ToArray() ?? Array.Empty<RelationProperty>();
        Members = members;

        for (int i = 0; i < members.Length; i++)
        {
            RelationProperty prop = members[i];

            if (prop is RelationTextProperty textProp)
            {
                if (textProp.IsEmpty && serializer.IgnoreEmptyItems)
                {
                    continue;
                }

                members[i] = Uri.TryCreate(textProp.Value?.Trim(), UriKind.Absolute, out Uri? uri)
                    ? RelationProperty.FromUri(uri, prop.Parameters.Relation, prop.Group)
                    : RelationProperty.FromVCard(new VCard { DisplayNames = new TextProperty(textProp.Value) });
            }
        }
    }
}
