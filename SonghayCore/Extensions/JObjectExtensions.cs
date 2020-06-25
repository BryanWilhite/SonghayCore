using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay.Models;
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
        /// Displays the top properties of <see cref="JObject"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        public static string DisplayTopProperties(this JObject jObject)
        {
            var properties = jObject?
                .Properties()
                .Select(p => p.Name);

            return ((properties != null) && properties.Any()) ?
                properties.Aggregate((a, name) => string.Concat(a, Environment.NewLine, name))
                :
                string.Empty;
        }

        /// <summary>
        /// Ensures the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">The expected {nameof(JObject)} is not here.</exception>
        public static JObject Ensure(this JObject jObject)
        {
            if (jObject == null)
            {
                throw new NullReferenceException($"The expected {nameof(JObject)} is not here.");
            }

            return jObject;
        }

        /// <summary>
        /// Converts to <c>TDomainData</c> from <see cref="JObject" />.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TDomainData">The type of the domain data.</typeparam>
        /// <param name="jObject">The <see cref="JObject" />.</param>
        /// <returns></returns>
        public static TDomainData FromJObject<TInterface, TDomainData>(this JObject jObject)
            where TDomainData : class
            where TInterface : class
        {
            return jObject.FromJObject<TInterface, TDomainData>(settings: null);
        }

        /// <summary>
        /// Converts to <c>TDomainData</c> from <see cref="JObject" />.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TDomainData">The type of the domain data.</typeparam>
        /// <param name="jObject">The <see cref="JObject" /> in the shape of <c>TInterface</c>.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings" />.</param>
        /// <returns></returns>
        /// <remarks>
        /// The default <see cref="JsonSerializerSettings" />
        /// from <see cref="IContractResolverExtensions.ToJsonSerializerSettings(Newtonsoft.Json.Serialization.IContractResolver)" />
        /// assumes <c>TDomainData</c> derives from an Interface.
        /// </remarks>
        public static TDomainData FromJObject<TInterface, TDomainData>(this JObject jObject, JsonSerializerSettings settings)
            where TDomainData : class
            where TInterface : class
        {
            if (jObject == null) return null;
            if (settings == null) settings = new InterfaceContractResolver<TInterface>().ToJsonSerializerSettings();

            var domainData = jObject.ToObject<TDomainData>(JsonSerializer.Create(settings));

            return domainData;
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="dictionaryPropertyName">Name of the dictionary property.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(this JObject jObject, string dictionaryPropertyName)
        {
            return jObject.GetDictionary(dictionaryPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
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
            else if (throwException) throw new FormatException($"The expected property name `{dictionaryPropertyName}` is not here.");

            var data = jO.ToObject<Dictionary<string, string>>();
            return data;
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetDictionary(this JObject jObject)
        {
            return jObject.GetDictionary(throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="throwException">when set to <c>true</c> then [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">jObject;The expected JObject is not here.</exception>
        public static Dictionary<string, string[]> GetDictionary(this JObject jObject, bool throwException)
        {
            if (throwException) jObject.Ensure();
            if ((jObject == null) && !throwException) return null;

            var data = jObject.ToObject<Dictionary<string, string[]>>();
            return data;
        }

        /// <summary>
        /// Gets the <see cref="JArray" /> or will throw <see cref="FormatException"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="arrayPropertyName">Name of the array property.</param>
        /// <returns></returns>
        public static JArray GetJArray(this JObject jObject, string arrayPropertyName)
        {
            return jObject.GetJArray(arrayPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="JArray" />.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
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
            else if (throwException) throw new FormatException($"The expected array `{arrayPropertyName}` is not here.");

            return jsonArray;
        }

        /// <summary>
        /// Gets the <see cref="JObject"/> by the specified property name.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <returns></returns>
        public static JObject GetJObject(this JObject jObject, string objectPropertyName)
        {
            return jObject.GetJObject(objectPropertyName, throwException: true);
        }

        /// <summary>
        /// Gets the <see cref="JObject"/> by the specified property name.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <param name="throwException">when set to <c>true</c> then throw exception.</param>
        /// <returns></returns>
        public static JObject GetJObject(this JObject jObject, string objectPropertyName, bool throwException)
        {
            return jObject.GetValue<JObject>(objectPropertyName, throwException);
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
            return jObject.GetValue<JToken>(objectPropertyName, throwException);
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> from <see cref="JArray" /> or will throw <see cref="FormatException"/>.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
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
        /// <param name="jObject">The <see cref="JObject"/>.</param>
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
                var errorMessage = $"The expected JToken in the JArray at index {arrayIndex} is not here.";
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
        /// <typeparam name="TValue">type of returned value</typeparam>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the <see cref="JObject" /> property.</param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this JObject jObject, string objectPropertyName)
        {
            return jObject.GetValue<TValue>(objectPropertyName, throwException: true);
        }

        /// <summary>
        /// Get the value by property name
        /// from the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="TValue">type of returned value</typeparam>
        /// <param name="jObject"></param>
        /// <param name="objectPropertyName"></param>
        /// <param name="throwException">when set to <c>true</c> then throw exception.</param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this JObject jObject, string objectPropertyName, bool throwException)
        {
            if (string.IsNullOrWhiteSpace(objectPropertyName)) throw new ArgumentNullException(nameof(objectPropertyName));
            if (throwException) jObject.Ensure();
            if ((jObject == null) && !throwException) return default(TValue);

            var jToken = jObject[objectPropertyName];
            if ((jToken == null) && throwException) throw new NullReferenceException($"The expected {nameof(JToken)} of `{objectPropertyName}` is not here.");
            return jToken.GetValue<TValue>(throwException);
        }

        /// <summary>
        /// Returns <c>true</c> when <see cref="JObject.Properties"/>
        /// has a <see cref="JProperty"/> with the specified name.
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="objectPropertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this JObject jObject, string objectPropertyName)
        {
            if (string.IsNullOrWhiteSpace(objectPropertyName)) throw new ArgumentNullException(nameof(objectPropertyName));
            if (jObject == null) return false;
            return jObject.Properties().Any(i => i.Name.EqualsInvariant(objectPropertyName));
        }

        /// <summary>
        /// Parses the <see cref="JObject"/>
        /// serialized as a string
        /// at the specified property name.
        /// 
        /// It will throw any exceptions.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        /// <returns></returns>
        public static JObject ParseJObject(this JObject jObject, string objectPropertyName)
        {
            return jObject.ParseJObject(objectPropertyName, throwException: true);
        }

        /// <summary>
        /// Parses the <see cref="JObject"/>
        /// serialized as a string
        /// at the specified property name.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        /// <param name="throwException">when <c>true</c> throw any exceptions; otherwise, return <c>null</c>.</param>
        /// <returns></returns>
        public static JObject ParseJObject(this JObject jObject, string objectPropertyName, bool throwException)
        {
            var json = jObject.GetValue<string>(objectPropertyName, throwException);

            JObject jO = null;
            try
            {
                jO = JObject.Parse(json);
            }
            catch (Exception)
            {
                if (throwException) throw;
            }

            return jO;
        }

        /// <summary>
        /// Ensures the <see cref="JObject"/> has the specified property.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        public static JObject WithProperty(this JObject jObject, string objectPropertyName)
        {
            return jObject.WithProperty(objectPropertyName, throwException: true);
        }

        /// <summary>
        /// Ensures the <see cref="JObject"/> has the specified property.
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/>.</param>
        /// <param name="objectPropertyName">Name of the object property.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public static JObject WithProperty(this JObject jObject, string objectPropertyName, bool throwException)
        {
            if (string.IsNullOrWhiteSpace(objectPropertyName)) throw new ArgumentNullException(nameof(objectPropertyName));
            if (throwException && !jObject.HasProperty(objectPropertyName))
            {
                jObject.Ensure();

                var msg = $@"
The expected property name `{objectPropertyName}` is not here.

Actual properties:
{jObject.DisplayTopProperties()}
".Trim();
                throw new FormatException(msg);
            }

            return jObject;
        }
    }
}
