﻿namespace Songhay;

public static partial class ProgramUtility
{
    /// <summary>
    /// Gets the name of the current method.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string? GetCurrentMethodName() => GetMethodName(2);

    /// <summary>
    /// Gets the name of the current method.
    /// </summary>
    /// <param name="stackFrameIndex">Index of the stack frame.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string? GetMethodName(int stackFrameIndex)
    {
        var trace = new StackTrace();
        var frame = trace.GetFrame(stackFrameIndex);

        return frame?.GetMethod()?.Name;
    }
}
