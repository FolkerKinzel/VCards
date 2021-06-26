using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Intls.Attributes
{
    /// <summary>
    /// Das Attribut simuliert einen Zugriffsmodifizierer, mit dem Methoden, Eigenschaften und Konstruktoren, die ihn nutzen 
    /// außerhalb der Assembly unsichtbar (internal) und innerhalb der Assembly nur für abgeleitete Klassen sichtbar sind 
    /// (internal UND protected). Member, die ihn nutzen, müssen als internal deklariert sein und als erstes die Methode 
    /// InternalProtectedAttribute.Run() ausführen. Die Methode führt die Überprüfung durch (nur im Debug-Modus), das Attribut 
    /// dient nur zur Information. In Properties muss InternalProtectedAttribute.Run() auf jedem Accessor ausgeführt werden. 
    /// Wird eine Methode oder Property überschrieben oder verdeckt, muss InternalProtectedAttribute.Run() auf der verdeckenden 
    /// oder überschreibenden Methode bzw. Property ausgeführt werden: Es genügt nicht, wenn die base-Property oder Methode 
    /// aufgerufen wird. InternalProtected kann nicht verhindern, dass ein Aufruf von einem fremden Objekt aus erfolgt, dessen Klasse 
    /// der gleichen Verebungshierarchie angehört.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method |
        AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [Conditional("DEBUG")]
    [ExcludeFromCodeCoverage]
    internal sealed class InternalProtectedAttribute : Attribute
    {
        /// <summary>
        /// Die Methode prüft im Debug-Modus, ob der Aufrufer den Zugriffsmodifizierer internal nutzt (Warnmeldung, wenn nicht) 
        /// und von einer abgeleiteten Klasse oder einer Basisklasse aufgerufen wurde. Ist dies nicht der Fall, wird eine 
        /// InvalidOperationException geworfen. Die Methode kann nicht verhindern, dass der Aufruf von einem fremden Objekt aus erfolgt, dessen 
        /// Klasse der gleichen Verebungshierarchie angehört.
        /// </summary>
        /// <exception cref="InvalidOperationException">Ein InternalProtected deklarierter Konstruktor bzw. eine InternalProtected deklarierte 
        /// Eigenschaft oder Methode wurde nicht von einer abgeleiteten Klasse aufgerufen.</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [Conditional("DEBUG")]
        [DebuggerStepThrough()]
        public static void Run()
        {
            var sfCallee = new StackFrame(1, false);
            MethodBase? calleeMethod = sfCallee.GetMethod();
            Type? calleeType = calleeMethod?.DeclaringType;

            var sfCaller = new StackFrame(2, false);
            MethodBase? callerMethod = sfCaller.GetMethod();
            Type? callerType = callerMethod?.DeclaringType;

            Debug.Assert(calleeMethod?.IsAssembly ?? false, "Die Verwendung des Attributs \"InternalProtected\" ist nur auf Methoden, " +
                "Eigenschaften und Konstruktoren sinnvoll, die den Zugriffsmodifizierer \"internal\" verwenden.");

            if (IsAssignableFrom(callerType, calleeType) || IsAssignableFrom(calleeType, callerType))
            {
                return;
            }

            const string unknown = "<unbekannt>";

            const string format = "Die Methode \"{0}.{1}\" wurde von der Methode \"{2}.{3}\" aufgerufen. " +
                "Sie ist aber \"InternalProtected\" deklariert und darf nur von abgeleiteten Typen oder Basistypen aufgerufen werden.";
            string message = string.Format(CultureInfo.CurrentCulture, format,

                calleeMethod.DeclaringType?.Name ?? unknown,

                calleeMethod.Name,

                callerMethod?.DeclaringType?.Name ?? unknown,

                callerMethod?.Name ?? unknown);

            throw new InvalidOperationException(message);
        }


        [DebuggerStepThrough()]
        private static bool IsAssignableFrom(Type? extendType, Type? baseType)
        {
            if(extendType is null || baseType is null)
            {
                return false;
            }

            while (!baseType.IsAssignableFrom(extendType))
            {
                if (extendType.Equals(typeof(object)))
                {
                    return false;
                }
                
                extendType = 
                    extendType.IsGenericType && !extendType.IsGenericTypeDefinition
                    ? extendType.GetGenericTypeDefinition()
                    : extendType.BaseType ?? typeof(object);
            }
            return true;
        }

    }//class
}//ns
