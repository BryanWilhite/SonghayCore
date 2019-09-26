// <copyright file="FrameworkAssemblyInfo.cs" company="Songhay System">
//     Copyright 2008, Bryan D. Wilhite, Songhay System. All rights reserved.
// </copyright>
namespace Songhay.Models
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Defines Assembly information.
    /// </summary>
    /// <remarks>
    /// This definition was developed
    /// for About… dialogs in Windows Forms.
    /// </remarks>
    public class FrameworkAssemblyInfo : IFrameworkAssemblyInfo
    {
        /// <summary>
        /// Constructor of this class.
        /// </summary>
        /// <param name="targetAssembly">The target <see cref="System.Reflection.Assembly"/></param>
        public FrameworkAssemblyInfo(Assembly targetAssembly)
        {
            this._dll = targetAssembly;
        }

        /// <summary>
        /// Gets title of assembly.
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = this._dll.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (!string.IsNullOrWhiteSpace(titleAttribute.Title))
                    {
                        return titleAttribute.Title;
                    }
                }

                return System.IO.Path.GetFileNameWithoutExtension(this._dll.CodeBase);
            }
        }

        /// <summary>
        /// Gets Assembly version information.
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                AssemblyName name = this._dll.GetName();
                return name.Version.ToString();
            }
        }

        /// <summary>
        /// Gets detailed Assembly version information.
        /// </summary>
        public string AssemblyVersionDetail
        {
            get
            {
                AssemblyName dllName = this._dll.GetName();
                return string.Format(CultureInfo.CurrentCulture, "{0:D}.{1:D2}", dllName.Version.Major, dllName.Version.Minor);
            }
        }

        /// <summary>
        /// Gets Assembly description information.
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                object[] attributes = this._dll.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Gets Assembly product information.
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                object[] attributes = this._dll.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Gets Assembly copyright information.
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = this._dll.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets Assembly company information.
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = this._dll.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary>
        /// Returns format: <c>[AssemblyCompany], [AssemblyTitle] Version: [AssemblyVersion], [AssemblyVersionDetail]</c>.
        /// </summary>
        public override string ToString()
        {
            string s = string.Format("{0}, {1} Version: {2}, {3}",
                this.AssemblyCompany,
                this.AssemblyTitle,
                this.AssemblyVersion,
                this.AssemblyVersionDetail);
            return s;
        }

        Assembly _dll;
    }
}
