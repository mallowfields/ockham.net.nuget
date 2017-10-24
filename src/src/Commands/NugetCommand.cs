using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;

namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Wrapper class for calling nuget.exe
    /// </summary>
    public abstract class NugetCommand
    {
        private const string NugetExeUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";
        private static readonly Version NugetMinVersion = new Version(4, 3, 0);

        private static string _nugetPath;

        static NugetCommand()
        {
            _nugetPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Nuget", "bin", "nuget.exe");
            EnsureNugetExe();
        }


        private static Version GetAssemblyVersion(string assemblyPath)
        {
#if NETSTANDARD1_3
            var fvInfo = FileVersionInfo.GetVersionInfo(assemblyPath);
            Version asmVersion = null;
            if (Version.TryParse(fvInfo.ProductVersion, out asmVersion)) return asmVersion;
            if (Version.TryParse(fvInfo.FileVersion, out asmVersion)) return asmVersion;
            return null;
#else
            return AssemblyName.GetAssemblyName(assemblyPath).Version;
#endif
        }

        private static void EnsureNugetExe()
        {
            bool downloadNuget = true;
            if (File.Exists(_nugetPath))
            {
                downloadNuget = false;
                try
                {
                    Version asmVersion = GetAssemblyVersion(_nugetPath);
                    if (asmVersion < NugetMinVersion) downloadNuget = true;
                }
                catch
                {
                    downloadNuget = true;
                }
            }

            if (downloadNuget)
            {
                string binDir = Path.GetDirectoryName(_nugetPath);
                if (!Directory.Exists(binDir))
                {
                    Directory.CreateDirectory(binDir);
                }

                using (var wc = new WebClient())
                {
                    wc.DownloadFile(NugetExeUrl, _nugetPath);
                }

                if (!File.Exists(_nugetPath))
                {
                    throw new FileNotFoundException("nuget.exe not found at " + binDir);
                }
                else
                {
                    try
                    {
                        Version asmVersion = GetAssemblyVersion(_nugetPath);
                        string versionPath = Path.Combine(Path.GetDirectoryName(_nugetPath), asmVersion.ToString(), "nuget.exe");
                        File.Copy(_nugetPath, versionPath, true);
                        _nugetPath = versionPath;
                    }
                    catch { /* ignore */ }
                }
            }
        }

        /// <summary>
        /// Update the installed nuget.exe executable to the latest version available from nuget.org
        /// </summary>
        public static void UpdateClient()
        {
            if (File.Exists(_nugetPath))
            {
                File.Delete(_nugetPath);
            }
            EnsureNugetExe();
        }

        /// <summary>
        /// Execute the specified nuget command
        /// </summary>
        /// <param name="command">All arguments to be supplied to nuget.exe, not including 'nuget' itself</param>
        /// <returns>The concatenated standard output from executing the command</returns>
        public static string Execute(string command)
        {
            return NugetCommand.Execute(command, null, Commands.Verbosity.Normal);
        }

        /// <summary>
        /// Execute the specified nuget command, with the provided Verbosity setting
        /// </summary>
        /// <param name="command">All arguments to be supplied to nuget.exe, not including 'nuget' itself</param>
        /// <param name="verbosity">Value to provide for the -Verbosity argument</param>
        /// <returns>The concatenated standard output from executing the command</returns>
        public static string Execute(string command, Verbosity verbosity)
        {
            return NugetCommand.Execute(command, null, verbosity);
        }

        /// <summary>
        /// Execute the specified nuget command, using the specified config file path 
        /// </summary>
        /// <param name="command">All arguments to be supplied to nuget.exe, not including 'nuget' itself</param>
        /// <param name="configFilePath">File path for the -ConfigFile argument</param>
        /// <returns>The concatenated standard output from executing the command</returns>
        public static string Execute(string command, string configFilePath)
        {
            return NugetCommand.Execute(command, configFilePath, Commands.Verbosity.Normal);
        }

        /// <summary>
        /// Execute the specified nuget command, using the specified config file path, and with the provided Verbosity setting
        /// </summary>
        /// <param name="command">All arguments to be supplied to nuget.exe, not including 'nuget' itself</param>
        /// <param name="configFilePath">File path for the -ConfigFile argument</param>
        /// <param name="verbosity">Value to provide for the -Verbosity argument</param>
        /// <returns>The concatenated standard output from executing the command</returns>
        public static string Execute(string command, string configFilePath, Verbosity verbosity)
        {
            EnsureNugetExe();
            if (verbosity == Commands.Verbosity.Detailed)
            {
                command += " -Verbosity detailed";
            }
            else if (verbosity == Commands.Verbosity.Quiet)
            {
                command += " -Verbosity quiet";
            }

            if (!string.IsNullOrEmpty(configFilePath))
            {
                command += " -ConfigFile \"" + configFilePath + "\"";
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = _nugetPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.Arguments = command;

                process.Start();
                process.WaitForExit();

                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new ApplicationException(error);
                }

                return process.StandardOutput.ReadToEnd();
            }
        }

        /// <summary>
        /// The ConfigFile to use for executing the current command
        /// </summary>
        public string ConfigFile { get; set; }

        /// <summary>
        /// The Verbosity argument for the current command
        /// </summary>
        public Verbosity? Verbosity { get; set; }

        /// <summary>
        /// Render the full arguments for the current command, not including Verbosity and ConfigFile
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Verb + " " + string.Join(" ", GetArguments());
        }

        /// <summary>
        /// The verb of the current command (list, install, etc.)
        /// </summary>
        public abstract string Verb { get; }

        /// <summary>
        /// Generate the string arguments for the current command, not including Verbosity and ConfigFile
        /// </summary>
        /// <returns></returns>
        protected abstract List<string> GetArguments();

        /// <summary>
        /// Execute the current command
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            string command = ToString();
            return NugetCommand.Execute(command, ConfigFile, Verbosity ?? Commands.Verbosity.Normal);
        }
    }
}
