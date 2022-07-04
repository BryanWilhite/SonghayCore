using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Songhay.Models
{
    /// <summary>
    /// JSON.NET Contract Resolver to serialize a type through another type
    /// (usually an interface).
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    public class InterfaceContractResolverWithCamelCasing<TInterface> : CamelCasePropertyNamesContractResolver where TInterface : class
    {
        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(typeof(TInterface), memberSerialization);
            return properties;
        }
    }
}
