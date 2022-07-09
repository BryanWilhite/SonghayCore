using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ValidationResult" /> and <see cref="IValidatableObject" />,
/// returning a <see cref="ValidationContext"/>.
/// </summary>
/// <remarks>
/// The use of these methods should be the last resort
/// after deferring to a NuGet package like FluentValidation.
/// </remarks>
public static class ValidationContextExtensions
{
    /// <summary>
    /// Converts the <see cref="ValidationResult"/> into a display text.
    /// </summary>
    /// <param name="result">The result.</param>
    public static string ToDisplayText(this ValidationResult? result) => result == null
        ? DisplayErrorMessage
        : $"Message: {result.ErrorMessage};Properties: {string.Join(",", result.MemberNames).Trim(new[] {','})}";

    /// <summary>
    /// Converts the <see cref="IEnumerable{ValidationResult}"/> into a display text.
    /// </summary>
    /// <param name="results">The results.</param>
    public static string ToDisplayText(this IEnumerable<ValidationResult>? results)
    {
        if (results == null) return DisplayErrorMessage;

        results = results.ToArray();

        var resultsCount = results.Count();

        if (resultsCount == 0) return DisplayErrorMessage;

        var builder = new StringBuilder();
        builder.Append($"Count: {resultsCount}");
        builder.AppendLine();

        foreach (var result in results)
        {
            builder.Append(result.ToDisplayText());
            builder.AppendLine();
        }

        return builder.ToString();
    }

    /// <summary>
    /// Converts the <see cref="Object"/> into a validation context.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">The expected object to validate is not here.</exception>
    public static ValidationContext ToValidationContext(this IValidatableObject objectToValidate) =>
        objectToValidate == null
            ? throw new NullReferenceException("The expected object to validate is not here.")
            : new ValidationContext(objectToValidate);

    /// <summary>
    /// Converts the <see cref="Object" /> into a validation results.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <remarks>
    /// This member will validate all properties;<c>validateAllProperties == true</c>.
    /// </remarks>
    /// <returns></returns>
    public static IEnumerable<ValidationResult> ToValidationResults(this IValidatableObject? objectToValidate) =>
        objectToValidate.ToValidationResults(validateAllProperties: true, validationContext: null);

    /// <summary>
    /// Converts the <see cref="Object" /> into a validation results.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <param name="validationContext">the <see cref="ValidationContext"/></param>
    /// <remarks>
    /// This member will validate all properties;<c>validateAllProperties == true</c>.
    /// </remarks>
    /// <returns></returns>
    public static IEnumerable<ValidationResult> ToValidationResults(this IValidatableObject? objectToValidate,
        ValidationContext? validationContext) =>
        objectToValidate.ToValidationResults(validateAllProperties: true, validationContext: validationContext);

    /// <summary>
    /// Converts the <see cref="Object" /> into a validation results.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <param name="validateAllProperties"><c>true</c> to validate all properties;if <c>false</c>, only required attributes are validated.</param>
    /// <param name="validationContext">the <see cref="ValidationContext"/></param>
    /// <returns></returns>
    public static IEnumerable<ValidationResult> ToValidationResults(this IValidatableObject? objectToValidate,
        bool validateAllProperties, ValidationContext? validationContext)
    {
        if (objectToValidate == null) return Enumerable.Empty<ValidationResult>();
        validationContext ??= objectToValidate.ToValidationContext();

        var results = new List<ValidationResult>();

        Validator.TryValidateObject(objectToValidate, validationContext, results, validateAllProperties);

        return results;
    }

    /// <summary>
    /// Converts the <see cref="Object"/> into a validation results.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">propertyName</exception>
    public static IEnumerable<ValidationResult> ToValidationResults(this IValidatableObject? objectToValidate,
        string? propertyName, object? propertyValue) =>
        objectToValidate.ToValidationResults(propertyName, propertyValue, validationContext: null);

    /// <summary>
    /// Converts the <see cref="Object"/> into a validation results.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <param name="validationContext">the <see cref="ValidationContext"/></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">propertyName</exception>
    public static IEnumerable<ValidationResult> ToValidationResults(this IValidatableObject? objectToValidate,
        string? propertyName, object? propertyValue, ValidationContext? validationContext)
    {
        if (objectToValidate == null) return Enumerable.Empty<ValidationResult>();
        validationContext ??= objectToValidate.ToValidationContext();
        if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

        var results = new List<ValidationResult>();
        validationContext.MemberName = propertyName;

        Validator.TryValidateProperty(propertyValue, validationContext, results);

        return results;
    }

    internal const string DisplayErrorMessage = $"[Unable to display {nameof(ValidationResult)}s]";
}
