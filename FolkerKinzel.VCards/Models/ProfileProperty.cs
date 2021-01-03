using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-3.0-Property <c>PROFILE</c>, die festlegt, dass die vCard eine vCard ist.
    /// </summary>
    public class ProfileProperty : TextProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        private const string PROFILE_PROPERTY_VALUE = "VCARD";

        /// <summary>
        /// Initialisiert ein neues <see cref="ProfileProperty"/>-Objekt.
        /// </summary>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        public ProfileProperty(string? propertyGroup = null) : base(PROFILE_PROPERTY_VALUE, propertyGroup) { }


        internal ProfileProperty(VcfRow row, VCardDeserializationInfo info, VCdVersion version) : base(row, info, version) { }

        /// <summary>
        /// Textinhalt der <see cref="ProfileProperty"/>.
        /// </summary>
        public override string Value => base.Value ?? PROFILE_PROPERTY_VALUE;


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            this.Parameters.Encoding = null;
            this.Parameters.Charset = null;
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            serializer.Builder.Append(PROFILE_PROPERTY_VALUE);
        }
    }
}
