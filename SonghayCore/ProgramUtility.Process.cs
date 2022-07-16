namespace Songhay;

public static partial class ProgramUtility
{
    /// <summary>
    /// Starts the process.
    /// </summary>
    /// <param name="command">The command.</param>
    public static void StartProcess(string? command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;

        string file = Environment.ExpandEnvironmentVariables(command);
        string? args;

        if (File.Exists(file) || Directory.Exists(file))
        {
            //The entire entry is a file or directory:
            Process.Start(file);
        }
        else
        {
            //Look for file and arg’s:
            MatchCollection matches = Regex.Matches(command, @"""[^""]+""|\s+.+");
            if (matches.Count > 0)
            {
                if (File.Exists(matches[0].Value))
                {
                    //First match is file:
                    file = matches[0].Value;
                    args = command.Replace(file, string.Empty).Trim();
                }
                else
                {
                    //Assume all matches are arg's:
                    string[] matchedArgs = new string[matches.Count];
                    for (int i = 0; i < matches.Count; i++)
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
    public static void StartProcess(string argumentOfExe, string pathToExe, bool useExe)
    {
        if (useExe)
            Process.Start(pathToExe, argumentOfExe);
        else
            Process.Start(argumentOfExe);
    }
}
