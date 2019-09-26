using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Songhay
{
    /// <summary>
    /// Static members for Open Packaging Conventions.
    /// </summary>
    /// <remarks>
    ///     See http://blogs.msdn.com/b/opc/.
    /// </remarks>
    public static class OpcUtility
    {
        /// <summary>
        /// Gets the Pack URI
        /// from the specified <see cref="System.Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="strategy">The strategy.</param>
        /// <remarks>
        ///     The Pack URI we have is of the form:
        /// 
        ///     <c>pack://application:,,,/{ReferencedAssembly};component/Subfolder/{TypeName}.{Extension}</c>
        /// 
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "OPC Pack URIs are not fully supported by System.Uri.")]
        public static string GetPackUriFromType(Type type, OpcReferencedTypeStrategy strategy)
        {
            return OpcUtility.GetPackUriFromType(type, strategy, "xaml");
        }

        /// <summary>
        /// Gets the Pack URI
        /// from the specified <see cref="System.Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="strategy">The strategy.</param>
        /// <param name="extension">The extension (<c>xaml</c> by default).</param>
        /// <remarks>
        ///     The Pack URI we have is of the form:
        /// 
        ///     <c>pack://application:,,,/{ReferencedAssembly};component/Subfolder/{TypeName}.{Extension}</c>
        /// 
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "OPC Pack URIs are not fully supported by System.Uri.")]
        public static string GetPackUriFromType(Type type, OpcReferencedTypeStrategy strategy, string extension)
        {
            if(type == null) throw new ArgumentNullException("type", "The Type is null.");

            string referencedAssembly = GetReferencedAssembly(type, strategy);

            return (new StringBuilder())
                .AppendFormat("pack://application:,,,/{0};component/Subfolder/{1}.{2}",
                    referencedAssembly, type.Name, extension).ToString();
        }

        /// <summary>
        /// Gets the referenced assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="strategy">The strategy.</param>
        public static string GetReferencedAssembly(Type type, OpcReferencedTypeStrategy strategy)
        {
            if(type == null) return null;

            switch(strategy)
            {
                case OpcReferencedTypeStrategy.FromAssemblyFileName:
                    return Path.GetFileName(type.Assembly.Location)
                        .Replace(".dll", string.Empty);

                case OpcReferencedTypeStrategy.FromTypeFullName:
                    return type.FullName
                        .Replace("." + type.Name, string.Empty);
            }

            throw new ArgumentException("The ReferencedTypeStrategy is not here.");
        }

        /// <summary>
        /// Gets the relative pack URI.
        /// </summary>
        /// <param name="absoluteUri">The absolute URI.</param>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "OPC Pack URIs are not fully supported by System.Uri.")]
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
            Justification = "OpcUtility.PackUriIsValid does the null check.")]
        public static Uri GetRelativePackUri(string absoluteUri)
        {
            if(!OpcUtility.IsPackUriValid(absoluteUri))
                throw new ArgumentException("The URI string is not valid.", "absoluteUri");

            string uri = absoluteUri
                .Replace("pack://application:,,,", string.Empty);

            return new Uri(uri, UriKind.Relative);
        }

        /// <summary>
        /// Returns <c>true</c> when the pack URI is valid.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <remarks>
        /// For more detail, see:
        ///     “Pack URIs in WPF” (http://msdn.microsoft.com/en-us/library/aa970069.aspx)
        ///     “The "pack" URI Scheme” (http://tools.ietf.org/html/draft-shur-pack-uri-scheme-05)
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "OPC Pack URIs are not fully supported by System.Uri.")]
        public static bool IsPackUriValid(string uri)
        {
            bool test = false;
            if (string.IsNullOrWhiteSpace(uri)) return test;

            string Authority = ":///";
            string AuthorityPacked = ":,,,";

            string uriTest = uri.Replace(AuthorityPacked, Authority);

            test = Uri.IsWellFormedUriString(uriTest, UriKind.Absolute);

            return test;
        }
    }
}
