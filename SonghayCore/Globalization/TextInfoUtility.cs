using Songhay.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Songhay.Globalization
{
    /// <summary>
    /// Helper members for <see cref="System.Globalization.TextInfo" />.
    /// </summary>
    public static partial class TextInfoUtility
    {
        /// <summary>
        /// Wraps <see cref="System.Globalization.TextInfo.ToTitleCase"/>
        /// to add support for articles, conjunctions and prepositions.
        /// </summary>
        /// <param name="input">The input.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "This member is not making a security decision based on the result.")]
        public static string ToTitleCase(string input)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var textInfo = culture.TextInfo;
            input = textInfo.ToTitleCase(input);

            var firstWord = input.Split(' ').First();
            var wordsAfterFirstWord = string.Join(" ",
                input.Split(' ').Skip(1).ToArray());

            EnglishWordsNotCapitalized
                .Where(word => word.Contains(' '))
                .ForEachInEnumerable(word =>
                {
                    wordsAfterFirstWord = wordsAfterFirstWord
                        .Replace(textInfo.ToTitleCase(word), word);
                });

            input = firstWord + " " + wordsAfterFirstWord;

            var words = input.Split(' ')
                .Skip(1)
                .Select(word =>
                {
                    return EnglishWordsNotCapitalized
                        .Contains(word.ToLowerInvariant()) ?
                            word.ToLowerInvariant() : word;
                });
            input = firstWord + " " +
                string.Join(" ", words.ToArray());

            return input;
        }

        /// <summary>
        /// “A virtually complete list of English words that are are NOT capitalized in titles.”
        /// [http://www.cumbrowski.com/CarstenC/articles/20070623_Title_Capitalization_in_the_English_Language.asp]
        /// </summary>
        public static ReadOnlyCollection<string> EnglishWordsNotCapitalized
        {
            get
            {
                return new ReadOnlyCollection<string>(new List<string>
                {
                    //Articles:
                    "a", "an", "the",

                    //Conjunctions
                    "and", "but", "or", "so", "after",
                    "before", "when", "while", "since",
                    "until", "although", "even if",
                    "because", "both", "not only", "but also",

                    //Prepositions
                    "aboard", "about", "above", "absent", "across",
                    "after", "against", "along", "alongside", "amid",
                    "amidst", "among", "amongst", "around", "as", "aslant",
                    "astride", "at", "atop", "barring", "before", "behind",
                    "below", "beneath", "beside", "besides", "between",
                    "beyond", "but", "by", "despite", "down", "during", "except",
                    "failing", "following", "for", "from", "in", "inside", "into",
                    "like", "merry", "mid", "minus", "near", "next", "notwithstanding",
                    "of", "off", "on", "onto", "opposite", "outside", "over", "past", "plus",
                    "regarding", "round", "save", "since", "than", "through", "throughout",
                    "till", "times", "to", "toward", "towards", "under", "underneath", "unlike",
                    "until", "up", "upon", "via", "with", "within", "without",

                    //Prepositions; Two words:
                    "according to", "ahead of", "as to", "aside from",
                    "because of", "close to", "due to", "far from", "in to",
                    "inside of", "instead of", "near to", "next to", "on to", "out of",
                    "outside of", "owing to", "prior to", "subsequent to",

                    //Prepositions; Three words:
                    "as far as", "as well as", "by means of", "in accordance with",
                    "in addition to", "in front of", "in place of", "in spite of",
                    "on account of", "on behalf of", "on top of", "with regard to", "in case of",

                    //Prepositions; Archaic or infrequently used:
                    "anti", "betwixt", "circa", "cum", "in lieu of",
                    "per", "qua", "sans", "unto", "versus", "vis-a-vis",

                    //Prepositions; Postpositions:
                    "ago", "apart", "aside", "away", "hence",
                    "notwithstanding", "on", "through", "withal"
                });
            }
        }
    }
}
