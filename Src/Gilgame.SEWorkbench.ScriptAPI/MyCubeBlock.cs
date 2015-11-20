using System;

using Sandbox.ModAPI.Ingame;
using VRage.ObjectBuilders;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyCubeBlock : MyEntity, IMyCubeBlock
    {
        public SerializableDefinitionId BlockDefinition { get; set; }

        public bool CheckConnectionAllowed { get; set; }

        public IMyCubeGrid CubeGrid { get; set; }

        public String DefinitionDisplayNameText { get; set; }

        public float DisassembleRatio { get; set; }

        public String DisplayNameText { get; set; }

        public bool IsBeingHacked { get; set; }

        public bool IsFunctional { get; set; }

        public bool IsWorking { get; set; }

        public VRageMath.Vector3I Max { get; set; }

        public float Mass { get; set; }

        public VRageMath.Vector3I Min { get; set; }

        public int NumberInGrid { get; set; }

        public VRageMath.MyBlockOrientation Orientation { get; set; }

        public long OwnerId { get; set; }

        public VRageMath.Vector3I Position { get; set; }

        public string GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        public Sandbox.Common.MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        public Sandbox.Common.MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId)
        {
            throw new NotImplementedException();
        }

        public void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }
}
