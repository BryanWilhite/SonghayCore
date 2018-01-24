using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

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
            return string.Format("Message: {0}; Properties: {1}",
                result.ErrorMessage, string.Join(",", result.MemberNames).Trim(new[] { ',' }));
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{ValidationResult}"/> into a display text.
        /// </summary>
        /// <param name="results">The results.</param>
        public static string ToDisplayText(this IEnumerable<ValidationResult> results)
        {
            if (results == null) return null;
            if (!results.Any()) return null;

            var builder = new StringBuilder();
            builder.AppendFormat("Count: {0}", results.Count());
            builder.AppendLine();
            results.ForEachInEnumerable(i =>
            {
                builder.Append(i.ToDisplayText());
                builder.AppendLine();
            });

            return builder.ToString();
        }
    }
}
