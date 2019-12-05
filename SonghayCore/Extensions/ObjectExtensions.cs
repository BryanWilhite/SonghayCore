using System;
using System.Runtime.Serialization;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="object"/>.
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the specified type
        /// can be assigned to <see cref="ISerializable" />.
        /// </summary>
        /// <typeparam name="T">the specified type</typeparam>
        /// <param name="objectOfDomain">The object of domain.</param>
        /// <returns>
        ///   <c>true</c> if the specified throw exception is serializable; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The expected serializable Type is not here.</exception>
        /// <remarks>
        /// For detail, see https://stackoverflow.com/a/945528/22944.
        /// For background, see https://manski.net/2014/10/net-serializers-comparison-chart/
        /// and https://github.com/BryanWilhite/SonghayCore/issues/76
        /// </remarks>
        public static bool IsAssignableToISerializable<T>(this T objectOfDomain)
        {
            return typeof(ISerializable).IsAssignableFrom(typeof(T));
        }

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
