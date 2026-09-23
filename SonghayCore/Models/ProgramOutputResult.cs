namespace Songhay.Models;

/// <summary>
/// Defines the general-purpose Result concept
/// for Program output with output,
/// avoiding third-party dependencies
/// that are not compatible with F#.
/// </summary>
/// <typeparam name="TOutput">nullable value or reference types</typeparam>
/// <param name="IsSuccessful">when <c>true</c> the Program completed without exceptions.</param>
/// <param name="Message">a domain-specific message summarizing the Program completion</param>
/// <param name="Output">the output of the Program</param>
/// <remarks>
/// This type (and its possible subtypes) is intended
/// to define the output of Activities
/// (like <see cref="IActivityTask{TInput, TOuput}"/>
/// or <see cref="IActivityTaskGroup{TOutput}"/>).
/// </remarks>
public record ProgramOutputResult<TOutput>(
    bool IsSuccessful,
    string? Message,
    TOutput? Output
): ProgramResult(
    IsSuccessful,
    Message
);