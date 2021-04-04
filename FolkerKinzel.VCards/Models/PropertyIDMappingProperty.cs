using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Globalization;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt Informationen, die dazu dienen, verschiedene Bearbeitungsstände einer <see cref="VCardProperty"/> plattformübergreifend
    /// zwischen verschiedenen vCards zu synchronisieren.
    /// </summary>
    public sealed class PropertyIDMappingProperty : VCardProperty, IEnumerable<PropertyIDMappingProperty>
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="PropertyIDMappingProperty"/>-Objekt.
        /// </summary>
        /// <param name="mappingNumber">Nummer des Mappings (Wert: zwischen 1 und 9).</param>
        /// <param name="uuid">Eine Instanz der <see cref="Guid"/>-Struktur, die als plattformübergreifender Bezeichner für eine 
        /// <see cref="VCardProperty"/> dient.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mappingNumber"/> ist kleiner als 1 oder größer als 9.</exception>
        public PropertyIDMappingProperty(int mappingNumber, Guid uuid) : base(new ParameterSection(), null) => Value = new PropertyIDMapping(mappingNumber, uuid);


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="vcfRow"><see cref="VcfRow"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Aus <paramref name="vcfRow"/> kann kein <see cref="PropertyIDMapping"/>
        /// geparst werden.</exception>
        internal PropertyIDMappingProperty(VcfRow vcfRow)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            if (vcfRow.Value is null)
            {
                return;
            }

            Value = PropertyIDMapping.Parse(vcfRow.Value);

        }


        /// <summary>
        /// Die von der <see cref="PropertyIDMappingProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new PropertyIDMapping Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;

                
        /// <inheritdoc/>
        public override bool IsEmpty => Value.IsEmpty;


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Value.AppendTo(serializer.Builder);
        }

        IEnumerator<PropertyIDMappingProperty> IEnumerable<PropertyIDMappingProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PropertyIDMappingProperty>)this).GetEnumerator();
    }
}
