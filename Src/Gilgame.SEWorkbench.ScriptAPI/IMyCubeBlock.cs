using System;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    /// <summary>
    /// Basic cube interface
    /// </summary>
    public interface IMyCubeBlock : IMyEntity
    {
        bool CheckConnectionAllowed { get; }
        IMyCubeGrid CubeGrid { get; }
        String DefinitionDisplayNameText { get; }
        float DisassembleRatio { get; }
        String DisplayNameText { get; }
        string GetOwnerFactionTag();
        bool IsBeingHacked { get; }
        bool IsFunctional { get; }
        bool IsWorking { get; }
        /// <summary>
        /// Block mass
        /// </summary>
        float Mass { get; }
        int NumberInGrid { get; }
        long OwnerId { get; }
        //void ReleaseInventory(Sandbox.Game.MyInventory inventory, bool damageContent = false);
        void UpdateIsWorking();
        void UpdateVisual();
    }
}
