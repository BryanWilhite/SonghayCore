#if !NET35 && !NET40
using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ValidationContext"/>
    /// </summary>
    public static class ValidationContextExtensions
    {
        /// <summary>
        /// Converts the <see cref="ValidationResult"/> into a display text.
        /// </summary>
        /// <param name="result">The result.</param>
        public static string ToDisplayText(this ValidationResult result)
        {
            if (result == null) return null;
            return string.Format("ErrorMessage: {0}, Properties: {1}", result.ErrorMessage, string.Join(",", result.MemberNames));
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{ValidationResult}"/> into a display text.
        /// </summary>
        /// <param name="results">The results.</param>
        public static string ToDisplayText(this IEnumerable<ValidationResult> results)
        {
            if (results == null) return null;
            var s = string.Format("Count {0}:{1}", results.Count(), Environment.NewLine);
            results.ForEachInEnumerable(i =>
            {
                s += string.Format("{0}{1}", i.ToDisplayText(), Environment.NewLine);
            });

            return s;
        }

        /// <summary>
        /// Converts the <see cref="Object"/> into a validation context.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">The expected object to validate is not here.</exception>
        public static ValidationContext ToValidationContext(this object objectToValidate)
        {
            if (objectToValidate == null) throw new NullReferenceException("The expected object to validate is not here.");
            return new ValidationContext(objectToValidate);
        }

        /// <summary>
        /// Converts the <see cref="Object"/> into a validation results.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> ToValidationResults(this object objectToValidate)
        {
            if (objectToValidate == null) return Enumerable.Empty<ValidationResult>();

            var results = new List<ValidationResult>();
            Validator.TryValidateObject(objectToValidate, objectToValidate.ToValidationContext(), results, validateAllProperties: true);
            return results;
        }

        /// <summary>
        /// Converts the <see cref="Object"/> into a validation results.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The expected Property Name to validate is not here.</exception>
        public static IEnumerable<ValidationResult> ToValidationResults(this object objectToValidate, object propertyValue, [CallerMemberName] string propertyName = "")
        {
            if (objectToValidate == null) return Enumerable.Empty<ValidationResult>();
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("The expected Property Name to validate is not here.");

            var results = new List<ValidationResult>();
            var context = objectToValidate.ToValidationContext();
            context.MemberName = propertyName;
            Validator.TryValidateProperty(propertyValue, context, results);
            return results;
        }
    }
}
#endif