using System;

namespace Gilgame.SEWorkbench.Interop
{
    /// <summary>
    /// Credit to Torston over at Stack Overflow
    /// http://stackoverflow.com/questions/13526519/ilspy-failed-to-resolve-assembly-in-astbuilder
    /// </summary>
    public class AssemblyResolver : Mono.Cecil.DefaultAssemblyResolver
    {
        public override Mono.Cecil.AssemblyDefinition Resolve(Mono.Cecil.AssemblyNameReference name)
        {
            try
            {
                return base.Resolve(name);
            }
            catch { }
            return null;
        }

        public override Mono.Cecil.AssemblyDefinition Resolve(Mono.Cecil.AssemblyNameReference name, Mono.Cecil.ReaderParameters parameters)
        {
            try
            {
                return base.Resolve(name, parameters);
            }
            catch { }
            return null;
        }

        public override Mono.Cecil.AssemblyDefinition Resolve(string fullName)
        {
            try
            {
                return base.Resolve(fullName);
            }
            catch { }
            return null;
        }

        public override Mono.Cecil.AssemblyDefinition Resolve(string fullName, Mono.Cecil.ReaderParameters parameters)
        {
            try
            {
                return base.Resolve(fullName, parameters);
            }
            catch { }
            return null;
        }
    }
}
