using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="object"/>.
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Determines whether this instance is type.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="objectOfDomain">The object of domain.</param>
        public static bool IsType<TClass>(this object objectOfDomain) where TClass : class
        {
            return (objectOfDomain as TClass) != null;
        }

        /// <summary>
        /// Boxes the value in object or returns <see cref="DBNull"/>.
        /// </summary>
        /// <param name="objectOfDomain">The object of domain.</param>
        public static object ToObjectOrDBNull(this object objectOfDomain)
        {
            return objectOfDomain ?? DBNull.Value;
        }
    }
}
