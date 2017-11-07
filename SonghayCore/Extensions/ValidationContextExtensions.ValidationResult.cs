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
    }
}
