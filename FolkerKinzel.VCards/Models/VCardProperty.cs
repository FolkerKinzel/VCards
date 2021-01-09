using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Abstrakte Basisklasse aller Klassen, die vCard-Properties repräsentieren.
    /// </summary>
    public abstract class VCardProperty
    {
        internal const bool OBSOLETE_AS_ERROR = false;
        internal const string OBSOLETE_MESSAGE = "Use \"Value\" instead!";
        private string? _group;

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
        /// Konstruktor, der von abgeleiteten Klassen aufgerufen wird.
        /// </summary>
        /// <param name="propertyGroup">Bezeichner der Gruppe von <see cref="VCardProperty"/>-Objekten,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        protected VCardProperty(string? propertyGroup)
        {
            Parameters = new ParameterSection();
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
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
        public virtual bool IsEmpty => GetVCardPropertyValue() is null;


        /// <summary>
        /// Überladung der <see cref="object.ToString"/>-Methode. Nur zum Debugging.
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="VCardProperty"/>-Objekts. </returns>
        public override string ToString() => Value?.ToString() ?? "<null>";


        internal void BuildProperty(VcfSerializer serializer)
        {
            Debug.Assert(serializer != null);
            Debug.Assert(serializer.PropertyKey != null);

            if (this.IsEmpty && !serializer.Options.IsSet(VcfOptions.WriteEmptyProperties))
            {
                return;
            }

            StringBuilder builder = serializer.Builder;
            Debug.Assert(builder != null);
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
        }


        [InternalProtected]
        internal virtual void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            Debug.Assert(serializer != null);

            if (this.Parameters.Encoding != VCdEncoding.Base64)
            {
                this.Parameters.Encoding = null;
            }
            this.Parameters.Charset = null;
        }


        internal abstract void AppendValue(VcfSerializer serializer);

    }
}
