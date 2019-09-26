namespace Songhay
{
    /// <summary>
    /// Static members for framework-level procedures.
    /// </summary>
    public static partial class FrameworkUtility
    {
        /// <summary>
        /// Gets the console characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns formatted input.</returns>
        public static string GetConsoleCharacters(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) return null;

            #region replace operations:

            input = input.Replace("©", "(C)");
            input = input.Replace("®", "(R)");
            input = input.Replace("™", "TM");
            input = input.Replace("‘", "'");
            input = input.Replace("’", "'");
            input = input.Replace("“", "\"");
            input = input.Replace("”", "\"");
            input = input.Replace("…", "...");

            //Characters 192–223:
            input = input.Replace("À", "A");
            input = input.Replace("Á", "A");
            input = input.Replace("Â", "A");
            input = input.Replace("Ã", "A");
            input = input.Replace("Ä", "A");
            input = input.Replace("Å", "A");
            input = input.Replace("Æ", "AE");
            input = input.Replace("Ç", "C");
            input = input.Replace("È", "E");
            input = input.Replace("É", "E");
            input = input.Replace("Ê", "E");
            input = input.Replace("Ë", "E");
            input = input.Replace("Ì", "I");
            input = input.Replace("Í", "I");
            input = input.Replace("Î", "I");
            input = input.Replace("Ï", "I");
            input = input.Replace("Ð", "D");
            input = input.Replace("Ñ", "N");
            input = input.Replace("Ò", "O");
            input = input.Replace("Ó", "O");
            input = input.Replace("Ô", "O");
            input = input.Replace("Õ", "O");
            input = input.Replace("Ö", "O");
            input = input.Replace("×", "x");
            input = input.Replace("Ø", "O");
            input = input.Replace("Ù", "U");
            input = input.Replace("Ú", "U");
            input = input.Replace("Û", "U");
            input = input.Replace("Ü", "U");
            input = input.Replace("Ý", "Y");

            //Characters 224–255:
            input = input.Replace("à", "a");
            input = input.Replace("á", "a");
            input = input.Replace("â", "a");
            input = input.Replace("ã", "a");
            input = input.Replace("ä", "a");
            input = input.Replace("å", "a");
            input = input.Replace("æ", "ae");
            input = input.Replace("ç", "c");
            input = input.Replace("è", "e");
            input = input.Replace("é", "e");
            input = input.Replace("ê", "e");
            input = input.Replace("ë", "e");
            input = input.Replace("ì", "i");
            input = input.Replace("í", "i");
            input = input.Replace("î", "i");
            input = input.Replace("ï", "i");
            input = input.Replace("ñ", "n");
            input = input.Replace("ò", "o");
            input = input.Replace("ó", "o");
            input = input.Replace("ô", "o");
            input = input.Replace("õ", "o");
            input = input.Replace("ö", "o");
            input = input.Replace("÷", "/");
            input = input.Replace("ø", "o");
            input = input.Replace("ù", "u");
            input = input.Replace("ú", "u");
            input = input.Replace("û", "u");
            input = input.Replace("ü", "u");
            input = input.Replace("ý", "y");
            input = input.Replace("ÿ", "y");

            #endregion

            return input;
        }
    }
}
