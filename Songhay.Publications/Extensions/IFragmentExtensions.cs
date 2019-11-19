using CloneExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Songhay.Publications.Models;
using Songhay.Models;
using System;
using System.Text;

namespace Songhay.Publications.Extensions
{

    /// <summary>
    /// Extensions of <see cref="IFragment"/>
    /// </summary>
    public static class IFragmentExtensions
    {
        /// <summary>
        /// Clones the instance of <see cref="IFragment"/>.
        /// </summary>
        /// <param name="data">The document.</param>
        /// <returns><see cref="Fragment"/></returns>
        public static Fragment Clone(this IFragment data)
        {
            return data?.GetClone(CloneInitializers.GenericWeb) as Fragment;
        }

        /// <summary>
        /// Gets the content of the fragment.
        /// </summary>
        /// <param name="data">The fragment.</param>
        public static string GetFragmentContent(this IFragment data)
        {
            if (data == null) return null;
            return data.ItemChar ?? data.ItemText;
        }

        /// <summary>
        /// Sets the defaults.
        /// </summary>
        /// <param name="data">The fragment.</param>
        public static void SetDefaults(this IFragment data)
        {
            if (data == null) return;

            data.CreateDate = DateTime.Now;
            data.IsActive = true;
            data.ModificationDate = DateTime.Now;
        }

