using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("StartupCommands", "Agamemnon", "0.3.0")]
    [Description("Runs a set of commands after server startups and/or server wipes")]
    internal class StartupCommands : RustPlugin
    {
        #region Initialization

        private bool wiped = false;

        private void OnServerInitialized()
        {
            if(wiped == true)
            {
                Puts("Initialising post-wipe commands.");
                int wipeCount = 0;

                foreach (ServerCommand wipeCommand in config.wipeCommands)
                {
                    if (wipeCommand.Enabled)
                    {
                        string parameters = "";

                        foreach (string parameter in wipeCommand.Parameters)
                        {
                            parameters = parameters + " \"" + parameter + "\"";
                        }

                        rust.RunServerCommand(wipeCommand.Command + parameters);

                        wipeCount++;
                    }
                }

                if (wipeCount == 0)
                {
                    Puts("All post-wipe commands are disabled.");
                }
            }

            Puts("Initialising startup commands.");
            int startupCount = 0;

            foreach (ServerCommand startupCommand in config.startupCommands)
            {
                if (startupCommand.Enabled)
                {
                    string parameters = "";

                    foreach (string parameter in startupCommand.Parameters)
                    {
                        parameters = parameters + " \"" + parameter + "\"";
                    }

                    rust.RunServerCommand(startupCommand.Command + parameters);

                    startupCount++;
                }
            }

            if (startupCount == 0)
            {
                Puts("All startup commands are disabled.");
            }
        }

        private void OnNewSave()
        {
            wiped = true;
        }

        #endregion Initialization

        #region Configuration

        private Configuration config;

        private class Configuration
        {
            [JsonProperty(PropertyName = "Startup commands", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<ServerCommand> startupCommands = new List<ServerCommand>()
            {
                new ServerCommand("env.time", new List<string> { "9", }, false),
                new ServerCommand("env.progresstime", new List<string> { "false", }, false)
            };

            [JsonProperty(PropertyName = "Wipe commands", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<ServerCommand> wipeCommands = new List<ServerCommand>()
            {
				new ServerCommand("pasteback", new List<string> { "arena", "stability", "false"}, false),
                new ServerCommand("pasteback", new List<string> { "adminbase", "undestr", "true"}, false)
            };
        }

        class ServerCommand
        {
            public string Command;
            public List<string> Parameters;
            public bool Enabled;

            public ServerCommand(string command, List<string> parameters, bool enabled)
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
