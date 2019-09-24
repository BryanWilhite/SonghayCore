using Newtonsoft.Json.Linq;
using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="JToken"/>
    /// </summary>
    public static class JTokenExtensions
    {
        /// <summary>
        /// Gets the value of the specified <see cref="JToken"/>
        /// </summary>
        /// <typeparam name="TValue">type of returned value</typeparam>
        /// <param name="jToken">the <see cref="JToken"/></param>
        /// <param name="throwException">when <c>true</c> throw and exception when value is not here</param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this JToken jToken, bool throwException = true)
        {
            if(jToken == null)
            {
                if (throwException) throw new NullReferenceException($"The expected {nameof(JToken)} is not here.");
                return default(TValue);
            }

            var value = jToken.Value<TValue>();
            if (value == null)
            {
                if(throwException) throw new NullReferenceException($"The expected {nameof(JToken)} value is not here.");
                return default(TValue);
            }

            return value;
        }
    }
}
