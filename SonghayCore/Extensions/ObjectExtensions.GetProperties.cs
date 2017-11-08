using System.Linq;
using System.Reflection;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="object"/>.
    /// </summary>
    public static partial class ObjectExtensions
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
        ///
        /// Note that this member uses <c>property.GetValue(objectWithProperties, index: null)</c>.
        /// Passing null is “fine for normal simple properties, this will fail with indexer properties,
        /// which take a non-null argument list as specified by <c>PropertyInfo.GetIndexParameters</c>.”
        /// [https://stackoverflow.com/a/1355110/22944]
        /// </remarks>
        public static object GetPropertyValue(this object objectWithProperties, string propertyName)
        {
            var property = objectWithProperties.GetProperty(propertyName);
            if (property == null) return null;
            return property.GetValue(objectWithProperties, index: null);
        }
    }
}
