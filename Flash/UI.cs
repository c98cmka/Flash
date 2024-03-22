using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;
using Dalamud.Memory;
using Flash.Managers;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Dalamud.Game.ClientState.Conditions;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using System.Drawing;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;

namespace Flash.Windows;

public unsafe class UI : Window, IDisposable
{
    private Configuration Configuration;
    internal float flagX => AgentMap.Instance()->FlagMapMarker.XFloat;
    internal float flagY => AgentMap.Instance()->FlagMapMarker.YFloat;
    internal byte isFlagSet => AgentMap.Instance()->IsFlagMarkerSet;

    internal float uX = 0.0f;
    internal float uY = 0.0f;
    internal float uZ = 0.0f;

    public UI(Plugin plugin) : base("Flash", ImGuiWindowFlags.NoScrollWithMouse)
    {
        var scale = ImGui.GetIO().FontGlobalScale;
        this.Size = new Vector2(560 * scale, 250);
        this.SizeCondition = ImGuiCond.FirstUseEver;

        this.Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {

        ImGui.TextColored(ImGuiColors.DalamudRed, "野外位移请注意频率及距离，频率较高(小于10s)或距离较长的位移可能会被强制90002");
        ImGui.TextColored(ImGuiColors.DalamudRed, "使用时请步行或乘坐坐骑并停留在地面");
        ImGui.TextColored(ImGuiColors.DalamudRed, "副本内及无人岛内可无限制位移");
        //if (ImGui.Button("test"))
        //{
        //    var pos = Service.ClientState.LocalPlayer.Position;
        //    Service.Log.Error(flagX.ToString());
        //    Service.Log.Error(flagY.ToString());
        //    Service.Log.Error(isFlagSet.ToString());
        //    Service.Log.Error("----------------------");
        //    Service.Log.Error(pos.X.ToString());
        //    Service.Log.Error(pos.Y.ToString());
        //    Service.Log.Error(pos.Z.ToString());
        //}
        ImGui.BeginDisabled(IsBusy());
        if (ImGui.Button("移动至<flag>(不会改变高度)"))
        {
            if (!IsBusy() && Service.ClientState.LocalPlayer != null && isFlagSet == 1)
            {
                var pos = Service.ClientState.LocalPlayer.Position;
                var address = Service.ClientState.LocalPlayer.Address;
                MemoryHelper.Write(address + 176, flagX);
                //MemoryHelper.Write(address + 180, pos.Y);
                MemoryHelper.Write(address + 184, flagY);
            }
        }

        if (ImGui.Button("上升10米"))
        {
            if (!IsBusy() && Service.ClientState.LocalPlayer != null)
            {
                var pos = Service.ClientState.LocalPlayer.Position;
                var address = Service.ClientState.LocalPlayer.Address;
                MemoryHelper.Write(address + 180, pos.Y + 10);
            }
        }
        ImGui.SameLine();
        if (ImGui.Button("下降10米"))
        {
            if (!IsBusy() && Service.ClientState.LocalPlayer != null)
            {
                var pos = Service.ClientState.LocalPlayer.Position;
                var address = Service.ClientState.LocalPlayer.Address;
                MemoryHelper.Write(address + 180, pos.Y - 10);
            }
        }
        if (ImGui.Button("上升50米"))
        {
            if (!IsBusy() && Service.ClientState.LocalPlayer != null)
            {
                var pos = Service.ClientState.LocalPlayer.Position;
                var address = Service.ClientState.LocalPlayer.Address;
                MemoryHelper.Write(address + 180, pos.Y + 50);
            }
        }
        ImGui.SameLine();
        if (ImGui.Button("下降50米"))
        {
            if (!IsBusy() && Service.ClientState.LocalPlayer != null)
            {
                var pos = Service.ClientState.LocalPlayer.Position;
                var address = Service.ClientState.LocalPlayer.Address;
                MemoryHelper.Write(address + 180, pos.Y - 50);
            }
        }
        ImGui.EndDisabled();
    }

    private static bool IsBusy()
    {
        return Service.Condition[ConditionFlag.Jumping] ||
               Service.Condition[ConditionFlag.Jumping61] ||
               Service.Condition[ConditionFlag.OccupiedInQuestEvent] ||
               Service.Condition[ConditionFlag.InFlight];
    }

}
