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
    public static partial class ValidationContextExtensions
    {
        /// <summary>
        /// Converts the <see cref="Object"/> into a validation results.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The expected Property Name to validate is not here.</exception>
        public static IEnumerable<ValidationResult> ToValidationResults(this object objectToValidate, object propertyValue, [CallerMemberName] string propertyName = "")
        {
            if (objectToValidate == null) return Enumerable.Empty<ValidationResult>();
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("The expected Property Name to validate is not here.");

            var results = new List<ValidationResult>();
            var context = objectToValidate.ToValidationContext();
            context.MemberName = propertyName;
            Validator.TryValidateProperty(propertyValue, context, results);
            return results;
        }
    }
}
