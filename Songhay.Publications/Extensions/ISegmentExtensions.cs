using CloneExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay.Publications.Models;
using Songhay.Models;
using System;
using System.Text;

namespace Songhay.Publications.Extensions
{

    /// <summary>
    /// Extensions of <see cref="ISegment"/>
    /// </summary>
    public static class ISegmentExtensions
    {
        /// <summary>
        /// Clones the instance of <see cref="ISegment"/>.
        /// </summary>
        /// <param name="data">The document.</param>
        /// <returns><see cref="Segment"/></returns>
        public static Segment Clone(this ISegment data)
        {
            return data?.GetClone(CloneInitializers.GenericWeb) as Segment;
        }

        /// <summary>
        /// Sets the defaults.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void SetDefaults(this ISegment data)
        {
            if (data == null) return;

            data.CreateDate = DateTime.Now;
            data.IsActive = true;
        }

        /// <summary>
        /// Converts the <see cref="ISegment"/> into a display text.
        /// </summary>
        /// <param name="data">The data.</param>
        public static string ToDisplayText(this ISegment data)
        {
            if (data == null) return null;

            var builder = new StringBuilder($"{nameof(data.SegmentId)}: {data?.SegmentId}");
            builder.Append($", {nameof(data.SegmentName)}: {data?.SegmentName}");
            builder.Append($", {nameof(data.IsActive)}: {data?.IsActive}");
            builder.Append($", {nameof(data.ParentSegmentId)}: {data?.ParentSegmentId}");
            builder.Append($", {nameof(data.CreateDate)}: {data?.CreateDate}");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the <see cref="ISegment" /> to <see cref="JObject" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="useJavaScriptCase">when <c>true</c> use “camel” casing.</param>
        /// <returns></returns>
        public static JObject ToJObject(this ISegment data, bool useJavaScriptCase)
        {
            if (data == null) return null;

            var settings = JsonSerializationUtility
                .GetConventionalResolver<ISegment>(useJavaScriptCase)
                .ToJsonSerializerSettings();

            var jO = JObject.FromObject(data, JsonSerializer.Create(settings));
            return jO;
        }

        /// <summary>
        /// Converts the <see cref="ISegment"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The data.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this ISegment data)
        {
            return data.ToMenuDisplayItemModel(group: null);
        }

        /// <summary>
        /// Converts the <see cref="ISegment"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="group">The group.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this ISegment data, IGroupable group)
        {
            if (data == null) return null;

            var dataOut = new MenuDisplayItemModel()
            {
                GroupDisplayText = (group == null) ? MenuDisplayItemModelGroups.GenericWebDocument : group.GroupDisplayText,
                GroupId = (group == null) ? MenuDisplayItemModelGroups.GenericWebDocument.ToLowerInvariant() : group.GroupId,
                Id = data.SegmentId,
                ItemName = data.SegmentName
            };
            return dataOut;
        }

        /// <summary>
        /// Converts the <see cref="ISegment"/> into a SQLite insert statement.
        /// </summary>
        /// <param name="data">The data.</param>
        public static string ToSQLiteInsertStatement(this ISegment data)
        {
            if (data == null) return null;

            var sqlFormat = @"
INSERT INTO [Segment]
    ([SegmentId]
    ,[SegmentName]
    ,[SortOrdinal]
    ,[CreateDate]
    ,[ParentSegmentId]
    ,[ClientId]
    ,[IsActive])
     VALUES
    ({0}
    ,""{1}""
    ,{2}
    ,""{3}""
    ,{4}
    ,""{5}""
    ,{6});
";
            var sql = string.Format(sqlFormat,
                data.SegmentId,
                data.SegmentName,
                FrameworkTypeUtility.ParseString(data.SortOrdinal, "NULL"),
                FrameworkTypeUtility.ConvertDateTimeToRfc3339DateTime(data.CreateDate.GetValueOrDefault()),
                FrameworkTypeUtility.ParseString(data.ParentSegmentId, "NULL"),
                data.ClientId,
                data.IsActive.GetValueOrDefault() ? 1 : 0);

            return sql;
        }

        /// <summary>
        /// Returns <see cref="ISegment"/> with default values.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static ISegment WithDefaults(this ISegment data)
        {
            data.SetDefaults();
            return data;
        }

        /// <summary>
        /// Returns <see cref="ISegment"/>
        /// after the specified edit <see cref="Action{ISegment}"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public static ISegment WithEdit(this ISegment data, Action<ISegment> editAction)
        {
            editAction?.Invoke(data);
            return data;
        }
    }
}
