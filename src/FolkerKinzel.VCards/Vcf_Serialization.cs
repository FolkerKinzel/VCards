using System.ComponentModel;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards;

public static partial class Vcf
{
    /// <summary>Saves a collection of <see cref="VCard" /> objects in a common VCF
    /// file.</summary>
    /// <param name="vCards">The <see cref="VCard" /> objects to be saved. The collection
    /// may be empty or may contain <c>null</c> values.</param>
    /// <param name="filePath">The file path. If the file exists, it will be truncated and
    /// overwritten.</param>
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
    /// <paramref name="vCards" />. This happens when a VCF file is saved as vCard&#160;4.0 and 
    /// when in the properties <see cref="VCard.Members" /> or <see cref="VCard.Relations"
    /// /> of a <see cref="VCard" /> object further VCard objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard&#160;2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOpts.AppendAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.RelationType" /> parameter the 
    /// <see cref="Rel.Agent" /> flag is set. 
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="filePath" /> or <paramref
    /// name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="filePath" /> is not a valid
    /// file path.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version" /> is not a defined value of the <see cref="VcfOpts"/> 
    /// enum.
    /// </exception>
    /// <exception cref="IOException">The file could not be written.</exception>
    public static void Save(
        IEnumerable<VCard?> vCards,
        string filePath,
        VCdVersion version = VCard.DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOpts options = VcfOpts.Default)
    {
        _ArgumentNullException.ThrowIfNull(vCards, nameof(vCards));

        using FileStream stream = InitializeFileStream(filePath);
        Serialize(vCards, stream, version, tzConverter, options, false);
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
    /// <param name="leaveStreamOpen"> <c>true</c> means that <paramref name="stream"/> will
    /// not be closed by the method. The default value is <c>false</c> to close 
    /// <paramref name="stream"/> when the method returns.</param>
    /// 
    /// <remarks>
    /// <note type="caution">
    /// Although the method itself is thread-safe, the <see cref="VCard" /> objects
    /// passed to the method are not. Block read and write access to these <see cref="VCard"
    /// /> objects, while this method is being executed!
    /// </note>
    /// <para>
    /// The method may serialize more vCards than were originally elements in the argument 
    /// <paramref name="vCards" />. This happens when a VCF file is saved as vCard&#160;4.0 and 
    /// when in the properties <see cref="VCard.Members" /> or <see cref="VCard.Relations"
    /// /> of a <see cref="VCard" /> object further VCard objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard&#160;2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOpts.AppendAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.RelationType" /> parameter the 
    /// <see cref="Rel.Agent" /> flag is set. 
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="stream" /> or <paramref
    /// name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="stream" /> does not support
    /// write operations.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version" /> is not a defined value of the <see cref="VcfOpts"/> 
    /// enum.
    /// </exception>
    /// <exception cref="IOException">I/O error.</exception>
    /// <exception cref="ObjectDisposedException"> <paramref name="stream" /> was already
    /// closed.</exception>
    public static void Serialize(IEnumerable<VCard?> vCards,
                                 Stream stream,
                                 VCdVersion version = VCard.DEFAULT_VERSION,
                                 ITimeZoneIDConverter? tzConverter = null,
                                 VcfOpts options = VcfOpts.Default,
                                 bool leaveStreamOpen = false)
    {
        DebugWriter.WriteMethodHeader(
            $"{nameof(Vcf)}.{nameof(Serialize)}(IEnumerable<{nameof(VCard)}?>, {nameof(Stream)})");

        ValidateArguments(stream, vCards, leaveStreamOpen);
        using var serializer = VcfSerializer.GetSerializer(stream,
                                                           leaveStreamOpen,
                                                           version,
                                                           options,
                                                           tzConverter);

        var list = vCards.OfType<VCard>().ToList();

        if (version < VCdVersion.V4_0)
        {
            if (options.IsSet(VcfOpts.AppendAgentAsSeparateVCard))
            {
                AppendAgents(list);
            }
        }
        else
        {
            foreach (VCard vCard in list)
            {
                vCard.NormalizeMembers();
            }

            VCard.ReferenceIntl(list);
        }

#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1
        foreach (VCard vCard in list)
#else
        foreach (VCard vCard in CollectionsMarshal.AsSpan(list))
#endif
        {
            vCard.Version = version;

            if (options.HasFlag(VcfOpts.UpdateTimeStamp))
            {
                vCard.Updated = new TimeStampProperty();
            }

            serializer.Serialize(vCard);
        }

        static void ValidateArguments(Stream stream, IEnumerable<VCard?> vCards, bool leaveStreamOpen)
        {
            _ArgumentNullException.ThrowIfNull(stream, nameof(stream));

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

                if (vCard.Relations.PrefOrNullIntl(x => x.Value.VCard is not null &&
                                                        x.Parameters.RelationType.IsSet(Rel.Agent),
                                                        skipEmptyItems: true) is RelationProperty agent)
                {
                    Debug.Assert(agent.Value.VCard != null);

                    if (!list.Contains(agent.Value.VCard))
                    {
                        list.Add(agent.Value.VCard);
                    }
                }

            }//for
        }
    }

    /// <summary>Serializes <paramref name="vCards" /> as a <see cref="string" /> that
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
    /// <paramref name="vCards" />. This happens when a VCF file is saved as vCard&#160;4.0 and 
    /// when in the properties <see cref="VCard.Members" /> or <see cref="VCard.Relations"
    /// /> of a <see cref="VCard" /> object further VCard objects can be found. 
    /// </para>
    /// <para>
    /// In the same way the method behaves, if a vCard&#160;2.1 or 3.0 is serialized with the 
    /// option <see cref="VcfOpts.AppendAgentAsSeparateVCard" /> and if in the
    /// <see cref="VCard.Relations" /> property of a VCard object an instance is located 
    /// on whose <see cref="ParameterSection.RelationType" /> parameter the 
    /// <see cref="Rel.Agent" /> flag is set. 
    /// </para>
    /// </remarks>
    /// <seealso cref="ITimeZoneIDConverter" />
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\NoPidExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="vCards" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version" /> is not a defined value of the <see cref="VcfOpts"/> 
    /// enum.
    /// </exception>
    /// <exception cref="OutOfMemoryException">The system is out of memory.</exception>
    public static string AsString(
        IEnumerable<VCard?> vCards,
        VCdVersion version = VCard.DEFAULT_VERSION,
        ITimeZoneIDConverter? tzConverter = null,
        VcfOpts options = VcfOpts.Default)
    {
        _ArgumentNullException.ThrowIfNull(vCards, nameof(vCards));

        using var stream = new MemoryStream();

        Serialize(vCards, stream, version, tzConverter, options, leaveStreamOpen: true);

        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    [ExcludeFromCodeCoverage]
    private static FileStream InitializeFileStream(string filePath)
    {
        try
        {
            return new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (NotSupportedException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (System.Security.SecurityException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (PathTooLongException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (Exception e)
        {
            throw new IOException(e.Message, e);
        }
    }
}
