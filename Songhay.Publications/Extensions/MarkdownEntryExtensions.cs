using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Songhay.Publications.Models;

namespace Songhay.Publications
{
    /// <summary>
    /// Extensions of <see cref="MarkdownEntry" />
    /// </summary>
    public static class MarkdownEntryExtensions
    {
        /// <summary>
        /// Effectively validates <see cref="MarkdownEntry" />
        /// </summary>
        /// <param name="entry">the <see cref="MarkdownEntry" /> entry</param>
        public static MarkdownEntry DoNullCheck(this MarkdownEntry entry)
        {
            if (entry == null)
            {
                throw new NullReferenceException($"The expected {nameof(MarkdownEntry)} is not here.");
            }

            if (entry.FrontMatter == null)
            {
                throw new NullReferenceException($"The expected {nameof(MarkdownEntry.FrontMatter)} is not here.");
            }

            if (string.IsNullOrWhiteSpace(entry.Content))
            {
                throw new NullReferenceException($"The expected {nameof(MarkdownEntry.Content)} is not here.");
            }

            return entry;
        }

        /// <summary>
        /// Converts the specified <see cref="MarkdownEntry" />
        /// into the final edit <see cref="String" />
        /// </summary>
        /// <param name="entry">the <see cref="MarkdownEntry" /> entry</param>
        public static string ToFinalEdit(this MarkdownEntry entry)
        {
            entry.DoNullCheck();

            var finalEdit = string.Concat(
                "---json",
                MarkdownEntry.NewLine,
                entry.FrontMatter.ToString().Trim(),
                MarkdownEntry.NewLine,
                "---",
                MarkdownEntry.NewLine,
                MarkdownEntry.NewLine,
                entry.Content.Trim(),
                MarkdownEntry.NewLine
            );

            return finalEdit;
        }

        /// <summary>
        /// Converts the specified <see cref="FileInfo" />
        /// into <see cref="MarkdownEntry" />
        /// </summary>
        /// <param name="entry">the <see cref="FileInfo" /> entry</param>
        /// <param name="editAction">the edit <see cref="Action{FileInfo, MarkdownEntry}" /></param>
        public static MarkdownEntry ToMarkdownEntry(this FileInfo entry)
        {
            if (entry == null) throw new NullReferenceException($"The expected {nameof(FileInfo)} is not here.");
            if (!File.Exists(entry.FullName)) throw new NullReferenceException($"The expected {nameof(FileInfo)} path is not here.");

            var frontTop = "---json";
            var frontBottom = "---";
            var lines = File.ReadAllLines(entry.FullName);

            if (!lines.Any()) throw new FormatException($"File {entry.Name} is empty.");
            if (lines.First().Trim() != frontTop) throw new FormatException("The expected entry format is not here [front matter top].");
            if (!lines.Contains(frontBottom)) throw new FormatException("The expected entry format is not here [front matter bottom].");

            var json = lines
                .Skip(1)
                .TakeWhile(i => !i.Contains(frontBottom))
                .Aggregate((a, i) => $"{a}{MarkdownEntry.NewLine}{i}");

            var content = lines
                .SkipWhile(i => !i.Equals(frontBottom))
                .Skip(1)
                .Aggregate((a, i) => $"{a}{MarkdownEntry.NewLine}{i}");

            var frontMatter = JObject.Parse(json);

            var mdEntry = new MarkdownEntry
            {
                EntryFileInfo = entry,
                FrontMatter = frontMatter,
                Content = content
            };

            return mdEntry;
        }

        /// <summary>
        /// Sets the modification date of the <see cref="MarkdownEntry" />.
        /// </summary>
        /// <param name="entry">the <see cref="MarkdownEntry" /> entry</param>
        /// <param name="date">the touch <see cref="DateTime" /></param>
        /// <returns></returns>
        public static MarkdownEntry Touch(this MarkdownEntry entry, DateTime date)
        {
            string ConvertLocalToUtc(DateTime local)
            { //TODO: move to SonghayCore
                return local
                    .ToUniversalTime()
                    .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }

            return entry.WithEdit(i => i.FrontMatter["modificationDate"] = ConvertLocalToUtc(date));
        }

