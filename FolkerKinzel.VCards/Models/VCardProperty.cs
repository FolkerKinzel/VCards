using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Abstrakte Basisklasse aller Klassen, die vCard-Properties repräsentieren.
    /// </summary>
    /// <typeparam name="T">Beliebiger Datentyp, der den Inhalt der vCard-Property darstellt.</typeparam>
    public abstract class VCardProperty<T> : IVCardData, IVcfSerializable, IVcfSerializableData
    {
        private string? _group;

#pragma warning disable CS8618 // Das Feld "Value" lässt keine NULL-Werte zu und ist nicht initialisiert. Deklarieren Sie das Feld ggf. als "Nullable".

        /// <summary>
        /// Initialiert ein neues <see cref="VCardProperty{T}"/>-Objekt.
        /// </summary>
        /// <param name="parameters">Ein <see cref="ParameterSection"/>-Objekt, das den Parameter-Teil einer
        /// vCard-Property repräsentiert.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> ist <c>null</c>.</exception>
        protected VCardProperty(ParameterSection parameters, string? propertyGroup)
        {
            if(parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            Parameters = parameters;
            Group = propertyGroup;

        }

        /// <summary>
        /// Initialiert ein neues <see cref="VCardProperty{T}"/>-Objekt.
        /// </summary>
        protected VCardProperty()
        {
            Parameters = new ParameterSection();
        }

#pragma warning restore CS8618 // Das Feld "Value" lässt keine NULL-Werte zu und ist nicht initialisiert. Deklarieren Sie das Feld ggf. als "Nullable".

        /// <summary>
        /// Repräsentiert den Inhalt einer vCard-Property.
        /// </summary>
        public virtual T Value { get; protected set; }


        /// <summary>
        /// Optionaler Gruppenbezeichner einer vCard-Property.
        /// </summary>
        public string? Group
        {
            get { return _group; }
            set { _group = string.IsNullOrWhiteSpace(value) ? null : value.Trim(); }
        }

        /// <summary>
        /// Enthält die Daten des Parameter-Teils der vCard-Property. (Ist nie <c>null</c>.)
        /// </summary>
        public ParameterSection Parameters { get; }


        /// <summary>
        /// True, wenn das <see cref="VCardProperty{T}"/>-Objekt keine Daten enthält.
        /// </summary>
        public virtual bool IsEmpty => Value == null;


        /// <summary>
        /// Repräsentiert den Inhalt einer vCard-Property.
        /// </summary>
        object? IVCardData.Value => Value;





        /// <summary>
        /// Überladung der <see cref="Object.ToString"/>-Methode. Nur zum Debugging.
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="VCardProperty{T}"/>-Objekts. </returns>
        public override string ToString()
        {
            return Value?.ToString() ?? "<null>";
        }


        void IVcfSerializable.BuildProperty(VcfSerializer serializer)
        {
            Debug.Assert(serializer != null);
            Debug.Assert(serializer.PropertyKey != null);

            if (this.IsEmpty && !serializer.Options.IsSet(VcfOptions.WriteEmptyProperties)) return;

            var builder = serializer.Builder;
            Debug.Assert(builder != null);
            builder.Clear();

            PrepareForVcfSerialization(serializer);

            if (serializer.Options.IsSet(VcfOptions.WriteGroups) && Group != null)
            {
                builder.Append(Group);
                builder.Append('.');
            }
            builder.Append(serializer.PropertyKey);

            builder.Append(
                serializer.ParameterSerializer
                .Serialize(Parameters, serializer.PropertyKey, serializer.IsPref));

            builder.Append(':');
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
