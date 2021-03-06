﻿using System.Diagnostics;
using Xunit;

namespace Songhay.Tests
{
    /// <summary>
    /// Extends <see cref="TheoryAttribute"/> to skip
    /// when <see cref="Debugger.IsAttached"/> is <c>false</c>.
    /// </summary>
    public sealed class DebuggerAttachedTheoryAttribute : TheoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerAttachedTheoryAttribute"/> class.
        /// </summary>
        public DebuggerAttachedTheoryAttribute()
        {
            if (!Debugger.IsAttached) this.Skip = "This test is intended to run when a Debugger is attached.";
        }
    }
}
