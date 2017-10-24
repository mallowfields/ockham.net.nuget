using System;
using System.Collections.Generic;
using System.Text;

namespace Ockham.Nuget
{
    public class NugetClient
    {
        private bool _defaultConfig;

        public NugetClient()
        {
            this._defaultConfig = true;
            this.ConfigFile = Path.Combine(Environment.GetEnvironmentVariable("AppData"), "Nuget", "Nuget.config");
            Init();
        }

        public NugetClient(string pConfigFile)
        {
            this._defaultConfig = false;
            this.ConfigFile = pConfigFile;
            Init();
        }

        private void Init()
        {
            this.Sources = new ClientSources(this);
            this.Packages = new ClientPackages(this);
            string lConfigDir = Path.GetDirectoryName(this.ConfigFile);
            if (!Directory.Exists(lConfigDir))
            {
                Directory.CreateDirectory(lConfigDir);
            }
            if (!File.Exists(ConfigFile))
            {

            }
        }

        public string ConfigFile { get; private set; }

        public ClientSources Sources { get; private set; }
        public ClientPackages Packages { get; private set; }

        public string Execute(string pCommand)
        {
            return NugetCommand.Execute(pCommand, this.ConfigFile);
        }

        public string Execute(string pCommand, Verbosity pVerbosity)
        {
            return NugetCommand.Execute(pCommand, this.ConfigFile, pVerbosity);
        }
    }
}
