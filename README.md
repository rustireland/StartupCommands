# StartupCommands
**StartupCommands** is an [Oxide](https://umod.org/) plugin that allows Rust server admins to run a configurable list of commands upon server startup. Each command can be individually enabled or disabled in the configuration. The plugin correctly supports multiple-parameter commands with spaces in the parameters.

## Installation
Download the plugin:
```bash
git clone https://github.com/rustireland/StartupCommands.git
```
Copy it to your Oxide plugins directory:
```bash
cp StartupCommands/StartupCommands.cs oxide/plugins
```
Oxide will compile and load the plugin automatically.

## Permissions
This plugin doesn't make use of the Oxide permissions system.

## Commands
This plugin doesn't provide any console or chat commands.

## Configuration
The settings and options for this plugin can be configured in the **StartupCommands.json** file under the **oxide/config** directory. The use of a JSON editor or validation site such as [jsonlint.com](jsonlint.com) is recommended to avoid formatting issues and syntax errors. 

When run for the first time, the plugin will create a default configuration file with all commands *disabled*.
```json
{
  "Startup commands": [
    {
      "Command": "server.hostname",
      "Parameters": [
        "My Rust Server"
      ],
      "Enabled": false
    },
    {
      "Command": "env.time",
      "Parameters": [
        "9"
      ],
      "Enabled": false
    },
    {
      "Command": "env.progresstime",
      "Parameters": [
        "false"
      ],
      "Enabled": false
    }
  ]
}
```

## Changelog
* 01-May-2020 - 0.2.0 - Initial release
