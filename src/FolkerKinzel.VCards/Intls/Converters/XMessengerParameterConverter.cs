using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class XMessengerParameterConverter
{
    internal static void ConvertFromInstantMessengerType(ParameterSection par)
    {
        Debug.Assert(par is not null);

        if (par.InstantMessengerType.IsSet(Impp.Personal) || par.PropertyClass.IsSet(PCl.Home))
        {
            par.PropertyClass = par.PropertyClass.Set(PCl.Home);
        }

        if (par.InstantMessengerType.IsSet(Impp.Business) || par.PropertyClass.IsSet(PCl.Work))
        {
            par.PropertyClass = par.PropertyClass.Set(PCl.Work);
        }

        if (par.InstantMessengerType.IsSet(Impp.Mobile) || par.PhoneType.IsSet(Tel.PCS))
        {
            par.PhoneType = par.PhoneType.Set(Tel.PCS);
        }
    }

    internal static void ConvertToInstantMessengerType(ParameterSection par)
    {
        Debug.Assert(par is not null);

        if (par.PhoneType.IsSet(Tel.PCS))
        {
            par.InstantMessengerType = par.InstantMessengerType.Set(Impp.Mobile);
        }

        if (par.PropertyClass.IsSet(PCl.Home))
        {
            par.InstantMessengerType = par.InstantMessengerType.Set(Impp.Personal);
        }

        if (par.PropertyClass.IsSet(PCl.Work))
        {
            par.InstantMessengerType = par.InstantMessengerType.Set(Impp.Business);
        }
    }
}
