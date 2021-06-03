using System;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class DoubleExtension
    {
        //private const double _2 = 0.01;
        //private const double _3 = 0.001;
        //private const double _4 = 0.0001;
        //private const double _5 = 0.00001;
        //private const double _6 = 0.000001;
        //private const double _7 = 0.0000001;



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:Ausdruckskörper für Methoden verwenden", Justification = "<Ausstehend>")]
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static bool IsNormal(this double d)
        {
#if NET40
            long bits = BitConverter.DoubleToInt64Bits(d);

            bits &= 0x7FFFFFFFFFFFFFFF;
            return (bits < 0x7FF0000000000000) && (bits != 0) && ((bits & 0x7FF0000000000000) != 0);
#else
            return double.IsNormal(d);
#endif
        }


        //public static bool Equals6DigitPrecision(this double left, double right)
        //{
        //    bool? res = CheckAnormalValues(left, right);

        //    return res ?? Math.Abs(left - right) < _6;
        //}


        //public static int GetHashcode6DigitPrecision(this double d)
        //    => IsNormal(d) ? Math.Floor(d * 1000000).GetHashCode() : d.GetHashCode();


        //private static bool? CheckAnormalValues(double left, double right)
        //{
        //    if (double.IsNaN(left))
        //    {
        //        return double.IsNaN(right);
        //    }

        //    if (double.IsNaN(right))
        //    {
        //        return double.IsNaN(left);
        //    }

        //    if (double.IsNegativeInfinity(left))
        //    {
        //        return double.IsNegativeInfinity(right);
        //    }

        //    if (double.IsNegativeInfinity(right))
        //    {
        //        return double.IsNegativeInfinity(left);
        //    }

        //    if (double.IsPositiveInfinity(left))
        //    {
        //        return double.IsPositiveInfinity(right);
        //    }

        //    if (double.IsPositiveInfinity(right))
        //    {
        //        return double.IsPositiveInfinity(left);
        //    }

        //    return null;
        //}

        
    }
}
