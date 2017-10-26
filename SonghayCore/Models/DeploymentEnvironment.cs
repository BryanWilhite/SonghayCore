namespace Songhay.Models
{
    /// <summary>
    /// Defines the deployment environment
    /// </summary>
    public static class DeploymentEnvironment
    {
        /// <summary>
        /// The configuration key
        /// </summary>
        public const string ConfigurationKey = "environment";

        /// <summary>
        /// The configuration key delimiter
        /// </summary>
        public const string ConfigurationKeyDelimiter = ".";

        /// <summary>
        /// The default trace source name configuration key
        /// </summary>
        public const string DefaultTraceSourceNameConfigurationKey = "defaultTraceSourceName";

        /// <summary>
        /// The development environment name
        /// </summary>
        public const string DevelopmentEnvironmentName = "dev";

        /// <summary>
        /// The staging environment name
        /// </summary>
        public const string StagingEnvironmentName = "qa";

        /// <summary>
        /// The production environment name
        /// </summary>
        public const string ProductionEnvironmentName = "prod";
    }
}
