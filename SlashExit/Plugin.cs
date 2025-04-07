using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SlashExit.Windows;
using System.Diagnostics;

namespace SlashExit;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IChatGui Chat { get; private set; } = null!; // Add this line

    private const string CommandName = "/exit";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SlashExit");
    private ConfigWindow ConfigWindow { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // you might normally want to embed resources and load them from the manifest stream
        var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");

        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Exit Game"
        });

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // Check if the user is idle
        if (ClientState.IsClientIdle())
        {
            //Kill ACT if enabled
            if (Configuration.KillACT)
            {
                var processes = Process.GetProcessesByName("Advanced Combat Tracker");
                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                    {
                        process.Kill();
                    }
                }
            }

            //Kill Triggevent if enabled
            if (Configuration.KillTriggevent)
            {
                var processes = Process.GetProcessesByName("javaw");
                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle == "Triggevent")
                        {
                            process.Kill();
                        }
                    }
                }
            }

            //Kill XIV
            Process.GetCurrentProcess().Kill();
        }
        else
        {
            Chat.Print("[SlashExit] You must be idle to execute this command.");
        }
    }

    public void ToggleConfigUI() => ConfigWindow.Toggle();
}
