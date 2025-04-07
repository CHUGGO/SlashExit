using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using System.Diagnostics;

namespace SlashExit;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IChatGui Chat { get; private set; } = null!;

    private const string CommandName = "/exit";
    private const string CommandName2 = "/exitACT";
    private const string CommandName3 = "/exitTrigg";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SlashExit");

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Exit Game"
        });

        CommandManager.AddHandler(CommandName2, new CommandInfo(OnCommandConfigACT)
        {
            HelpMessage = "Toggle also killing ACT"
        });

        CommandManager.AddHandler(CommandName3, new CommandInfo(OnCommandConfigTriggevent)
        {
            HelpMessage = "Toggle also killing Triggevent"
        });
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        CommandManager.RemoveHandler(CommandName);
        CommandManager.RemoveHandler(CommandName2);
        CommandManager.RemoveHandler(CommandName3);
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

    private void OnCommandConfigACT(string command, string args)
    {
        Configuration.KillACT = !Configuration.KillACT;
        Configuration.Save();
        Chat.Print($"[SlashExit] KillACT: {Configuration.KillACT}");
    }

    private void OnCommandConfigTriggevent(string command, string args)
    {
        Configuration.KillTriggevent = !Configuration.KillTriggevent;
        Configuration.Save();
        Chat.Print($"[SlashExit] KillTriggevent: {Configuration.KillTriggevent}");
    }
}
