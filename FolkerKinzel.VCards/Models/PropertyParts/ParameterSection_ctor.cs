using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    public partial class ParameterSection
    {
        internal ParameterSection() { }

        internal ParameterSection(string propertyKey, IEnumerable<Tuple<string, string>> propertyParameters, VCardDeserializationInfo info)
        {
            Debug.Assert(propertyParameters != null);
            Debug.Assert(propertyParameters.All(x => x != null));
            Debug.Assert(propertyParameters.All(
                x => !(string.IsNullOrWhiteSpace(x.Item1) || string.IsNullOrWhiteSpace(x.Item2))
                ));

            StringBuilder builder = info.Builder;

            foreach (Tuple<string, string>? parameter in propertyParameters)
            {
                switch (parameter.Item1)
                {
                    case ParameterKey.LANGUAGE:
                        this.Language = parameter.Item2;
                        break;
                    case ParameterKey.VALUE:
                        {
                            string valValue = CleanParameterValue(parameter.Item2, builder);
                            VCdDataType? dataType = VCdDataTypeConverter.Parse(valValue);
                            this.DataType = dataType;

                            if (!dataType.HasValue)
                            {
                                VCdContentLocation contentLocation = VCdContentLocationConverter.Parse(valValue);
                                this.ContentLocation = contentLocation;

                                if (contentLocation == VCdContentLocation.Url)
                                {
                                    this.DataType = VCdDataType.Uri;
                                }
                            }

                            break;
                        }
                    case ParameterKey.PREF:
                        {
                            if (int.TryParse(parameter.Item2.Trim().Trim(info.AllQuotes), out int intVal))
                            {
                                this.Preference = intVal;
                            }
                            break;
                        }
                    case ParameterKey.PID:
                        {
                            string[] val = parameter.Item2.Trim().Trim(info.AllQuotes).Split(info.Comma, StringSplitOptions.RemoveEmptyEntries);

                            List<PropertyID> pid = (List<PropertyID>?)this.PropertyIDs ?? new List<PropertyID>();
                            this.PropertyIDs = pid;

                            for (int i = 0; i < val.Length; i++)
                            {
                                string[] arr = val[i].Split(info.Dot, StringSplitOptions.RemoveEmptyEntries);
                                pid.Add(new PropertyID(arr));
                            }

                            break;
                        }
                    case ParameterKey.TYPE:
                        {
                            List<string>? values = parameter.Item2.SplitValueString(',', StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < values.Count; i++)
                            {
                                if (builder.Length != 0)
                                {
                                    ParseTypeParameter(CleanParameterValue(values[i], builder), propertyKey);
                                }
                            }
                            break;
                        }
                    case ParameterKey.GEO:
                        this.GeographicPosition = GeoCoordinateConverter.Parse(parameter.Item2.Trim().Trim(info.AllQuotes));
                        break;
                    case ParameterKey.TZ:
                        this.TimeZone = TimeZoneInfoConverter.Parse(parameter.Item2.Trim().Trim(info.AllQuotes));
                        break;
                    case ParameterKey.SORT_AS:
                        {
                            List<string>? list = parameter.Item2.SplitValueString(',', StringSplitOptions.RemoveEmptyEntries);

                            for (int i = list.Count - 1; i >= 0; i--)
                            {
                                builder.Clear();
                                builder.Append(list[i]).Trim().RemoveQuotes().UnMask(VCdVersion.V4_0);

                                if (builder.Length != 0)
                                {
                                    list[i] = builder.ToString();
                                }
                                else
                                {
                                    list.RemoveAt(i);
                                }

                            }

                            break;
                        }
                    case ParameterKey.CALSCALE:
                        this.Calendar = parameter.Item2.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.ENCODING:
                        this.Encoding = VCdEncodingConverter.Parse(parameter.Item2);
                        break;
                    case ParameterKey.CHARSET:
                        this.Charset = parameter.Item2.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.ALTID:
                        this.AltID = parameter.Item2.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.MEDIATYPE:
                        this.MediaType = parameter.Item2.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.LABEL:
#if NET40
                        this.Label = parameter.Item2.Trim().Trim(info.DoubleQuotes).Replace(@"\n", Environment.NewLine).Replace(@"\N", Environment.NewLine);
#else
                        this.Label = parameter.Item2.Trim().Trim(info.DoubleQuotes).Replace(@"\n", Environment.NewLine, StringComparison.OrdinalIgnoreCase);
#endif
                        break;
                    case ParameterKey.CONTEXT:
                        this.Context = parameter.Item2.Trim().Trim(info.AllQuotes);
                        break;
                    case ParameterKey.INDEX:
                        {
                            if (int.TryParse(parameter.Item2.Trim().Trim(info.AllQuotes), out int result))
                            {
                                this.Index = result;
                            }
                            break;
                        }
                    case ParameterKey.LEVEL:
                        if (propertyKey == VCard.PropKeys.NonStandard.EXPERTISE)
                        {
                            this.ExpertiseLevel = ExpertiseLevelConverter.Parse(parameter.Item2.Trim().Trim(info.AllQuotes));
                        }
                        else // HOBBY oder INTEREST
                        {
                            this.InterestLevel = InterestLevelConverter.Parse(parameter.Item2.Trim().Trim(info.AllQuotes));
                        }//else
                        break;


                    default:
                        {
                            List<KeyValuePair<string, string>> userAttributes 
                                = (List<KeyValuePair<string, string>>?)this.NonStandardParameters ?? new List<KeyValuePair<string, string>>();
                            userAttributes.Add(new KeyValuePair<string, string>(parameter.Item1, parameter.Item2));
                            this.NonStandardParameters = userAttributes;
                            break;
                        }

                }//switch


            }//foreach

        }//ctor


        private static string CleanParameterValue(string parameterValue, StringBuilder builder)
        {
            builder.Clear();
            builder.Append(parameterValue);

            for (int i = builder.Length - 1; i >= 0; i--)
            {
                char c = builder[i];

                if (char.IsWhiteSpace(c) || c == '\'' || c == '\"')
                {
                    builder.Remove(i, 1);
                    continue;
                }

                builder[i] = char.ToUpperInvariant(c);
            }

            return builder.ToString();
        }


        private void ParseTypeParameter(string typeValue, string propertyKey)
        {
            switch (typeValue)
            {
                case TypeValue.PREF:
                    this.Preference = 1;
                    return;
                case TypeValue.HOME:
                    this.PropertyClass = this.PropertyClass.Set(Enums.PropertyClassTypes.Home);
                    return;
                case TypeValue.WORK:
                    this.PropertyClass = this.PropertyClass.Set(Enums.PropertyClassTypes.Work);
                    return;
                default:
                    break;
            }


            switch (propertyKey)
            {
                case VCard.PropKeys.ADR:
                    {
                        this.AddressType = AddressTypesConverter.Parse(typeValue, this.AddressType);
                        break;
                    }
                case VCard.PropKeys.TEL:
                    {
                        this.TelephoneType = TelTypesConverter.Parse(typeValue, this.TelephoneType);
                        break;
                    }
                case VCard.PropKeys.RELATED:
                    {
                        this.RelationType = RelationTypesConverter.Parse(typeValue, this.RelationType);
                        break;
                    }
                case VCard.PropKeys.EMAIL:
                    this.EmailType = propertyKey;
                    break;
                case VCard.PropKeys.KEY:
                    this.MediaType = MimeTypeConverter.MimeTypeFromEncryptionTypeValue(typeValue);
                    break;
                case VCard.PropKeys.SOUND:
                    this.MediaType = MimeTypeConverter.MimeTypeFromSoundTypeValue(typeValue);
                    break;
                case VCard.PropKeys.PHOTO:
                case VCard.PropKeys.LOGO:
                    this.MediaType = MimeTypeConverter.MimeTypeFromImageTypeValue(typeValue);
                    break;
                case VCard.PropKeys.IMPP:
                    this.InstantMessengerType = ImppTypesConverter.Parse(typeValue, this.InstantMessengerType);
                    break;

                default:
                    break;
            }//switch
        }




    }
}
