using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using Microsoft.Extensions.Logging;

namespace Stafflist;

public class Stafflist : BasePlugin, IPluginConfig<StafflistConfig>
{
    public override string ModuleName => "Stafflist";
    public override string ModuleDescription => "Stafflist for CS2";
    public override string ModuleAuthor => "verneri";
    public override string ModuleVersion => "1.0";

    public StafflistConfig Config { get; set; } = new();
    private readonly HashSet<string> hiddenPlayers = new();

    public void OnConfigParsed(StafflistConfig config)
	{
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        Logger.LogInformation($"[{ModuleName}] Loaded (version {ModuleVersion})");
        AddCommand($"{Config.StaffCommand}", "See staff online", StaffCommand);
        AddCommand($"{Config.HideCommand}", "Hide from staff list", HidemeCommand);
    }

    public void StaffCommand(CCSPlayerController? user, CommandInfo command)
    {
        var serverstaff = 1;
        var staffList = new List<string>();

        foreach (var player in Utilities.GetPlayers().Where(player => !AdminManager.PlayerHasPermissions(player, Config.Immunity) && AdminManager.PlayerHasPermissions(player, Config.ShowOnList) && !hiddenPlayers.Contains(player.PlayerName)))
        {
            staffList.Add(player.PlayerName);
            serverstaff++;
        }

        if (staffList.Count > 0)
        {
            user?.PrintToChat($"{Localizer["stafflist.title"]}");
            var staffNames = string.Join(", ", staffList);
            user?.PrintToChat($"{Localizer["stafflist.online", staffNames]}");

        }else
        {
            user?.PrintToChat($"{Localizer["stafflist.title"]}");
            user?.PrintToChat($"{Localizer["stafflist.offline"]}");
        }
    }

    public void HidemeCommand(CCSPlayerController? user, CommandInfo command)
    {
        if (!AdminManager.PlayerHasPermissions(user, Config.HideCommandFlag)) {

            user.PrintToChat($"{Localizer["stafflist.nopermission", Config.HideCommandFlag]}");
            return; 
        }

        if (user != null)
        {
            if (hiddenPlayers.Contains(user.PlayerName))
            {
                hiddenPlayers.Remove(user.PlayerName);
                user.PrintToChat($"{Localizer["stafflist.adminvisible"]}");
            }
            else
            {
                hiddenPlayers.Add(user.PlayerName);
                user.PrintToChat($"{Localizer["stafflist.adminhidden"]}");
            }
        }
    }
}