﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Songhay.Models
{
    /// <summary>
    /// REST API Metadata
    /// </summary>
    public class RestApiMetadata
    {
        /// <summary>
        /// Gets or sets the API base.
        /// </summary>
        /// <value>
        /// The API base.
        /// </value>
        public Uri ApiBase { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the URI templates.
        /// </summary>
        /// <value>
        /// The URI templates.
        /// </value>
        public Dictionary<string, string> UriTemplates { get; set; }

        /// <summary>
        /// Converts this instance into a <see cref="string"/> representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (this.ApiBase != null) sb.AppendFormat("ApiBase: {0}", this.ApiBase);
            if (!string.IsNullOrEmpty(this.ApiKey)) sb.AppendFormat(" ApiKey: {0}", this.ApiKey);

            return (sb.Length > 0) ? sb.ToString().Trim() : base.ToString();
        }
    }
}