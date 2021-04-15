using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay;
using Songhay.Extensions;
using Songhay.Models;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ITemporal"/>
    /// </summary>
    public static class ITemporalExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="ITemporal"/> data
        /// to <see cref="JObject"/>
        /// </summary>
        /// <param name="data">the <see cref="ITemporal"/> data</param>
        /// <param name="useJavaScriptCase">when <c>true</c> use snakecase</param>
        /// <typeparam name="TData">a type derived from <see cref="ITemporal"/></typeparam>
        public static JObject ToJObject<TData>(this ITemporal data, bool useJavaScriptCase) where TData : class, ITemporal
        {
            if (data == null) return null;

            var settings = JsonSerializationUtility
                .GetConventionalResolver<TData>(useJavaScriptCase)
                .ToJsonSerializerSettings()
                .WithConventionalSettings()
                ;

            var jO = JObject.FromObject(data, JsonSerializer.Create(settings));

            return jO;
        }
    }
}