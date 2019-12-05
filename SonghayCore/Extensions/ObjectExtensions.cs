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
        /// Determines whether the specified throw exception is serializable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectOfDomain">The object of domain.</param>
        /// <returns>
        ///   <c>true</c> if the specified throw exception is serializable; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The expected serializable Type is not here.</exception>
        /// <remarks>
        /// For detail, see https://stackoverflow.com/a/945528/22944
        /// For background, see https://manski.net/2014/10/net-serializers-comparison-chart/
        /// </remarks>
        public static bool IsSerializable<T>(this object objectOfDomain) where T : new()
        {
            return objectOfDomain.IsSerializable<T>(shouldExtendISerializable: false, throwException: true);
        }

        /// <summary>
        /// Determines whether the specified throw exception is serializable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectOfDomain">The object of domain.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns>
        ///   <c>true</c> if the specified throw exception is serializable; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The expected serializable Type is not here.</exception>
        /// <remarks>
        /// For detail, see https://stackoverflow.com/a/945528/22944
        /// For background, see https://manski.net/2014/10/net-serializers-comparison-chart/
        /// </remarks>
        public static bool IsSerializable<T>(this object objectOfDomain, bool throwException) where T : new()
        {
            return objectOfDomain.IsSerializable<T>(shouldExtendISerializable: false, throwException);
        }

        /// <summary>
        /// Determines whether the specified throw exception is serializable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectOfDomain">The object of domain.</param>
        /// <param name="shouldExtendISerializable">if set to <c>true</c> should extend <see cref="ISerializable"/>.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns>
        ///   <c>true</c> if the specified throw exception is serializable; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The expected serializable Type is not here.</exception>
        /// <remarks>
        /// For detail, see https://stackoverflow.com/a/945528/22944
        /// For background, see https://manski.net/2014/10/net-serializers-comparison-chart/
        /// </remarks>
        public static bool IsSerializable<T>(this object objectOfDomain, bool shouldExtendISerializable, bool throwException) where T : new()
        {
            var test = typeof(T).IsSerializable;

            if(shouldExtendISerializable)
                test = test && typeof(ISerializable).IsAssignableFrom(typeof(T));

            if (!test && throwException)
                throw new InvalidOperationException("The expected serializable Type is not here.");

            return test;
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
