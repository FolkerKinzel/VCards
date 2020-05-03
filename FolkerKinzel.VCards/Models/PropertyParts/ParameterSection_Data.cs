using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt die Information über die Parameter einer vCard-Property.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    public partial class ParameterSection
    {
        private readonly Dictionary<VCdParam, object> _propDic = new Dictionary<VCdParam, object>();

        [return: MaybeNull]
        private T Get<T>(VCdParam prop)
        {
            return _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;
        }

        private void Set<T>(VCdParam prop, T value)
        {
            if (value is null || value.Equals(default))
            {
                _propDic.Remove(prop);
            }
            else
            {
                _propDic[prop] = value;
            }
        }




        /// <summary>
        /// <c>(TYPE)</c>: Beschreibt die Art einer Adresse. (2,3)
        /// </summary>
        public AddressTypes? AddressType
        {
            get => Get<AddressTypes?>(VCdParam.AddressType);
            set
            {
                value = (value == (AddressTypes)0) ? null : value;
                Set(VCdParam.AddressType, value);
            }
        }


        /// <summary>
        /// <c>(ALTID)</c>: Ein String, der zu erkennen gibt, dass mehrere Instanzen derselben Property dasselbe 
        /// darstellen (z.B. in unterschiedlichen Sprachen). (4)
        /// </summary>
        public string? AltID
        {
            get => Get<string?>(VCdParam.AltID);
            set
            {
                Set(VCdParam.AltID, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
            }
        }

        /// <summary>
        /// <c>(CALSCALE)</c>: Gibt die Art des Kalenders an, der für Datumsangaben verwendet wird. (4)
        /// </summary>
        /// <value>Der einzige offiziell registrierte Wert ist "GREGORIAN" für den gregorianischen 
        /// Kalender.</value>
        public string? Calendar
        {
            get => Get<string?>(VCdParam.Calendar);
            set
            {
                Set<string?>(VCdParam.Calendar, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
            }
        }

        /// <summary>
        /// <c>(CHARSET)</c>: Gibt den Zeichensatz an, der für die Property verwendet wurde. (2)
        /// </summary>
        public string? Charset
        {
            get => Get<string?>(VCdParam.Charset);
            set
            {
                Set(VCdParam.Charset, value);
            }
        }

        /// <summary>
        /// <c>(VALUE)</c>: Gibt an, wo sich der eigentiche Inhalt der Property befindet. (2)
        /// </summary>
        public VCdContentLocation ContentLocation
        {
            get => Get<VCdContentLocation>(VCdParam.ContentLocation);
            set
            {
                Set(VCdParam.ContentLocation, value);
            }
        }

        /// <summary>
        /// <c>(CONTEXT)</c>: Gibt den Kontext der Daten an, z.B. "VCARD" oder "LDAP". (3)
        /// </summary>
        /// <remarks>Kommt in der SOURCE-Property von vCard 3.0 zum Einsatz.</remarks>
        public string? Context
        {
            get => Get<string?>(VCdParam.Context);
            set
            {
                Set<string?>(VCdParam.Context, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
            }
        }


        /// <summary>
        /// <c>(VALUE)</c>: Gibt an, welchem der vom vCard-Standard vordefinierten Datentypen der Inhalt
        /// der vCard-Property entspricht. (3,4)
        /// </summary>
        public VCdDataType? DataType
        {
            get => Get<VCdDataType?>(VCdParam.DataType);
            set
            {
                Set(VCdParam.DataType, value);
            }
        }



        /// <summary>
        /// <c>(TYPE)</c>: Beschreibt die Art einer E-Mail. (2,3)
        /// </summary>
        /// <value>Verwenden Sie nur die Konstanten der Klasse
        /// <see cref="EmailType"/>.</value>
        public string? EmailType
        {
            get => Get<string?>(VCdParam.EmailType);
            set
            {
                Set<string?>(VCdParam.EmailType, string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant());
            }
        }


        /// <summary>
        /// <c>(ENCODING)</c>: Gibt die Encodierung der Property an. (2,3)
        /// </summary>
        /// <value>
        /// In vCard 3.0 ist nur <see cref="VCdEncoding.Base64"/> gestattet.
        /// </value>
        public VCdEncoding? Encoding
        {
            get => Get<VCdEncoding?>(VCdParam.Encoding);
            set
            {
                Set(VCdParam.Encoding, value);
            }
        }


        /// <summary>
        /// <c>(LEVEL)</c>: Grad der Sachkenntnis (Für Property "EXPERTISE"). (4 - Erweiterung RFC 6715)
        /// </summary>
        public ExpertiseLevel? ExpertiseLevel
        {
            get => Get<ExpertiseLevel?>(VCdParam.ExpertiseLevel);
            set
            {
                Set(VCdParam.ExpertiseLevel, value);
            }
        }


        /// <summary>
        /// <c>(GEO)</c>: Geografische Position (4)
        /// </summary>
        public GeoCoordinate? GeographicPosition
        {
            get => Get<GeoCoordinate?>(VCdParam.GeographicPosition);
            set
            {
                Set(VCdParam.GeographicPosition, value);
            }
        }


        /// <summary>
        /// <c>(TYPE)</c>: Nähere Beschreibung einer Instant-Messenger-Adresse. (3 - Erweiterung RFC 4770)
        /// </summary>
        public ImppTypes? InstantMessengerType
        {
            get => Get<ImppTypes?>(VCdParam.InstantMessengerType);
            set
            {
                Set(VCdParam.InstantMessengerType, value);
            }
        }

        /// <summary>
        /// <c>(INDEX)</c>: Index einer Property, wenn mehrere Instanzen derselben Property möglich sind. (4 - Erweiterung RFC 6715)
        /// </summary>
        public int? Index
        {
            get => Get<int?>(VCdParam.Index);

            set
            {
                if (value.HasValue && value < 1)
                {
                    value = 1;
                }

                Set(VCdParam.Index, value);
            }
        }


        /// <summary>
        /// <c>(LEVEL)</c>: Grad des Interesses für eine Sache (Für Property INTEREST.) (4 - Erweiterung RFC 6715)
        /// </summary>
        public InterestLevel? InterestLevel
        {
            get => Get<InterestLevel?>(VCdParam.InterestLevel);
            set
            {
                Set(VCdParam.InterestLevel, value);
            }
        }


        /// <summary>
        /// <c>(LABEL)</c>: Gibt die formatierte Textdarstellung einer Adresse an. ([2],[3],4)
        /// </summary>
        /// <remarks>In vCard 2.1 und vCard 3.0 wird der Inhalt als separate LABEL-Property eingefügt.</remarks>
        public string? Label
        {
            get => Get<string?>(VCdParam.Label);
            set
            {
                value = string.IsNullOrWhiteSpace(value) ? null : value;
                Set(VCdParam.Label, value);
            }
        }


        /// <summary>
        /// <c>(LANGUAGE)</c>: Sprache der Property. (2,3,4)
        /// </summary>
        public string? Language
        {
            get => Get<string?>(VCdParam.Language);
            set => Set<string?>(VCdParam.Language, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
        }


        /// <summary>
        /// <c>(MEDIATYPE)</c>: Gibt bei URIs den MIME-Typ an, auf den die Uri verweist (z.B. text/plain). (4)
        /// </summary>
        public string? MediaType
        {
            get => Get<string?>(VCdParam.MediaType);
            set => Set<string?>(VCdParam.MediaType, string.IsNullOrWhiteSpace(value) ? null : value.Trim());
        }

        /// <summary>
        /// Nichtstandardisierte Attribute. (2,3,4)
        /// </summary>
        /// <remarks>
        /// Um nicht-standardisierte Attribute in eine vCard-Property zu schreiben, muss beim Serialisieren des 
        /// <see cref="VCard"/>-Objekts das Flag <see cref="VcfOptions.WriteNonStandardParameters"/> gesetzt sein.
        /// </remarks>
        public IEnumerable<KeyValuePair<string, string>>? NonStandardParameters
        {
            get => Get<IEnumerable<KeyValuePair<string, string>>?>(VCdParam.NonStandard);
            set
            {
                Set(VCdParam.NonStandard, value);
            }
        }


        /// <summary>
        /// <c>(PREF</c> oder <c>TYPE=PREF)</c>: Drückt die Beliebtheit einer Property aus (zwischen 1 und 100). 1 bedeutet 
        /// am beliebtesten. Bei Properties, die mehrfach vorkommen, zählt die größte Beliebtheit. (2,3,4)
        /// </summary>
        public int Preference
        {
            get => _propDic.ContainsKey(VCdParam.Preference) ? (int)_propDic[VCdParam.Preference] : 100;

            set
            {
                if (value < PREF_MIN_VALUE)
                {
                    value = PREF_MIN_VALUE;
                }
                else if (value > PREF_MAX_VALUE)
                {
                    value = PREF_MAX_VALUE;
                }

                Set(VCdParam.Preference, value);
            }
        }


        /// <summary>
        /// <c>(TYPE)</c>: Klassifiziert eine Property als dienstlich und / oder privat. (2,3,4)
        /// </summary>
        public PropertyClassTypes? PropertyClass
        {
            get => Get<PropertyClassTypes?>(VCdParam.PropertyClass);
            set
            {
                value = (value == (PropertyClassTypes)0) ? null : value;
                Set(VCdParam.PropertyClass, value);
            }
        }


        /// <summary>
        /// <c>(PID)</c>: Property-ID zur Identifizierung einer bestimmten Property unter verschiedenen Instanzen derselben Property. (4)
        /// </summary>
        public IEnumerable<PropertyID>? PropertyIDs
        {
            get => Get<IEnumerable<PropertyID>?>(VCdParam.PID);
            set
            {
                Set(VCdParam.PID, value);
            }
        }



        /// <summary>
        /// <c>(TYPE)</c>: Bestimmt in der Relations-Property (RELATED) die Art der Beziehung zu einer Person. (4)
        /// </summary>
        public RelationTypes? RelationType
        {
            get => Get<RelationTypes?>(VCdParam.RelationType);
            set
            {
                value = (value == (RelationTypes)0) ? null : value;
                Set(VCdParam.RelationType, value);
            }
        }

        /// <summary>
        /// <c>(SORT-AS)</c>:&#160;<see cref="string"/>s (case-sensitiv!), die die Sortierreihenfolge festlegen. (Maximal so viele, wie Felder der 
        /// zusammengesetzten Property!) ([3],4)
        /// </summary>
        /// <example>
        /// <code>
        /// FN:Rene van der Harten
        /// N;SORT-AS="Harten,Rene":van der Harten;Rene,J.;Sir;R.D.O.N.
        /// </code>
        /// </example>
        /// <remarks>
        /// In vCard 3.0 wird eine separate <c>SORT-STRING</c> - Property eingefügt, in die lediglich der erste <see cref="string"/>
        /// übernommen wird.
        /// </remarks>
        public IEnumerable<string?>? SortAs
        {
            get => Get<IEnumerable<string?>?>(VCdParam.SortAs);
            set
            {
                Set(VCdParam.SortAs, value);
            }
        }


        /// <summary>
        /// <c>(TYPE)</c>: Beschreibt die Art einer Telefonnummer. (2,3,4)
        /// </summary>
        public TelTypes? TelephoneType
        {
            get => Get<TelTypes?>(VCdParam.TelephoneType);
            set
            {
                value = (value == (TelTypes)0) ? null : value;
                Set(VCdParam.TelephoneType, value);
            }
        }


        /// <summary>
        /// <c>(TZ)</c>: Zeitzone (4)
        /// </summary>
        public TimeZoneInfo? TimeZone
        {
            get => Get<TimeZoneInfo?>(VCdParam.TimeZone);
            set
            {
                Set(VCdParam.TimeZone, value);
            }
        }

    }
}