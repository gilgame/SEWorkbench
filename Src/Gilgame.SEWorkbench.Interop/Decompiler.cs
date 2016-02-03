using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            List<string> namespaces = new List<string>()
            {
                "Sandbox.ModAPI.Ingame",
                "Sandbox.ModAPI.Interfaces",
                "VRageMath",
                "VRage.Game",
                "VRage.Game.Entity"
            };
            Interop.Decompiler decompiler = new Interop.Decompiler(namespaces);

            List<string> assemblies = new List<string>()
            {
                "Sandbox.Common.dll",
                "VRage.Game.dll",
                "VRage.Math.dll"
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

                MyDefaultAssemblyResolver resolver = new MyDefaultAssemblyResolver();
                resolver.AddSearchDirectory(directory);

                Mono.Cecil.ReaderParameters parameters = new Mono.Cecil.ReaderParameters
                {
                    AssemblyResolver = resolver,
                };

                Mono.Cecil.AssemblyDefinition definition = Mono.Cecil.AssemblyDefinition.ReadAssembly(assembly, parameters);
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
