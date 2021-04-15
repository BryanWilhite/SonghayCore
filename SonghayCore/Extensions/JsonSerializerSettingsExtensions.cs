using Newtonsoft.Json;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="JsonSerializerSettings"/>
    /// </summary>
    public static class JsonSerializerSettingsExtensions
    {
        /// <summary>
        /// Returns <see cref="JsonSerializerSettings"/>
        /// with conventional settings.
        /// </summary>
        /// <param name="settings">the <see cref="JsonSerializerSettings"/></param>
        /// <returns>
        /// Returns <see cref="Formatting.Indented"/>,
        /// <see cref="MissingMemberHandling.Ignore"/>
        /// and <see cref="NullValueHandling.Ignore"/>.
        /// </returns>
        public static JsonSerializerSettings WithConventionalSettings(this JsonSerializerSettings settings)
        {
            return settings
                .WithFormatting(Formatting.Indented)
                .WithMissingMemberHandling(MissingMemberHandling.Ignore)
                .WithNullValueHandling(NullValueHandling.Ignore)
                ;
        }

        /// <summary>
        /// Returns <see cref="JsonSerializerSettings"/>
        /// with <see cref="JsonSerializerSettings.Formatting"/> set.
        /// </summary>
        /// <param name="settings">the <see cref="JsonSerializerSettings"/></param>
        /// <param name="setting">the <see cref="Formatting"/> setting</param>
        /// <returns></returns>
        public static JsonSerializerSettings WithFormatting(this JsonSerializerSettings settings, Formatting setting)
        {
            if (settings == null) return null;

            settings.Formatting = setting;
            return settings;
        }

        /// <summary>
        /// Returns <see cref="JsonSerializerSettings"/>
        /// with <see cref="JsonSerializerSettings.MissingMemberHandling"/> set.
        /// </summary>
        /// <param name="settings">the <see cref="JsonSerializerSettings"/></param>
        /// <param name="setting">the <see cref="MissingMemberHandling"/> setting</param>
        /// <returns></returns>
        public static JsonSerializerSettings WithMissingMemberHandling(this JsonSerializerSettings settings, MissingMemberHandling setting)
        {
            if (settings == null) return null;

            settings.MissingMemberHandling = setting;
            return settings;
        }

        /// <summary>
        /// Returns <see cref="JsonSerializerSettings"/>
        /// with <see cref="JsonSerializerSettings.NullValueHandling"/> set.
        /// </summary>
        /// <param name="settings">the <see cref="JsonSerializerSettings"/></param>
        /// <param name="setting">the <see cref="NullValueHandling"/> setting</param>
        /// <returns></returns>
        public static JsonSerializerSettings WithNullValueHandling(this JsonSerializerSettings settings, NullValueHandling setting)
        {
            if (settings == null) return null;

            settings.NullValueHandling = setting;
            return settings;
        }
    }
}