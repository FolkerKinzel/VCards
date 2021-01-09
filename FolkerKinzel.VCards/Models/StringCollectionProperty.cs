using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert vCard-Properties, die eine Sammlung von <see cref="string"/>s speichern.
    /// </summary>
    public sealed class StringCollectionProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="StringCollectionProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Eine Sammlung von <see cref="string"/>s oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public StringCollectionProperty(IEnumerable<string?>? value, string? propertyGroup = null) : base(propertyGroup)
        {
            if (value == null)
            {
                return;
            }

            // Die Überprüfungen könnten eine allgemeine Verwendbarkeit der Klasse einschränken:
            string[] arr = value.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!.Trim()).ToArray();

            if (arr.Length == 0)
            {
                return;
            }

            this.Value = new ReadOnlyCollection<string>(arr);
        }

        /// <summary>
        /// Initialisiert ein <see cref="StringCollectionProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/> oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public StringCollectionProperty(string? value, string? propertyGroup = null) :
            this(value is null ? null : new string?[] { value }, propertyGroup)
        { }


        internal StringCollectionProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.DecodeQuotedPrintable();
            string[] arr = SplitValue();

            if (arr.Length == 0)
            {
                return;
            }

            this.Value = new ReadOnlyCollection<string>(arr);

            //////////////////////////////////////////////////////

            string[] SplitValue()
            {
                string? value = vcfRow.Value;
                StringBuilder builder = info.Builder;
                List<string> list = value.SplitValueString(',', StringSplitOptions.RemoveEmptyEntries);


                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = UnescapeString(list[i]);
                }

                return list.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray();


                string UnescapeString(string val)
                {
                    _ = builder.Clear();
                    _ = builder.Append(val);
                    _ = builder.UnMask(version);
                    return builder.ToString();
                }
            }
        }


        /// <summary>
        /// Die von der <see cref="StringCollectionProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new ReadOnlyCollection<string>? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            if (Value is null)
            {
                return;
            }

            Debug.Assert(Value.Count != 0);

            StringBuilder worker = serializer.Worker;
            StringBuilder builder = serializer.Builder;
            string s;

            for (int i = 0; i < Value.Count - 1; i++)
            {
                s = Value[i];

                Debug.Assert(!string.IsNullOrEmpty(s));

                _ = worker.Clear().Append(s).Mask(serializer.Version);
                _ = builder.Append(worker).Append(',');
            }

            s = Value[Value.Count - 1];

            Debug.Assert(!string.IsNullOrEmpty(s));

            _ = worker.Clear().Append(s).Mask(serializer.Version);
            _ = builder.Append(worker);
        }


        ///// <summary>
        ///// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="StringCollectionProperty"/>-Objekts. (Nur zum 
        ///// Debuggen.)
        ///// </summary>
        ///// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="StringCollectionProperty"/>-Objekts.</returns>
        /// <inheritdoc/>
        public override string ToString()
        {
            string s = "";

            if (Value is null)
            {
                return s;
            }

            Debug.Assert(Value.Count != 0);

            for (int i = 0; i < Value.Count - 1; i++)
            {
                s += Value[i];
                s += ", ";
            }

            s += Value[Value.Count - 1];

            return s;
        }

    }
}
