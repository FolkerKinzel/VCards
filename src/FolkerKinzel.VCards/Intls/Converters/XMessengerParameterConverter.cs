using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class XMessengerParameterConverter
{
    internal static void ConvertFromInstantMessengerType(ParameterSection par)
    {
        Debug.Assert(par is not null);

        if (par.InstantMessengerType.IsSet(ImppTypes.Personal) || par.PropertyClass.IsSet(PropertyClassTypes.Home))
        {
            par.PropertyClass = par.PropertyClass.Set(PropertyClassTypes.Home);
        }

        if (par.InstantMessengerType.IsSet(ImppTypes.Business) || par.PropertyClass.IsSet(PropertyClassTypes.Work))
        {
            par.PropertyClass = par.PropertyClass.Set(PropertyClassTypes.Work);
        }

        if (par.InstantMessengerType.IsSet(ImppTypes.Mobile) || par.TelephoneType.IsSet(TelTypes.PCS))
        {
            par.TelephoneType = par.TelephoneType.Set(TelTypes.PCS);
        }
    }

    internal static void ConvertToInstantMessengerType(ParameterSection par)
    {
        Debug.Assert(par is not null);

        if (par.TelephoneType.IsSet(TelTypes.PCS))
        {
            par.InstantMessengerType = par.InstantMessengerType.Set(ImppTypes.Mobile);
        }
        if (par.PropertyClass.IsSet(PropertyClassTypes.Home))
        {
            par.InstantMessengerType = par.InstantMessengerType.Set(ImppTypes.Personal);
        }
        if (par.PropertyClass.IsSet(PropertyClassTypes.Work))
        {
            par.InstantMessengerType = par.InstantMessengerType.Set(ImppTypes.Business);
        }
    }

}
