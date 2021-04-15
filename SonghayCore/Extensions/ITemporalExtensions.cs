using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay.Models;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ITemporal"/>
    /// </summary>
    public static class ITemporalExtensions
    {
        /// <summary>
        /// Sets conventional default values
        /// for <see cref="ITemporal"/> data.
        /// </summary>
        /// <param name="data">the <see cref="ITemporal"/> data</param>
        public static void SetDefaults(this ITemporal data)
        {
            data.SetDefaults(endDate: null);
        }

        /// <summary>
        /// Sets conventional default values
        /// for <see cref="ITemporal"/> data.
        /// </summary>
        /// <param name="data">the <see cref="ITemporal"/> data</param>
        /// <param name="endDate">sets <see cref="ITemporal.EndDate"/></param>
        public static void SetDefaults(this ITemporal data, DateTime? endDate)
        {
            if (data == null) return;

            data.InceptDate = DateTime.Now;
            data.ModificationDate = data.InceptDate;
            data.EndDate = endDate;
        }

        /// <summary>
        /// Converts the specified <see cref="ITemporal"/> data
        /// to <see cref="JObject"/>.
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
