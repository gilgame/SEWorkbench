using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using VRage.FileSystem;

namespace Gilgame.SEWorkbench.Interop
{
    public class IlCompiler
    {
        private const string invokeWrapper = "public static class wrapclass{{ public static object run() {{ {0} return null;}} }}";

        public static CompilerParameters Options;

        private static CSharpCodeProvider m_cp;

        private static VRage.Compiler.IlReader m_reader;

        private static Dictionary<string, string> m_compatibilityChanges;

        private static StringBuilder m_cache;

        public static StringBuilder Buffer;

        static IlCompiler()
        {
            IlCompiler.m_cp = new CSharpCodeProvider();
            IlCompiler.m_reader = new VRage.Compiler.IlReader();
            IlCompiler.m_compatibilityChanges = new Dictionary<string, string>
			{
				{
					"using VRage.Common.Voxels;",
					""
				},
				{
					"VRage.Common.Voxels.",
					""
				},
				{
					"Sandbox.ModAPI.IMyEntity",
					"VRage.ModAPI.IMyEntity"
				},
				{
					"Sandbox.Common.ObjectBuilders.MyObjectBuilder_EntityBase",
					"VRage.ObjectBuilders.MyObjectBuilder_EntityBase"
				},
				{
					"Sandbox.Common.MyEntityUpdateEnum",
					"VRage.ModAPI.MyEntityUpdateEnum"
				},
				{
					"using Sandbox.Common.ObjectBuilders.Serializer;",
					""
				},
				{
					"Sandbox.Common.ObjectBuilders.Serializer.",
					""
				},
				{
					"Sandbox.Common.MyMath",
					"VRageMath.MyMath"
				},
				{
					"Sandbox.Common.ObjectBuilders.VRageData.SerializableVector3I",
					"VRage.SerializableVector3I"
				}
			};
            IlCompiler.m_cache = new StringBuilder();
            IlCompiler.Buffer = new StringBuilder();
            IlCompiler.Options = new CompilerParameters(new string[]
			{
				"System.Xml.dll",
				"Sandbox.Game.dll",
				"Sandbox.Common.dll",
				"Sandbox.Graphics.dll",
				"VRage.dll",
				"VRage.Library.dll",
				"VRage.Math.dll",
				"VRage.Game.dll",
				"System.Core.dll",
				"System.dll",
				"SpaceEngineers.ObjectBuilders.dll",
				"MedievalEngineers.ObjectBuilders.dll"
			});
            IlCompiler.Options.GenerateInMemory = true;
        }

