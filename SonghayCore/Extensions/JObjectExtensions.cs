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
        /// <param name="jObject">The json object.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(this JObject jObject, string dictionaryPropertyName)
        {
            return jObject.GetDictionary(dictionaryPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <param name="throwException">when set to <c>true</c> then [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">dictionaryPropertyName;The expected Dictionary Property Name is not here.</exception>
        /// <exception cref="System.FormatException"></exception>
        public static Dictionary<string, string> GetDictionary(this JObject jObject, string dictionaryPropertyName, bool throwException)
        {
            var token = jObject.GetJToken(dictionaryPropertyName, throwException);
            JObject jO = null;
            if (token.HasValues) jO = (JObject)token;
            else if (throwException) throw new FormatException(string.Format("The expected property name “{0}” is not here.", dictionaryPropertyName));

            var data = jO.ToObject<Dictionary<string, string>>();
            return data;
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetDictionary(this JObject jObject)
        {
            return jObject.GetDictionary(throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <param name="throwException">when set to <c>true</c> then [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">jObject;The expected JObject is not here.</exception>
        public static Dictionary<string, string[]> GetDictionary(this JObject jObject, bool throwException)
        {
            if ((jObject == null) && !throwException) return null;
            if ((jObject == null) && throwException) throw new ArgumentNullException("jObject", "The expected JObject is not here.");

            var data = jObject.ToObject<Dictionary<string, string[]>>();
            return data;
        }

        /// <summary>
        /// Gets the <see cref="JArray" /> or will throw <see cref="FormatException"/>.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <returns></returns>
        public static JArray GetJArray(this JObject jObject, string arrayPropertyName)
        {
            return jObject.GetJArray(arrayPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="JArray" />.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <param name="throwException">when set to <c>true</c> then throw <see cref="FormatException"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">arrayPropertyName;The expected JArray Property Name is not here.</exception>
        /// <exception cref="FormatException">
        /// </exception>
        public static JArray GetJArray(this JObject jObject, string arrayPropertyName, bool throwException)
        {
            var token = jObject.GetJToken(arrayPropertyName, throwException);
            if (token == null) return null;

            JArray jsonArray = null;
            if (token.HasValues) jsonArray = (JArray)token;
            else if (throwException) throw new FormatException(string.Format("The expected array “{0}” is not here.", arrayPropertyName));

            return jsonArray;
        }

        /// <summary>
        /// Gets the <see cref="JObject"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <param name="throwException">when set to <c>true</c> then throw exception.</param>
        /// <returns></returns>
        public static JObject GetJObject(this JObject jObject, string objectPropertyName, bool throwException)
        {
            if ((jObject == null) && !throwException) return null;
            if ((jObject == null) && throwException) throw new NullReferenceException($"The expected {nameof(JObject)} is not here.");
            return jObject[objectPropertyName]?.GetValue<JObject>(throwException);
        }

        /// <summary>
        /// Gets the <see cref="JToken"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <param name="throwException">when set to <c>true</c> then throw exception.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// jObject;The expected JObject is not here.
        /// or
        /// objectPropertyName;The expected property name is not here.
        /// </exception>
        /// <exception cref="System.FormatException"></exception>
        public static JToken GetJToken(this JObject jObject, string objectPropertyName, bool throwException)
        {
            if ((jObject == null) && !throwException) return null;
            if ((jObject == null) && throwException) throw new NullReferenceException($"The expected {nameof(JObject)} is not here.");
            return jObject[objectPropertyName]?.GetValue<JToken>(throwException);
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> from <see cref="JArray" /> or will throw <see cref="FormatException"/>.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <returns></returns>
        public static JToken GetJTokenFromJArray(this JObject jObject, string arrayPropertyName, string objectPropertyName, int arrayIndex)
        {
            return jObject.GetJTokenFromJArray(arrayPropertyName, objectPropertyName, arrayIndex, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> from <see cref="JArray" />.
        /// </summary>
        /// <param name="jObject">The json object.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <param name="throwException">when set to <c>true</c> then throw <see cref="FormatException"/>.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">objectPropertyName;The expected JObject Property Name is not here.</exception>
        public static JToken GetJTokenFromJArray(this JObject jObject, string arrayPropertyName, string objectPropertyName, int arrayIndex, bool throwException)
        {
            var jsonArray = jObject.GetJArray(arrayPropertyName, throwException);
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

        /// <summary>
        /// Get the value by property name
        /// from the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="objectPropertyName"></param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this JObject jObject, string objectPropertyName)
        {
            return jObject.GetValue<TValue>(throwException: true);
        }

        /// <summary>
        /// Get the value by property name
        /// from the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="objectPropertyName"></param>
        /// <param name="throwException">when set to <c>true</c> then throw exception.</param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this JObject jObject, string objectPropertyName, bool throwException)
        {
            if ((jObject == null) && !throwException) return default(TValue);
            if ((jObject == null) && throwException) throw new NullReferenceException($"The expected {nameof(JObject)} is not here.");
            return jObject[objectPropertyName].GetValue<TValue>(throwException);
        }
    }
}
