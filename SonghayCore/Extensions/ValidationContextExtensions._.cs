using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ValidationContext"/>
    /// </summary>
    public static partial class ValidationContextExtensions
    {
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
    }
}
