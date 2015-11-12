using System;
using System.Collections.Generic;
using System.Reflection;

using Sandbox.Game.Gui;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Compiler;

namespace Gilgame.SEWorkbench.Interop
{
    public class InGameScript
    {
        private Assembly _Assembly;
        private IMyGridProgram _Instance;

        private List<String> _CompileErrors = new List<string>();
        public List<String> CompileErrors
        {
            get
            {
                return _CompileErrors;
            }
        }

        public string LastError { get; private set; }

        public InGameScript(string program)
        {
            Assembly temp = null;

            MyGuiScreenEditor.CompileProgram(program, _CompileErrors, ref temp);

            if (temp != null)
            {
                try
                {
                    _Assembly = IlInjector.InjectCodeToAssembly("IngameScript_safe", temp, typeof(IlInjector).GetMethod("CountInstructions", BindingFlags.Public | BindingFlags.Static), typeof(IlInjector).GetMethod("CountMethodCalls", BindingFlags.Public | BindingFlags.Static));

                    var type = _Assembly.GetType("Program");
                    if (type != null)
                    {
                        IlInjector.RestartCountingInstructions(50000);
                        IlInjector.RestartCountingMethods(10000);

                        try
                        {
                            _Instance = Activator.CreateInstance(type) as IMyGridProgram;
                            if (_Instance != null)
                            {
                                // success?
                            }
                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException != null)
                            {
                                string response = ex.InnerException.Message;
                                if (LastError != response)
                                {
                                    LastError = response;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string response = ex.Message;
                    if (LastError != response)
                    {
                        LastError = response;
                    }
                }
            }

            // InGameScript
        }
    }
}
