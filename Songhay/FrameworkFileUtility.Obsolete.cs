using System;
using System.IO;
using System.Text;

namespace Songhay
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.IO"/>.
    /// </summary>
    public static partial class FrameworkFileUtility
    {
        /// <summary>
        /// Returns the file name
        /// based on the specified path
        /// and <see cref="Path.DirectorySeparatorChar"/>.
        /// </summary>
        /// <param name="pathWithFile">
        /// The specified path.
        /// </param>
        [Obsolete("Since .NET 1.1 use System.IO.Path.GetFileName().")]
        public static string GetFileName(string pathWithFile)
        {
            return GetFileName(pathWithFile, null);
        }

        /// <summary>
        /// Returns the file name
        /// based on the specified path.
        /// </summary>
        /// <param name="pathWithFile">
        /// The specified path.
        /// </param>
        /// <param name="pathDelimiter">
        /// Path delimiter (e.g. \ or /).
        /// </param>
        [Obsolete("Since .NET 1.1 use System.IO.Path.GetFileName().")]
        public static string GetFileName(string pathWithFile, char? pathDelimiter)
        {
            if(string.IsNullOrEmpty(pathWithFile)) return null;

            char delim = (pathDelimiter.HasValue) ? pathDelimiter.Value : Path.DirectorySeparatorChar;
            int pos = pathWithFile.LastIndexOf(delim);
            pos++;
            if (pos > pathWithFile.Length)
                return null;
            else
                return pathWithFile.Substring(pos);
        }

        /// <summary>
        /// Returns the directory root
        /// based on the specified path.
        /// </summary>
        /// <param name="pathWithFile">
        /// The specified path.
        /// </param>
        [Obsolete("Since .NET 1.1 use System.IO.Path.GetDirectoryName().")]
        public static string GetPathRoot(string pathWithFile)
        {
            return GetPathRoot(pathWithFile, null);
        }

        /// <summary>
        /// Returns the directory root
        /// based on the specified path.
        /// </summary>
        /// <param name="pathWithFile">
        /// The specified path.
        /// </param>
        /// <param name="pathDelimiter">
        /// Path delimiter (e.g. \ or /).
        /// </param>
        [Obsolete("Since .NET 1.1 use System.IO.Path.GetDirectoryName().")]
        public static string GetPathRoot(string pathWithFile, char? pathDelimiter)
        {
            if(string.IsNullOrEmpty(pathWithFile)) return null;
            string ret = GetFileName(pathWithFile, pathDelimiter);
            return pathWithFile.Replace(ret, string.Empty);
        }

#if !SILVERLIGHT
        /// <summary>
        /// Gets the UTF-8 encoded string.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        public static string GetEncodedString(string rawValue)
        {
            return FrameworkFileUtility.GetEncodedString(rawValue, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the encoded string.
        /// </summary>
        /// <param name="rawValue">The raw value (<see cref="System.Text.Encoding.ASCII"/> by default).</param>
        /// <param name="encoding">The encoding.</param>
        public static string GetEncodedString(string rawValue, Encoding encoding)
        {
            if(encoding == null)
                throw new ArgumentNullException("encoding",
                    "The expected encoding is not here.");

            byte[] b = Encoding.Convert(Encoding.ASCII, encoding, encoding.GetBytes(rawValue));

            return new string(Encoding.ASCII.GetChars(b));
        }
#endif

        /// <summary>
        /// Joins the path and root.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="root">The root.</param>
        [Obsolete("Since .NET 4.0 use System.IO.Path.Combine().")]
        public static string JoinPathAndRoot(string path, string root)
        {
            return JoinPathAndRoot(path, root, null);
        }

        /// <summary>
        /// Joins the path and root.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="root">The root.</param>
        /// <param name="pathDelimiter">
        /// Path delimiter (e.g. \ or /).
        /// </param>
        [Obsolete("Since .NET 4.0 use System.IO.Path.Combine().")]
        public static string JoinPathAndRoot(string path, string root, char? pathDelimiter)
        {
            if(string.IsNullOrEmpty(path) || string.IsNullOrEmpty(root)) return root;

            char delim = (pathDelimiter.HasValue) ? pathDelimiter.Value : Path.DirectorySeparatorChar;
            path = path.Replace("./", string.Empty);
            string s = string.Concat(root, delim.ToString(), path);
            s = s.Replace(string.Concat(delim.ToString(), delim.ToString()), delim.ToString());
            return s;
        }

        /// <summary>
        /// Writes the specified content to a file.
        /// </summary>
        /// <param name="content">The content to write or overwrite.</param>
        /// <param name="pathWithFile">The path to the file.</param>
        [Obsolete("Since .NET 2.0 use System.IO.File.WriteAllText().")]
        public static void Write(string content, string pathWithFile)
        {
            using(StreamWriter writer = File.CreateText(pathWithFile))
            {
                writer.Write(content);
            }
        }
    }
}
