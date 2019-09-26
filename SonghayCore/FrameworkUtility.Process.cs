using System;
using System.Diagnostics;
using System.IO;
#if NET452 || NET462
using System.Security.Permissions;
#endif
using System.Text.RegularExpressions;

// <copyright file="FrameworkUtility.cs" company="Songhay System">
//     Copyright 2008, Bryan D. Wilhite, Songhay System. All rights reserved.
// </copyright>
namespace Songhay
{
    /// <summary>
    /// Static members for framework-level procedures.
    /// </summary>
    public static partial class FrameworkUtility
    {
        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="command">The command.</param>
#if NET452 || NET462
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
#endif
        public static void StartProcess(string command)
        {
            if(string.IsNullOrWhiteSpace(command)) return;

            string file = Environment.ExpandEnvironmentVariables(command);
            string args = string.Empty;

            if(File.Exists(file) || Directory.Exists(file))
            {
                //The entire entry is a file or directory:
                Process.Start(file);
            }
            else
            {
                //Look for file and arg’s:
                MatchCollection matches = Regex.Matches(command, @"""[^""]+""|\s+.+");
                if(matches.Count > 0)
                {
                    if(File.Exists(matches[0].Value))
                    {
                        //First match is file:
                        file = matches[0].Value;
                        args = command.Replace(file, string.Empty).Trim();
                    }
                    else
                    {
                        //Assume all matches are arg's:
                        string[] matchedArgs = new string[matches.Count];
                        for(int i = 0; i < matches.Count; i++)
                        {
                            matchedArgs[i] = matches[i].Value;
                        }
                        args = string.Join(" ", matchedArgs);
                        file = command.Replace(args, string.Empty).Trim();
                    }
                    file = Environment.ExpandEnvironmentVariables(file);
                    args = Environment.ExpandEnvironmentVariables(args);
                    Process.Start(file, args);
                }
                else
                {
                    throw new System.ComponentModel.Win32Exception(-2147467259);
                }
            }
        }

        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="argumentOfExe">The argument of executable.</param>
        /// <param name="pathToExe">The path to executable.</param>
        /// <param name="useExe">if set to <c>true</c> use path to executable.</param>
#if NET452 || NET462
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
#endif
        public static void StartProcess(string argumentOfExe, string pathToExe, bool useExe)
        {
            if(useExe)
                Process.Start(pathToExe, argumentOfExe);
            else
                Process.Start(argumentOfExe);
        }
    }
}
