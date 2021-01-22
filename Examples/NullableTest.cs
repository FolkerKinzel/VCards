using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    internal static class NullableTest
    {
        internal static void CallIt()
        {
            var notNullable = new List<object>();
            var nullable = new List<object?>();

            F1(notNullable);
            F1(nullable);
            F1(null);

            F2(notNullable);
            F2(nullable);

            notNullable.F2();
            nullable.F2();

            F3(notNullable);
            F3(nullable);

            F4(notNullable);
            F4(nullable);

            F5(notNullable);
            F5(nullable);

            F6(notNullable);
            F6(nullable);

            notNullable = F4(notNullable);
            nullable = F4(nullable);

        }


        private static void F1(this List<
            # nullable disable
            object
            #nullable restore
            > list)
        {

        }



        private static void F2(this List<object?> list)
        {

        }


        private static void F3(this IEnumerable<object> list)
        {

        }


        private static List<object> F4(this IEnumerable<object?> list)
        {
            return new List<object>(list.Where( x => x != null)!);
        }


        private static void F5(this IList<object> list)
        {

        }


        private static void F6(this IList<object?> list)
        {

        }
    }
}
