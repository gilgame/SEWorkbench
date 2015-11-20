using System;

using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyAirVent : MyFunctionalBlock, IMyAirVent
    {
        public bool CanPressurize { get; set; }

        public bool IsDepressurizing { get; set; }

        public bool IsPressurized()
        {
            throw new NotImplementedException();
        }

        public float GetOxygenLevel()
        {
            throw new NotImplementedException();
        }
    }
}
