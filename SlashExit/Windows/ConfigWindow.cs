using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SlashExit.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("SlashExit Configuration Window###SEC_ID")
    {
        Flags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar;

        Size = new Vector2(232, 90);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
    }

    public void Dispose()
    { }

    public override void PreDraw()
    {
    }

    public override void Draw()
    {
        var kACT = Configuration.KillACT;
        if (ImGui.Checkbox("Advanced Combat Tracker", ref kACT))
        {
            Configuration.KillACT = kACT;
            Configuration.Save();
        }

        var kTriggevent = Configuration.KillTriggevent;
        if (ImGui.Checkbox("Triggevent", ref kTriggevent))
        {
            Configuration.KillTriggevent = kTriggevent;
            Configuration.Save();
        }
    }
}