        /// <summary>
        /// Returns the <see cref="MarkdownEntry" /> with conventional 11ty frontmatter.
        /// </summary>
        /// <param name="entry">the <see cref="MarkdownEntry" /> entry</param>
        /// <param name="title">the title of th entry</param>
        /// <param name="inceptDate">the incept date of the entry</param>
        /// <param name="path">the path to the entry</param>
        /// <param name="tag">the tag of the entry</param>
        /// <returns></returns>
        public static MarkdownEntry WithNew11tyFrontMatter(this MarkdownEntry entry, string title, DateTime inceptDate, string path, string tag)
        {
            return entry.WithNewFrontMatter(title, inceptDate,
                documentId: 0, fileName: "index.html", path: path, segmentId: 0, tag: tag)
                .WithEdit(i => i.FrontMatter["clientId"] = $"{inceptDate.ToString("yyyy-MM-dd")}-{i.FrontMatter["clientId"]}")
                .WithEdit(i => i.FrontMatter["documentShortName"] = i.FrontMatter["clientId"])
                .WithEdit(i => i.FrontMatter["path"] = $"{i.FrontMatter["path"]}{i.FrontMatter["clientId"]}")
                .WithEdit(i => i.Content = $"## {i.FrontMatter["title"]}{MarkdownEntry.NewLine}");
        }

        /// <summary>
        /// Returns the <see cref="MarkdownEntry" /> with conventional frontmatter.
        /// </summary>
        /// <param name="entry">the <see cref="MarkdownEntry" /> entry</param>
        /// <param name="title">the title of th entry</param>
        /// <param name="inceptDate">the incept date of the entry</param>
        /// <param name="documentId">the DBMS ID of the entry</param>
        /// <param name="fileName">the file name (with extension) of the entry</param>
        /// <param name="path">the path to the entry</param>
        /// <param name="segmentId">the DBMS ID of the GenericWeb Segment</param>
        /// <param name="tag">the tag of the entry</param>
        /// <returns></returns>
        public static MarkdownEntry WithNewFrontMatter(this MarkdownEntry entry, string title, DateTime inceptDate, int documentId, string fileName, string path, int segmentId, string tag)
        {
            if (entry == null)
            {
                throw new NullReferenceException($"The expected {nameof(MarkdownEntry)} is not here.");
            }

            string ConvertLocalToUtc(DateTime local)
            { //TODO: move to SonghayCore
                return local
                    .ToUniversalTime()
                    .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }

            var slug = title.ToBlogSlug();

            var fm = new
            {
                documentId,
                title,
                documentShortName = slug,
                fileName,
                path,
                date = ConvertLocalToUtc(inceptDate),
                modificationDate = ConvertLocalToUtc(inceptDate),
                templateId = 0,
                segmentId,
                isRoot = false,
                isActive = true,
                sortOrdinal = 0,
                clientId = slug,
                tag
            };

            entry.FrontMatter = JObject.FromObject(fm);

            return entry;
        }

        /// <summary>
        /// Converts <see cref="MarkdownEntry.Content" /> to paragraphs
        /// </summary>
        /// <param name="entry">the <see cref="MarkdownEntry" /> entry</param>
        public static string[] ToParagraphs(this MarkdownEntry entry)
        {
            entry.DoNullCheck();
            var paragraphs = entry.Content
                .Trim()
                .Split($"{MarkdownEntry.NewLine}{MarkdownEntry.NewLine}");
            return paragraphs;
        }

        /// <summary>
        /// Edits the <see cref="MarkdownEntry" /> with the specified edit action.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="editAction">the edit <see cref="Action{MarkdownEntry}" /></param>
        public static MarkdownEntry WithEdit(this MarkdownEntry entry, Action<MarkdownEntry> editAction)
        {
            editAction?.Invoke(entry);
            return entry;
        }
    }
}