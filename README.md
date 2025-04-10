# SlashExit
This is a simple plugin for Dalamud/FFXIV that allows you to exit the game using a slash command. It also provides options to kill ACT and Triggevent when exiting.

> Please be aware: This immediately terminates the process, there is the potential for loss of data if you don't allow other plugin or game settings to properly save before issuing `/exit`

### Usage:
| Command | Description |
| ------- | ----------- |
| /exit | Exit the game (and optionally ACT, Triggevent) |
| /exitACT | Toggle killing ACT |
| /exitTrigg | Toggle killing Triggevent |

By default, the plugin will kill ACT and Triggevent when exiting. You can toggle this behavior using the `/exitACT` and `/exitTrigg` commands.

### TODO:
- Implement a proper safety net to ensure all settings (game/plugin) are saved prior to termination
