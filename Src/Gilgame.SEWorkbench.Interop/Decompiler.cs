using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gilgame.SEWorkbench.Interop
{
    public class Decompiler
    {
        private List<string> _Namespaces;

        public Decompiler(List<string> namespaces)
        {
            _Namespaces = namespaces;
        }

        public List<AssemblyObject> Read(List<string> assemblies)
        {
            List<AssemblyObject> result = new List<AssemblyObject>();
            foreach (string assembly in assemblies)
            {
                string directory = Directory.GetCurrentDirectory();

                MyDefaultAssemblyResolver resolver = new MyDefaultAssemblyResolver();
                resolver.AddSearchDirectory(directory);

                ReaderParameters parameters = new ReaderParameters
                {
                    AssemblyResolver = resolver,
                };

                AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(assembly, parameters);
                foreach (ModuleDefinition module in definition.Modules)
                {
                    foreach (TypeDefinition type in module.Types)
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
                            foreach (FieldDefinition f in type.Fields)
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
                            foreach (PropertyDefinition p in type.Properties)
                            {
                                string pname = p.Name;
                                o.Properties.Add(new AssemblyObject() { Name = pname });
                            }
                        }
                        if (type.HasMethods)
                        {
                            foreach (MethodDefinition m in type.Methods)
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
                                        foreach (ParameterDefinition r in m.Parameters)
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

        //public List<AssemblyObject> Read(string assembly)
        //{
        //    string working = Directory.GetCurrentDirectory();

        //    MyDefaultAssemblyResolver resolver = new MyDefaultAssemblyResolver();
        //    resolver.AddSearchDirectory(working);

        //    ReaderParameters parameters = new ReaderParameters
        //    {
        //        AssemblyResolver = resolver,
        //    };

        //    List<AssemblyObject> namespaces = new List<AssemblyObject>();

        //    AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(assembly, parameters);
        //    foreach (ModuleDefinition module in definition.Modules)
        //    {
        //        foreach (TypeDefinition type in module.Types)
        //        {
        //            string name = type.Name;
        //            string ns = type.Namespace;

        //            if (!_Namespaces.Contains(ns))
        //            {
        //                continue;
        //            }

        //            AssemblyObject found = namespaces.First(a => a.Namespace == ns);

        //            AssemblyObject o = new AssemblyObject() { Name = name, Namespace = ns };
        //            if (type.HasFields)
        //            {
        //                foreach (FieldDefinition f in type.Fields)
        //                {
        //                    if (f.IsPublic)
        //                    {
        //                        string fname = f.Name;
        //                        o.Fields.Add(new AssemblyObject() { Name = fname });
        //                    }
        //                }
        //            }
        //            if (type.HasProperties)
        //            {
        //                foreach (PropertyDefinition p in type.Properties)
        //                {
        //                    string pname = p.Name;
        //                    o.Properties.Add(new AssemblyObject() { Name = pname });
        //                }
        //            }
        //            if (type.HasMethods)
        //            {
        //                foreach (MethodDefinition m in type.Methods)
        //                {
        //                    if (m.IsPublic)
        //                    {
        //                        string mname = m.Name;

        //                        AssemblyObject mo = new AssemblyObject() { Name = mname };
        //                        if (m.HasParameters)
        //                        {
        //                            foreach (ParameterDefinition r in m.Parameters)
        //                            {
        //                                string rname = r.Name;
        //                                mo.Methods.Add(new AssemblyObject() { Name = rname });
        //                            }
        //                        }
        //                        o.Methods.Add(mo);
        //                    }
        //                }
        //            }

        //            objects.Add(o);
        //        }
        //    }

        //    return objects;
        //}
    }
}
