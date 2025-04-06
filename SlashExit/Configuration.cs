using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SlashExit;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;
    public bool KillACT { get; set; } = true;
    public bool KillTriggevent { get; set; } = true;

    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
