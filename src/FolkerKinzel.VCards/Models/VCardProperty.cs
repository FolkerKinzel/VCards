using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Abstrakte Basisklasse aller Klassen, die vCard-Properties repräsentieren.
/// </summary>
public abstract class VCardProperty : ICloneable
{
    private string? _group;

    /// <summary>
    /// Kopierkonstruktor.
    /// </summary>
    /// <param name="prop">Das zu klonende <see cref="VCardProperty"/>-Objekt.</param>
    protected VCardProperty(VCardProperty prop)
    {
        Parameters = (ParameterSection)prop.Parameters.Clone();
        Group = prop.Group;
    }

    /// <summary>
    /// Konstruktor, der von abgeleiteten Klassen aufgerufen wird.
    /// </summary>
    /// <param name="parameters">Ein <see cref="ParameterSection"/>-Objekt, das den Parameter-Teil einer
    /// vCard-Property repräsentiert.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe von <see cref="VCardProperty"/>-Objekten,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameters"/> ist <c>null</c>.</exception>
    protected VCardProperty(ParameterSection parameters, string? propertyGroup)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }
        Parameters = parameters;
        Group = propertyGroup;

    }

    /// <summary>
    /// Die von der <see cref="VCardProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public object? Value => GetVCardPropertyValue();


    /// <summary>
    /// Zugriffsmethode für die Daten von <see cref="VCardProperty"/>.
    /// </summary>
    /// <returns>Die Daten, die den Inhalt von <see cref="VCardProperty"/> darstellen.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract object? GetVCardPropertyValue();


    /// <summary>
    /// Gruppenbezeichner einer vCard-Property oder <c>null</c>, wenn die vCard-Property keinen Gruppenbezeichner hat.
    /// </summary>
    public string? Group
    {
        get => _group;
        set => _group = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    /// <summary>
    /// Enthält die Daten des Parameter-Abschnitts einer vCard-Property. (Nie <c>null</c>.)
    /// </summary>
    public ParameterSection Parameters { get; }


    /// <summary>
    /// <c>true</c>, wenn das <see cref="VCardProperty"/>-Objekt keine verwertbaren Daten enthält.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Value))]
    public virtual bool IsEmpty => GetVCardPropertyValue() is null;


    /// <summary>
    /// Überladung der <see cref="object.ToString"/>-Methode. Nur zum Debugging.
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="VCardProperty"/>-Objekts. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? "<null>";


    internal bool BuildProperty(VcfSerializer serializer)
    {
        Asserts(serializer);

        StringBuilder builder = serializer.Builder;
        _ = builder.Clear();

        PrepareForVcfSerialization(serializer);

        if (serializer.Options.IsSet(VcfOptions.WriteGroups) && Group != null)
        {
            _ = builder.Append(Group);
            _ = builder.Append('.');
        }
        _ = builder.Append(serializer.PropertyKey);

        _ = builder.Append(
            serializer.ParameterSerializer
            .Serialize(Parameters, serializer.PropertyKey, serializer.IsPref));

        _ = builder.Append(':');
        AppendValue(serializer);

        // vermeidet, dass das Line-Wrapping in vCard 2.1 durchgeführt wird, wenn es schon wegen
        // QuotedPrintable oder Base64 gemacht worden ist:
        return !(serializer.Version == VCdVersion.V2_1 &&
            (Parameters.Encoding == ValueEncoding.QuotedPrintable || Parameters.Encoding == ValueEncoding.Base64));
    }

    [ExcludeFromCodeCoverage]
    [Conditional("DEBUG")]
    private void Asserts(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        Debug.Assert(serializer.PropertyKey != null);
        Debug.Assert(!this.IsEmpty || serializer.Options.IsSet(VcfOptions.WriteEmptyProperties));
        Debug.Assert(serializer.Builder != null);
    }

    internal virtual void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        if (this.Parameters.Encoding != ValueEncoding.Base64)
        {
            this.Parameters.Encoding = null;
        }
        this.Parameters.CharSet = null;
    }


    internal abstract void AppendValue(VcfSerializer serializer);


    /// <inheritdoc/>
    public abstract object Clone();
}