        /// <summary>
        /// Converts the <see cref="IFragment"/> into a display text.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string ToDisplayText(this IFragment data)
        {
            if (data == null) return null;

            var builder = new StringBuilder($"{nameof(data.FragmentId)}: {data?.FragmentId}");
            builder.Append($", {nameof(data.FragmentName)}: {data?.FragmentName}");
            builder.Append($", {nameof(data.IsActive)}: {data?.IsActive}");
            builder.Append($", {nameof(data.DocumentId)}: {data?.DocumentId}");

            if (!string.IsNullOrEmpty(data.FragmentDisplayName)) builder.Append($", {nameof(data.FragmentDisplayName)}: {data?.FragmentDisplayName}");
            if (!string.IsNullOrEmpty(data.ItemChar)) builder.Append($", {nameof(data.ItemChar)}: {data?.ItemChar.Truncate(32)}");
            if (!string.IsNullOrEmpty(data.ItemText)) builder.Append($", {nameof(data.ItemText)}: {data?.ItemText.Truncate(32)}");

            builder.Append($", {nameof(data.PrevFragmentId)}: {data?.PrevFragmentId}");
            builder.Append($", {nameof(data.NextFragmentId)}: {data?.NextFragmentId}");
            builder.Append($", {nameof(data.IsNext)}: {data?.IsNext}");
            builder.Append($", {nameof(data.IsPrevious)}: {data?.IsPrevious}");
            builder.Append($", {nameof(data.IsWrapper)}: {data?.IsWrapper}");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the <see cref="IFragment" /> to <see cref="JObject" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="useJavaScriptCase">when <c>true</c> use “camel” casing.</param>
        /// <returns></returns>
        public static JObject ToJObject(this IFragment data, bool useJavaScriptCase)
        {
            if (data == null) return null;

            var settings = JsonSerializationUtility
                .GetConventionalResolver<IFragment>(useJavaScriptCase)
                .ToJsonSerializerSettings();

            var jO = JObject.FromObject(data, JsonSerializer.Create(settings));

            return jO;
        }

        /// <summary>
        /// Converts the <see cref="IFragment"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The fragment.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this IFragment data)
        {
            return data.ToMenuDisplayItemModel(group: null, copyFragmentContent: false);
        }

        /// <summary>
        /// Converts the <see cref="IFragment"/> into a menu display item model.
        /// </summary>
        /// <param name="copyFragmentContent">if set to <c>true</c> include <see cref="IFragment"/> content.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this IFragment data, bool copyFragmentContent)
        {
            return data.ToMenuDisplayItemModel(group: null, copyFragmentContent: copyFragmentContent);
        }

        /// <summary>
        /// Converts the <see cref="IFragment"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The fragment.</param>
        /// <param name="group">The group.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this IFragment data, IGroupable group)
        {
            return data.ToMenuDisplayItemModel(group, copyFragmentContent: false);
        }

        /// <summary>
        /// Converts the <see cref="IFragment"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The document.</param>
        /// <param name="group">The group.</param>
        /// <param name="copyFragmentContent">if set to <c>true</c> include <see cref="IFragment"/> content.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this IFragment data, IGroupable group, bool copyFragmentContent)
        {
            if (data == null) return null;

            var dataOut = new MenuDisplayItemModel()
            {
                DisplayText = data.FragmentDisplayName,
                GroupDisplayText = (group == null) ? MenuDisplayItemModelGroups.GenericWebDocument : group.GroupDisplayText,
                GroupId = (group == null) ? MenuDisplayItemModelGroups.GenericWebDocument.ToLowerInvariant() : group.GroupId,
                Id = data.FragmentId,
                ItemName = data.FragmentName
            };
            if (copyFragmentContent) dataOut.Description = data.ItemChar;
            return dataOut;
        }

        /// <summary>
        /// Converts the <see cref="IFragment"/> into a SQLite insert statement.
        /// </summary>
        /// <param name="data">The data.</param>
        public static string ToSQLiteInsertStatement(this IFragment data)
        {
            if (data == null) return null;

            var sqlFormat = @"
INSERT INTO [Fragment]
 (
    [FragmentId]
    ,[ClientId]
    ,[CreateDate]
    ,[DocumentId]
    ,[EndDate]
    ,[FragmentName]
    ,[FragmentDisplayName]
    ,[IsActive]
    ,[IsNext]
    ,[IsPrevious]
    ,[IsWrapper]
    ,[ItemChar]
    ,[ItemText]
    ,[ModificationDate]
    ,[PrevFragmentId]
    ,[NextFragmentId]
    ,[SortOrdinal]
)
     VALUES
    (
    {0}
    ,{1}
    ,{2}
    ,{3}
    ,{4}
    ,{5}
    ,{6}
    ,{7}
    ,{8}
    ,{9}
    ,{10}
    ,{11}
    ,{12}
    ,{13}
    ,{14}
    ,{15}
    ,{16}
    );
";
            var sql = string.Format(sqlFormat,
                data.FragmentId,
                data.ClientId.InDoubleQuotesOrDefault("NULL"),
                FrameworkTypeUtility.ConvertDateTimeToRfc3339DateTime(data.CreateDate.GetValueOrDefault()).InDoubleQuotes(),
                FrameworkTypeUtility.ParseString(data.DocumentId, "NULL"),
                data.EndDate.HasValue ? FrameworkTypeUtility.ConvertDateTimeToRfc3339DateTime(data.EndDate.GetValueOrDefault()).InDoubleQuotes() : "NULL",
                data.FragmentName.InDoubleQuotesOrDefault("NULL"),
                data.FragmentDisplayName.InDoubleQuotesOrDefault("NULL"),
                data.IsActive.GetValueOrDefault() ? 1 : 0,
                data.IsNext.GetValueOrDefault() ? 1 : 0,
                data.IsPrevious.GetValueOrDefault() ? 1 : 0,
                data.IsWrapper.GetValueOrDefault() ? 1 : 0,
                data.ItemChar.InDoubleQuotesOrDefault("NULL"),
                data.ItemText.InDoubleQuotesOrDefault("NULL"),
                data.ModificationDate.HasValue ? FrameworkTypeUtility.ConvertDateTimeToRfc3339DateTime(data.ModificationDate.GetValueOrDefault()).InDoubleQuotes() : "NULL",
                FrameworkTypeUtility.ParseString(data.NextFragmentId, "NULL"),
                FrameworkTypeUtility.ParseString(data.PrevFragmentId, "NULL"),
                FrameworkTypeUtility.ParseString(data.SortOrdinal, "NULL"));

            return sql;
        }

        /// <summary>
        /// Returns <see cref="IFragment"/> with default values.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IFragment WithDefaults(this IFragment data)
        {
            data.SetDefaults();
            return data;
        }

        /// <summary>
        /// Returns <see cref="IFragment"/>
        /// after the specified edit <see cref="Action{IFragment}"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public static IFragment WithEdit(this IFragment data, Action<IFragment> editAction)
        {
            editAction?.Invoke(data);
            return data;
        }

    }
}
