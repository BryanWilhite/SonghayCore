using Newtonsoft.Json.Serialization;
using Songhay.Models;

namespace Songhay
{
    /// <summary>
    /// static members for JSON serialization
    /// </summary>
    public static class JsonSerializationUtility
    {
        /// <summary>
        /// Gets the conventional resolver.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="useJavaScriptCase">if set to <c>true</c> [use java script case].</param>
        /// <returns></returns>
        public static IContractResolver GetConventionalResolver<TInterface>(bool useJavaScriptCase) where TInterface : class
        {
            return useJavaScriptCase ?
                new InterfaceContractResolverWithCamelCasing<TInterface>() as IContractResolver
                :
                new InterfaceContractResolver<TInterface>() as IContractResolver
                ;
        }
    }
}
