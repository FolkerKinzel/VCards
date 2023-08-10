using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

public class ContentSizeRestriction
{
    private const SizeRestriction DISCARD_MAX_SIZE = (SizeRestriction)6;

    public ContentSizeRestriction(SizeRestriction readRestriction = SizeRestriction.Uri)
    {
        SizeLimit = Normalize(readRestriction);

        //////////////////////////////////////////////////////////////////////////////////

        static SizeRestriction Normalize(SizeRestriction value)
        {
            if(value <= SizeRestriction.None) { return SizeRestriction.None; }

            if(value <= DISCARD_MAX_SIZE) { return SizeRestriction.Discard; }
            return value;
        }
    }

    public SizeRestriction SizeLimit { get; } = SizeRestriction.Uri;



    //internal bool FitsReadSizeRestriction(long length) =>
    //    FitsSizeRestriction(length, ReadRestriction);

    //internal bool FitsWriteSizeRestriction(long length) =>
    //    FitsSizeRestriction(length, WriteRestriction);


    //internal static bool FitsSizeRestriction(long length, SizeRestriction sizeRestriction)
    //{
    //    int restriction = (int)sizeRestriction;
    //    return restriction < 0 ? true : restriction >= length + OVERHEAD;
    //}

}
