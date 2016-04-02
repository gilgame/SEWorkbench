using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using VRage.Compiler;

namespace Gilgame.SEWorkbench.Interop
{
    public class Decompiler
    {
        private List<string> _Namespaces;

        public static List<Interop.AssemblyObject> Classes = new List<Interop.AssemblyObject>();

        public Decompiler(List<string> namespaces)
        {
            _Namespaces = namespaces;
        }

        public static void LoadClasses()
        {
            if (!Engine.Initialized)
            {
                throw new Exception("Initialize SpaceEngineers interop first.");
            }

            List<string> namespaces = new List<string>();
            foreach(KeyValuePair<Assembly, List<string>> pair in IlChecker.AllowedNamespacesCommon)
            {
                foreach (string allowed in pair.Value)
                {
                    if (!namespaces.Contains(allowed))
                    {
                        namespaces.Add(allowed);
                    }
                }
            }
            foreach(KeyValuePair<Assembly, List<string>> pair in IlChecker.AllowedNamespacesModAPI)
            {
                foreach (string allowed in pair.Value)
                {
                    if (!namespaces.Contains(allowed))
                    {
                        namespaces.Add(allowed);
                    }
                }
            }
            Interop.Decompiler decompiler = new Interop.Decompiler(namespaces);

            List<string> assemblies = new List<string>()
            {
                "SpaceEngineers.Game.dll",
                "SpaceEngineers.ObjectBuilders.dll",
                "Sandbox.Game.dll",
		        "Sandbox.Common.dll",
		        "Sandbox.Graphics.dll",
		        "VRage.dll",
		        "VRage.Library.dll",
		        "VRage.Math.dll",
		        "VRage.Game.dll",
		        "System.Xml.dll",
		        "System.Core.dll",
		        "System.dll",
            };
            List<Interop.AssemblyObject> result = decompiler.Read(assemblies);

            Classes.AddRange(result);
        }

        public List<AssemblyObject> Read(List<string> assemblies)
        {
            List<AssemblyObject> result = new List<AssemblyObject>();
            foreach (string assembly in assemblies)
            {
                string directory = Directory.GetCurrentDirectory();
                if (assembly.StartsWith("System"))
                {
                    // TODO: This needs more love before we include .Net modules
                    //directory = RuntimeEnvironment.GetRuntimeDirectory();
                    continue;
                }

                Mono.Cecil.AssemblyDefinition definition = Mono.Cecil.AssemblyDefinition.ReadAssembly(System.IO.Path.Combine(directory, assembly));
                foreach (Mono.Cecil.ModuleDefinition module in definition.Modules)
                {
                    foreach (Mono.Cecil.TypeDefinition type in module.Types)
                    {
                        string name = type.Name;

                        string ns = type.Namespace;

                        bool found = false;
                        foreach (string allowed in _Namespaces)
                        {
                            if (ns.StartsWith(allowed))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            continue;
                        }

                        AssemblyObject working = result.FirstOrDefault(a => a.Namespace == ns);
                        if (working == null)
                        {
                            working = new AssemblyObject() { Name = ns, Namespace = ns };
                            result.Add(working);
                        }

                        AssemblyObject o = new AssemblyObject() { Name = name, Namespace = ns };
                        if (type.HasFields)
                        {
                            foreach (Mono.Cecil.FieldDefinition f in type.Fields)
                            {
                                if (f.IsPublic)
                                {
                                    string fname = f.Name;
                                    o.Fields.Add(new AssemblyObject() { Name = fname });
                                }
                            }
                        }
                        if (type.HasProperties)
                        {
                            foreach (Mono.Cecil.PropertyDefinition p in type.Properties)
                            {
                                string pname = p.Name;
                                o.Properties.Add(new AssemblyObject() { Name = pname });
                            }
                        }
                        if (type.HasMethods)
                        {
                            foreach (Mono.Cecil.MethodDefinition m in type.Methods)
                            {
                                if (m.IsPublic)
                                {
                                    string mname = m.Name;
                                    if (!mname.StartsWith("get_") || !mname.StartsWith("set_"))
                                    {
                                        continue;
                                    }

                                    AssemblyObject mo = new AssemblyObject() { Name = mname };
                                    if (m.HasParameters)
                                    {
                                        foreach (Mono.Cecil.ParameterDefinition r in m.Parameters)
                                        {
                                            string rname = r.Name;
                                            mo.Methods.Add(new AssemblyObject() { Name = rname });
                                        }
                                    }
                                    o.Methods.Add(mo);
                                }
                            }
                        }

                        working.Children.Add(o);
                    }
                }
            }

            return result;
        }
    }
}
