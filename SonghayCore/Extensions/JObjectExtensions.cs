using Newtonsoft.Json;
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
        /// Gets the dictionary.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(this JObject jsonObject, string dictionaryPropertyName)
        {
            return jsonObject.GetDictionary(dictionaryPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the dictionary.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">dictionaryPropertyName;The expected Dictionary Property Name is not here.</exception>
        /// <exception cref="System.FormatException"></exception>
        public static Dictionary<string, string> GetDictionary(this JObject jsonObject, string dictionaryPropertyName, bool throwException)
        {
            if (jsonObject == null) return null;
            if (string.IsNullOrEmpty(dictionaryPropertyName)) throw new ArgumentNullException("dictionaryPropertyName", "The expected Dictionary Property Name is not here.");

            var jO = jsonObject[dictionaryPropertyName];
            if ((jO == null) && throwException) throw new FormatException(string.Format("The expected property name “{0}” is not here.", dictionaryPropertyName));

            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jO.ToString());
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
            var jsonArray = JArray.Parse("[]");

            if (jsonObject == null) return jsonArray;
            if (string.IsNullOrEmpty(arrayPropertyName)) throw new ArgumentNullException("arrayPropertyName", "The expected JArray Property Name is not here.");

            var jO = jsonObject[arrayPropertyName];
            if ((jO == null) && throwException) throw new FormatException(string.Format("The expected property name “{0}” is not here.", arrayPropertyName));

            if (jO != null) jsonArray = JArray.FromObject(jO);
            if (!jsonArray.Any() && throwException) throw new FormatException(string.Format("The array “{0}” is not here.", arrayPropertyName));

            return jsonArray;
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

            if (string.IsNullOrEmpty(objectPropertyName)) throw new ArgumentNullException("objectPropertyName", "The expected JObject Property Name is not here.");

            var jsonToken = jsonArray[arrayIndex];
            return jsonToken[objectPropertyName];
        }
    }
}
