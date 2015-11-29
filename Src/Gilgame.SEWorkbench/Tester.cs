using System;
using System.IO;
using System.Reflection;
using System.Windows;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Ast;
using Mono.Cecil;

namespace Gilgame.SEWorkbench
{
    public static class Tester
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string directory = Directory.GetCurrentDirectory();

            var resolver = new MyDefaultAssemblyResolver();
            resolver.AddSearchDirectory(directory);
            var parameters = new ReaderParameters
            {
                AssemblyResolver = resolver,
            };

            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("Sandbox.Common.dll", parameters);
        }
    }

    public class MyDefaultAssemblyResolver : DefaultAssemblyResolver
    {
        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            try
            {
                return base.Resolve(name);
            }
            catch { }
            return null;
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            try
            {
                return base.Resolve(name, parameters);
            }
            catch { }
            return null;
        }

        public override AssemblyDefinition Resolve(string fullName)
        {
            try
            {
                return base.Resolve(fullName);
            }
            catch { }
            return null;
        }

        public override AssemblyDefinition Resolve(string fullName, ReaderParameters parameters)
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
