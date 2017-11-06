using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="JObject"/>
    /// </summary>
    public static class JObjectExtensions
    {
        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(this JObject jsonObject, string dictionaryPropertyName)
        {
            return jsonObject.GetDictionary(dictionaryPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">dictionaryPropertyName;The expected Dictionary Property Name is not here.</exception>
        /// <exception cref="System.FormatException"></exception>
        public static Dictionary<string, string> GetDictionary(this JObject jsonObject, string dictionaryPropertyName, bool throwException)
        {
            var token = jsonObject.GetJToken(dictionaryPropertyName, throwException);
            JObject jO = null;
            if (token.HasValues) jO = (JObject)token;
            else if (throwException) throw new FormatException(string.Format("The expected property name “{0}” is not here.", dictionaryPropertyName));

            var data = jO.ToObject<Dictionary<string, string>>();
            return data;
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetDictionary(this JObject jsonObject)
        {
            return jsonObject.GetDictionary(throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">jsonObject;The expected JObject is not here.</exception>
        public static Dictionary<string, string[]> GetDictionary(this JObject jsonObject, bool throwException)
        {
            if ((jsonObject == null) && !throwException) return null;
            if ((jsonObject == null) && throwException) throw new ArgumentNullException("jsonObject", "The expected JObject is not here.");

            var data = jsonObject.ToObject<Dictionary<string, string[]>>();
            return data;
        }

        /// <summary>
        /// Gets the <see cref="JArray" /> or will throw <see cref="FormatException"/>.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <returns></returns>
        public static JArray GetJArray(this JObject jsonObject, string arrayPropertyName)
        {
            return jsonObject.GetJArray(arrayPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="JArray" />.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <param name="throwException">if set to <c>true</c> throw <see cref="FormatException"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">arrayPropertyName;The expected JArray Property Name is not here.</exception>
        /// <exception cref="FormatException">
        /// </exception>
        public static JArray GetJArray(this JObject jsonObject, string arrayPropertyName, bool throwException)
        {
            var token = jsonObject.GetJToken(arrayPropertyName, throwException);
            if (token == null) return null;

            JArray jsonArray = null;
            if (token.HasValues) jsonArray = (JArray)token;
            else if (throwException) throw new FormatException(string.Format("The expected array “{0}” is not here.", arrayPropertyName));

            return jsonArray;
        }

        /// <summary>
        /// Gets the <see cref="JToken"/>.
        /// </summary>
        /// <param name="jsonObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// jsonObject;The expected JObject is not here.
        /// or
        /// objectPropertyName;The expected property name is not here.
        /// </exception>
        /// <exception cref="System.FormatException"></exception>
        public static JToken GetJToken(this JObject jsonObject, string objectPropertyName, bool throwException)
        {
            if ((jsonObject == null) && !throwException) return null;
            if ((jsonObject == null) && throwException) throw new ArgumentNullException("jsonObject", "The expected JObject is not here.");
            if (string.IsNullOrEmpty(objectPropertyName)) throw new ArgumentNullException("objectPropertyName", "The expected property name is not here.");

            if (!jsonObject.TryGetValue(objectPropertyName, out JToken token) && throwException)
                throw new FormatException(string.Format("The expected property name “{0}” is not here.", objectPropertyName));

            return token;
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> from <see cref="JArray" /> or will throw <see cref="FormatException"/>.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <returns></returns>
        public static JToken GetJTokenFromJArray(this JObject jsonObject, string arrayPropertyName, string objectPropertyName, int arrayIndex)
        {
            return jsonObject.GetJTokenFromJArray(arrayPropertyName, objectPropertyName, arrayIndex, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> from <see cref="JArray" />.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <param name="throwException">if set to <c>true</c> throw <see cref="FormatException"/>.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">objectPropertyName;The expected JObject Property Name is not here.</exception>
        public static JToken GetJTokenFromJArray(this JObject jsonObject, string arrayPropertyName, string objectPropertyName, int arrayIndex, bool throwException)
        {
            var jsonArray = jsonObject.GetJArray(arrayPropertyName, throwException);
            if (jsonArray == null) return null;

            var jsonToken = jsonArray.ElementAtOrDefault(arrayIndex);
            if (jsonToken == default(JToken))
            {
                var errorMessage = string.Format("The expected JToken in the JArray at index {0} is not here.", arrayIndex);
                if (throwException) throw new NullReferenceException(errorMessage);
                else return null;
            }

            var jO = jsonToken as JObject;
            if (jsonToken == null)
            {
                var errorMessage = "The expected JObject of the JToken is not here.";
                if (throwException) throw new NullReferenceException(errorMessage);
                else return null;
            }

            jsonToken = jO.GetJToken(objectPropertyName, throwException);
            return jsonToken;
        }
    }
}
