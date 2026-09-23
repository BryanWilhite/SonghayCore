namespace Songhay.Models;

/// <summary>
/// Defines the general-purpose Result concept
/// of Program completion,
/// avoiding third-party dependencies
/// that are not compatible with F#.
/// </summary>
/// <param name="IsSuccessful">when <c>true</c> the Program completed without exceptions.</param>
/// <param name="Message">a domain-specific message summarizing the Program completion</param>
/// <remarks>
/// This type (and its possible subtypes) is intended
/// to define the output of Activities
/// (like <see cref="IActivityTask{TInput, TOuput}"/>
/// or <see cref="IActivityTaskGroup{TOutput}"/>).
/// </remarks>
public record ProgramResult(
    bool IsSuccessful,
    string? Message
);
