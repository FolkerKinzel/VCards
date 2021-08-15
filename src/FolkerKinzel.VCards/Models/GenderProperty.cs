using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Text;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FolkerKinzel.VCards.Intls.Deserializers;
using System.Collections;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die in vCard 4.0 eingeführte vCard-Property <c>GENDER</c>, die Informationen über das Geschlecht
    /// und die Geschlechtsidentität speichert.
    /// </summary>
    public sealed class GenderProperty : VCardProperty, IEnumerable<GenderProperty>
    {
        /// <summary>
        /// Copy ctor
        /// </summary>
        /// <param name="prop"></param>
        private GenderProperty(GenderProperty prop) : base(prop)
            => Value = prop.Value;

        /// <summary>
        /// Initialisiert ein neues <see cref="GenderProperty"/>-Objekt.
        /// </summary>
        /// <param name="sex">Standardisierte Geschlechtsangabe.</param>
        /// <param name="genderIdentity">Freie Beschreibung des sexuellen Identität.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public GenderProperty(VCdSex? sex,
                              string? genderIdentity = null,
                              string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = new Gender(sex, genderIdentity);

        /// <summary>
        /// Die von der <see cref="GenderProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Gender Value
        {
            get;
        }

        
        /// <inheritdoc/>
        public override bool IsEmpty => Value.IsEmpty; // Value ist nie null


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        internal GenderProperty(VcfRow vcfRow, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            VCdSex? sex = null;
            string? genderIdentity = null;
            ValueSplitter semicolonSplitter = vcfRow.Info.SemiColonSplitter;

            bool initGenderIdentity = false;
            semicolonSplitter.ValueString = vcfRow.Value;
            foreach (var s in semicolonSplitter)
            {
                if (initGenderIdentity)
                {
                    genderIdentity = s.UnMask(vcfRow.Info.Builder, version);
                    break;
                }
                else
                {
                    sex = VCdSexConverter.Parse(s);
                    initGenderIdentity = true;
                }
            }

            Value = new Gender(sex, genderIdentity);
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            Value.AppendVCardStringTo(serializer);
        }


        IEnumerator<GenderProperty> IEnumerable<GenderProperty>.GetEnumerator()
        {
            yield return this;
        }


        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GenderProperty>)this).GetEnumerator();

        public override object Clone() => new GenderProperty(this);
    }
}
