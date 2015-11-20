using System;

using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyProductionBlock : IMyProductionBlock
    {
        public bool IsProducing { get; set; }

        public bool IsQueueEmpty { get; set; }

        public uint NextItemId { get; set; }

        public bool UseConveyorSystem { get; set; }

        public void MoveQueueItemRequest(uint queueItemId, int targetIdx)
        {
            throw new NotImplementedException();
        }

        public void RemoveQueueItemRequest(int itemIdx)
        {
            throw new NotImplementedException();
        }
    }
}
