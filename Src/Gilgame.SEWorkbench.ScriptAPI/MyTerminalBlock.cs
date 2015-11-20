using System;
using System.Collections.Generic;
using System.Text;

using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyTerminalBlock : MyCubeBlock, IMyTerminalBlock
    {
        public string CustomName { get; set; }

        public string CustomNameWithFaction { get; set; }

        public string DetailedInfo { get; set; }

        public string CustomInfo { get; set; }

        public bool ShowOnHUD { get; set; }

        public bool HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccess(long playerId)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        public void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalAction GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        public ITerminalProperty GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            throw new NotImplementedException();
        }
    }
}
