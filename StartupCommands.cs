using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("StartupCommands", "Agamemnon", "0.2.0")]
    [Description("Runs a set of commands at server startup")]
    internal class StartupCommands : RustPlugin
    {
        #region Initialization

        private void OnServerInitialized()
        {
            Puts("Running startup commands:");
            int count = 0;

            foreach (StartupCommand startupCommand in config.startupCommands)
            {
                if (startupCommand.Enabled)
                {
                    string parameters = "";

                    foreach (string parameter in startupCommand.Parameters)
                    {
                        parameters = parameters + " \"" + parameter + "\"";
                    }

                    rust.RunServerCommand(startupCommand.Command + parameters);

                    count++;
                }
            }

            if (count == 0)
            {
                Puts("All startup commands are disabled.");
            }
        }

        #endregion Initialization

        #region Configuration

        private Configuration config;

        private class Configuration
        {
            [JsonProperty(PropertyName = "Startup commands", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<StartupCommand> startupCommands = new List<StartupCommand>()
            {
                new StartupCommand("server.hostname", new List<string> { "My Rust Server", }, false),
                new StartupCommand("env.time", new List<string> { "9", }, false),
                new StartupCommand("env.progresstime", new List<string> { "false", }, false)
            };
        }

        class StartupCommand
        {
            public string Command;
            public List<string> Parameters;
            public bool Enabled;

            public StartupCommand(string command, List<string> parameters, bool enabled)
            {
                Command = command;
                Parameters = parameters;
                Enabled = enabled;
            }
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<Configuration>();
                if (config == null)
                    LoadDefaultConfig();
            }
            catch
            {
                PrintError("Detected corrupt configuration file.");
                LoadDefaultConfig();
            }
            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Creating new configuration file.");
            config = new Configuration();
        }

        protected override void SaveConfig() => Config.WriteObject(config);

        #endregion Configuration
    }
}
