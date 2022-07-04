using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IContractResolver"/>
    /// </summary>
    public static class IContractResolverExtensions
    {
        /// <summary>
        /// Converts the <see cref="IContractResolver" />
        /// to <see cref="JsonSerializerSettings" />.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static JsonSerializerSettings ToJsonSerializerSettings(this IContractResolver resolver)
        {
            return new JsonSerializerSettings
            {
                ContractResolver = resolver,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}