        public static string[] UpdateCompatibility(string[] files)
        {
            string[] array = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                using (Stream stream = MyFileSystem.OpenRead(files[i]))
                {
                    if (stream != null)
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            string text = streamReader.ReadToEnd();
                            text = text.Insert(0, "using VRage;\r\nusing VRage.Game.Components;\r\nusing VRage.ObjectBuilders;\r\nusing VRage.ModAPI;\r\nusing Sandbox.Common.ObjectBuilders;\r\n");
                            foreach (KeyValuePair<string, string> current in IlCompiler.m_compatibilityChanges)
                            {
                                text = text.Replace(current.Key, current.Value);
                            }
                            array[i] = text;
                        }
                    }
                }
            }
            return array;
        }

        public static bool CompileFileModAPI(string assemblyName, string[] files, out Assembly assembly, List<string> errors)
        {
            IlCompiler.Options.OutputAssembly = assemblyName;
            IlCompiler.Options.GenerateInMemory = true;
            string[] sources = IlCompiler.UpdateCompatibility(files);
            CompilerResults result = IlCompiler.m_cp.CompileAssemblyFromSource(IlCompiler.Options, sources);
            return IlCompiler.CheckResultInternal(out assembly, errors, result, false);
        }

        public static bool CompileStringIngame(string assemblyName, string[] source, out Assembly assembly, List<string> errors)
        {
            IlCompiler.Options.OutputAssembly = assemblyName;
            IlCompiler.Options.GenerateInMemory = true;
            IlCompiler.Options.GenerateExecutable = false;
            IlCompiler.Options.IncludeDebugInformation = false;
            CompilerResults result = IlCompiler.m_cp.CompileAssemblyFromSource(IlCompiler.Options, source);
            return IlCompiler.CheckResultInternal(out assembly, errors, result, true);
        }

        /// <summary>
        /// Checks assembly for not allowed operations (ie. accesing file system, network)
        /// </summary>
        /// <param name="tmpAssembly">output assembly</param>
        /// <param name="errors">compilation or check errors</param>
        /// <param name="result">compiled assembly</param>
        /// <param name="isIngameScript"></param>
        /// <returns>wheter the check was sucessflu (false AND null asembly on fail)</returns>
        private static bool CheckResultInternal(out Assembly assembly, List<string> errors, CompilerResults result, bool isIngameScript)
        {
            assembly = null;
            if (result.Errors.HasErrors)
            {
                IEnumerator enumerator = result.Errors.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (!(enumerator.Current as CompilerError).IsWarning)
                    {
                        errors.Add((enumerator.Current as CompilerError).ToString());
                    }
                }
                return false;
            }
            Assembly compiledAssembly = result.CompiledAssembly;
            Dictionary<Type, List<MemberInfo>> dictionary = new Dictionary<Type, List<MemberInfo>>();
            Type[] types = compiledAssembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type key = types[i];
                dictionary.Add(key, null);
            }
            List<MethodBase> list = new List<MethodBase>();
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            Type[] types2 = compiledAssembly.GetTypes();
            for (int j = 0; j < types2.Length; j++)
            {
                Type type = types2[j];
                list.Clear();
                list.AddArray(type.GetMethods(bindingAttr));
                list.AddArray(type.GetConstructors(bindingAttr));
                foreach (MethodBase current in list)
                {
                    Type type2;
                    if (IlChecker.IsMethodFromParent(type, current))
                    {
                        if (!IlChecker.CheckTypeAndMember(current.DeclaringType, isIngameScript, null))
                        {
                            errors.Add(string.Format("Class {0} derives from class {1} that is not allowed in script", type.Name, current.DeclaringType.Name));
                            bool result2 = false;
                            return result2;
                        }
                    }
                    else if (!IlChecker.CheckIl(IlCompiler.m_reader.ReadInstructions(current), out type2, isIngameScript, dictionary) || IlChecker.HasMethodInvalidAtrributes(current.Attributes))
                    {
                        errors.Add(string.Format("Type {0} used in {1} not allowed in script", (type2 == null) ? "FIXME" : type2.ToString(), current.Name));
                        bool result2 = false;
                        return result2;
                    }
                }
            }
            assembly = compiledAssembly;
            return true;
        }

        public static bool Compile(string assemblyName, string[] fileContents, out Assembly assembly, List<string> errors, bool isIngameScript)
        {
            IlCompiler.Options.OutputAssembly = assemblyName;
            CompilerResults result = IlCompiler.m_cp.CompileAssemblyFromSource(IlCompiler.Options, fileContents);
            return IlCompiler.CheckResultInternal(out assembly, errors, result, isIngameScript);
        }

        public static bool Compile(string[] instructions, out Assembly assembly, bool isIngameScript, bool wrap = true)
        {
            assembly = null;
            IlCompiler.m_cache.Clear();
            if (wrap)
            {
                IlCompiler.m_cache.AppendFormat("public static class wrapclass{{ public static object run() {{ {0} return null;}} }}", instructions);
            }
            else
            {
                IlCompiler.m_cache.Append(instructions[0]);
            }
            CompilerResults compilerResults = IlCompiler.m_cp.CompileAssemblyFromSource(IlCompiler.Options, new string[]
			{
				IlCompiler.m_cache.ToString()
			});
            if (compilerResults.Errors.HasErrors)
            {
                return false;
            }
            assembly = compilerResults.CompiledAssembly;
            Dictionary<Type, List<MemberInfo>> dictionary = new Dictionary<Type, List<MemberInfo>>();
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type key = types[i];
                dictionary.Add(key, null);
            }
            Type[] types2 = assembly.GetTypes();
            for (int j = 0; j < types2.Length; j++)
            {
                Type type = types2[j];
                MethodInfo[] methods = type.GetMethods();
                for (int k = 0; k < methods.Length; k++)
                {
                    MethodInfo method = methods[k];
                    Type type2;
                    if (!(type == typeof(MulticastDelegate)) && !IlChecker.CheckIl(IlCompiler.m_reader.ReadInstructions(method), out type2, isIngameScript, dictionary))
                    {
                        assembly = null;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
