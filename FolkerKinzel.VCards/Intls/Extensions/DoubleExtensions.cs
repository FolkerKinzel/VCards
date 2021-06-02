using System;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class DoubleExtensions
    {
        private const double _2 = 0.01;
        //private const double _3 = 0.001;
        //const double _4 = 0.0001;
        //const double _5 = 0.00001;
        //const double _6 = 0.000001;
        //const double _7 = 0.0000001;

        public static bool Equals2DigitPrecision(this double left, double right)
        {
            bool? res = CheckAnormalValues(left, right);

            return res.HasValue ? res.Value : Math.Abs(left - right) < _2;
        }


        private static bool? CheckAnormalValues(double left, double right)
        {
            if (double.IsNaN(left))
            {
                return double.IsNaN(right);
            }

            if (double.IsNaN(right))
            {
                return double.IsNaN(left);
            }

            if (double.IsNegativeInfinity(left))
            {
                return double.IsNegativeInfinity(right);
            }

            if (double.IsNegativeInfinity(right))
            {
                return double.IsNegativeInfinity(left);
            }

            if (double.IsPositiveInfinity(left))
            {
                return double.IsPositiveInfinity(right);
            }

            if (double.IsPositiveInfinity(right))
            {
                return double.IsPositiveInfinity(left);
            }

            return null;
        }

        //public static bool Equals3DigitPrecision(this double left, double right)
        //{
        //    return Math.Abs(left - right) < _3;
        //}

        //public static bool Equals4DigitPrecision(this double left, double right)
        //{
        //    return Math.Abs(left - right) < _4;
        //}
    }
}
