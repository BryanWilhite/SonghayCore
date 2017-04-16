using System;
using System.Linq;
using System.Reflection;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="objectWithProperties">The object with properties.</param>
        public static PropertyInfo[] GetProperties(this object objectWithProperties)
        {
            if (objectWithProperties == null) return Enumerable.Empty<PropertyInfo>().ToArray();
            return objectWithProperties.GetType().GetProperties();
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="objectWithProperties">The object with properties.</param>
        /// <param name="propertyName">Name of the property.</param>
        public static PropertyInfo GetProperty(this object objectWithProperties, string propertyName)
        {
            var props = objectWithProperties.GetProperties();
            if (!props.Any()) return null;
            return props.FirstOrDefault(i => i.Name == propertyName);
        }

#if !NETSTANDARD1_2 && !SILVERLIGHT && !PCL && !NET35 && !NET40

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="objectWithProperties">The object with properties.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <remarks>
        /// Very useful for an MVVM situation like this:
        /// <code>
        ///     var command = this.DataContext.GetPropertyValue("MyCommand") as ICommand;
        /// </code>
        /// </remarks>
        public static object GetPropertyValue(this object objectWithProperties, string propertyName)
        {
            var property = objectWithProperties.GetProperty(propertyName);
            if (property == null) return null;
            return property.GetValue(objectWithProperties);
        }

#endif

#if !NETSTANDARD1_2 && !PCL

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
        /// Boxes the value in object or returns <see cref="System.DBNull"/>.
        /// </summary>
        /// <param name="objectOfDomain">The object of domain.</param>
        public static object ToObjectOrDBNull(this object objectOfDomain)
        {
            return objectOfDomain ?? DBNull.Value;
        }
#endif
    }
}
