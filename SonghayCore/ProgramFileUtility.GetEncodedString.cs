﻿using System;
using System.Text;

namespace Songhay
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.IO"/>.
    /// </summary>
    public static partial class ProgramFileUtility
    {
        /// <summary>
        /// Gets the UTF-8 encoded string from.
        /// </summary>
        /// <param name="utf16Value">The raw value.</param>
        public static string GetEncodedString(string utf16Value)
        {
            return ProgramFileUtility
                .GetEncodedString(utf16Value, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the encoded <see cref="string"/>
        /// from its default <see cref="Encoding.Unicode"/> encoding.
        /// </summary>
        /// <param name="utf16Value">The raw value.</param>
        /// <param name="outputEncoding">The output encoding.</param>
        /// <remarks>
        /// <see cref="Encoding.Unicode"/> encoding is the UTF-16
        /// encoding of strings in .NET.
        /// See: https://docs.microsoft.com/en-us/dotnet/api/system.text.unicodeencoding
        /// </remarks>
        public static string GetEncodedString(string utf16Value, Encoding outputEncoding)
        {
            if (outputEncoding == null) throw new ArgumentNullException(nameof(outputEncoding));

            byte[] byteArray = Encoding.Convert(
                Encoding.Unicode,
                outputEncoding,
                Encoding.Unicode.GetBytes(utf16Value));

            return new string(outputEncoding.GetChars(byteArray));
        }
    }
}
