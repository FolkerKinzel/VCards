using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

public class ContentSizeRestriction
{
    private const int OVERHEAD = 64;

    private ContentSizeRestriction(ContentSizeRestriction other) :
        this(other.ReadRestriction, other.WriteRestriction)
    { }


    public ContentSizeRestriction(SizeRestriction readRestriction = SizeRestriction.Uri,
                                  SizeRestriction writeRestriction = SizeRestriction.Uri)
    {
        ReadRestriction = readRestriction;
        WriteRestriction = writeRestriction;
    }

    public SizeRestriction ReadRestriction { get; } = SizeRestriction.Uri;

    public SizeRestriction WriteRestriction { get; } = SizeRestriction.Uri;


    internal bool FitsReadSizeRestriction(long length) =>
        FitsSizeRestriction(length, ReadRestriction);

    internal bool FitsWriteSizeRestriction(long length) =>
        FitsSizeRestriction(length, WriteRestriction);


    private static bool FitsSizeRestriction(long length, SizeRestriction sizeRestriction)
    {
        int restriction = (int)sizeRestriction;
        return restriction < 0 ? true : restriction >= length + OVERHEAD;
    }

}
