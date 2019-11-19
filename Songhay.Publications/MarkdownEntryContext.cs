using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Songhay.Publications.Extensions;
using Songhay.Publications.Models;
using System;
using System.IO;
using System.Linq;

namespace Songhay.Publications
{
    /// <summary>
    /// Shares routines for <see cref="MarkdownEntry"/>.
    /// </summary>
    public static class MarkdownEntryContext
    {
        /// <summary>
        /// Generates or overwrites a <see cref="MarkdownEntry"/> file
        /// at the specified entry root.
        /// </summary>
        /// <param name="entryRoot">the conventional directory of <see cref="MarkdownEntry"/> drafts</param>
        /// <param name="title">see <see cref="MarkdownEntryExtensions.WithNew11tyFrontMatter"/></param>
        public static MarkdownEntry GenerateEntryFor11ty(string entryRoot, string title)
        {
            var tag = JObject.FromObject(new { extract = string.Empty }).ToString();
            return GenerateEntryFor11ty(entryRoot, title, DateTime.Now, "./entry/", tag);
        }

        /// <summary>
        /// Generates or overwrites a <see cref="MarkdownEntry"/> file
        /// at the specified entry root.
        /// </summary>
        /// <param name="entryRoot">the conventional directory of <see cref="MarkdownEntry"/> drafts</param>
        /// <param name="title">see <see cref="MarkdownEntryExtensions.WithNew11tyFrontMatter"/></param>
        /// <param name="inceptDate">see <see cref="MarkdownEntryExtensions.WithNew11tyFrontMatter"/></param>
        /// <param name="path">see <see cref="MarkdownEntryExtensions.WithNew11tyFrontMatter"/></param>
        /// <param name="tag">see <see cref="MarkdownEntryExtensions.WithNew11tyFrontMatter"/></param>
        public static MarkdownEntry GenerateEntryFor11ty(string entryRoot, string title, DateTime inceptDate, string path, string tag)
        {
            if (!Directory.Exists(entryRoot))
                throw new DirectoryNotFoundException($"The expected entry root directory, `{entryRoot ?? "[null]"}`, is not here.");

            var entry = new MarkdownEntry()
                    .WithNew11tyFrontMatter(title, inceptDate, path, tag)
                    .WithContentHeader();

            File.WriteAllText($"{entryRoot}/{entry.FrontMatter["clientId"]}.md", entry.ToFinalEdit());

            return entry;
        }

        /// <summary>
        /// Publishes a <see cref="MarkdownEntry"/>
        /// from the specified entry root
        /// to the specified presentation root
        /// for the eleventy pipeline.
        /// </summary>
        /// <param name="entryRoot">the conventional directory of <see cref="MarkdownEntry"/> drafts</param>
        /// <param name="presentationRoot">the presentation target directory for publication</param>
        /// <param name="fileName">the name of the <see cref="MarkdownEntry"/> file in the entry directory</param>
        /// <returns>
        /// Returns the path of the published file.
        /// </returns>
        public static string PublishEntryFor11ty(string entryRoot, string presentationRoot, string fileName)
        {
            return PublishEntryFor11ty(entryRoot, presentationRoot, fileName, DateTime.Now);
        }

        /// <summary>
        /// Publishes a <see cref="MarkdownEntry"/>
        /// from the specified entry root
        /// to the specified presentation root
        /// for the eleventy pipeline.
        /// </summary>
        /// <param name="entryRoot">the conventional directory of <see cref="MarkdownEntry"/> drafts</param>
        /// <param name="presentationRoot">the presentation target directory for publication</param>
        /// <param name="fileName">the name of the <see cref="MarkdownEntry"/> file in the entry directory</param>
        /// <param name="publicationDate">the <see cref="DateTime"/> of publication</param>
        /// <returns>
        /// Returns the path of the published file.
        /// </returns>
        /// <remarks>
        /// When the publication date is one day later or more than the entry incept date
        /// new eleventy <see cref="MarkdownEntry.FrontMatter"/> will be generated
        /// and the presentation file will be renamed accordingly.
        /// </remarks>
        public static string PublishEntryFor11ty(string entryRoot, string presentationRoot, string fileName, DateTime publicationDate)
        {
            if (!Directory.Exists(entryRoot))
                throw new DirectoryNotFoundException($"The expected entry root directory, `{entryRoot ?? "[null]"}`, is not here.");

            if (!Directory.Exists(presentationRoot))
                throw new DirectoryNotFoundException($"The expected presentation root directory, `{presentationRoot ?? "[null]"}`, is not here.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new NullReferenceException("The expected file name is not here.");

            if (!fileName.EndsWith(".md"))
                throw new FormatException("The expected file name format, `*.md`, is not here.");

            var rootInfo = new DirectoryInfo(entryRoot);
            var draftInfo = rootInfo.GetFiles().First(i => i.Name.EqualsInvariant(fileName));
            var draftEntry = draftInfo.ToMarkdownEntry();
            var inceptDate = draftEntry.FrontMatter.GetValue<DateTime>("date");

            if ((publicationDate - inceptDate).Days >= 1)
            {
                var title = draftEntry.FrontMatter.GetValue<string>("title");
                var tag = draftEntry.FrontMatter.GetValue<string>("tag", throwException: false);
                draftEntry.WithNew11tyFrontMatter(title, publicationDate, presentationRoot, tag);
            }

            var combinedPath = FrameworkFileUtility.GetCombinedPath(presentationRoot, $"{draftEntry.FrontMatter.GetValue<string>("clientId")}.md");
            File.WriteAllText(combinedPath, draftEntry.ToFinalEdit());
            draftInfo.Delete();

            return combinedPath;
        }
    }
}
