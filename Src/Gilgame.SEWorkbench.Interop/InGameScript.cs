using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Medieval.ObjectBuilders;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces;
using VRage;
using VRage.Compiler;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;


namespace Gilgame.SEWorkbench.Interop
{
    public class InGameScript
    {
        private static bool _Initialized = false;

        private List<String> _CompileErrors = new List<string>();
        public List<String> CompileErrors
        {
            get
            {
                return _CompileErrors;
            }
        }

        public InGameScript(string program)
        {
            if (!_Initialized)
            {
                Init();
            }

            Assembly temp = null;
            Sandbox.Game.Gui.MyGuiScreenEditor.CompileProgram(program, _CompileErrors, ref temp);
        }

        public static void Init()
        {
            InitIlChecker();
            InitIlCompiler();

            _Initialized = true;
        }

        private static void InitIlChecker()
        {
            IlChecker.AllowedOperands = new Dictionary<Type, List<MemberInfo>>();
            IlChecker.AllowedNamespacesCommon = new Dictionary<Assembly, List<string>>();
            IlChecker.AllowNamespaceOfTypeCommon(typeof(IEnumerator));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(IEnumerable<>));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(HashSet<>));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Queue<>));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(ListExtensions));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Enumerable));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(StringBuilder));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Regex));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Calendar));
            IlChecker.AllowedOperands.Add(typeof(object), null);
            IlChecker.AllowedOperands.Add(typeof(IDisposable), null);
            IlChecker.AllowedOperands.Add(typeof(StringBuilder), null);
            IlChecker.AllowedOperands.Add(typeof(string), null);
            IlChecker.AllowedOperands.Add(typeof(Math), null);
            IlChecker.AllowedOperands.Add(typeof(Enum), null);
            IlChecker.AllowedOperands.Add(typeof(int), null);
            IlChecker.AllowedOperands.Add(typeof(short), null);
            IlChecker.AllowedOperands.Add(typeof(long), null);
            IlChecker.AllowedOperands.Add(typeof(uint), null);
            IlChecker.AllowedOperands.Add(typeof(ushort), null);
            IlChecker.AllowedOperands.Add(typeof(ulong), null);
            IlChecker.AllowedOperands.Add(typeof(double), null);
            IlChecker.AllowedOperands.Add(typeof(float), null);
            IlChecker.AllowedOperands.Add(typeof(bool), null);
            IlChecker.AllowedOperands.Add(typeof(char), null);
            IlChecker.AllowedOperands.Add(typeof(byte), null);
            IlChecker.AllowedOperands.Add(typeof(sbyte), null);
            IlChecker.AllowedOperands.Add(typeof(decimal), null);
            IlChecker.AllowedOperands.Add(typeof(DateTime), null);
            IlChecker.AllowedOperands.Add(typeof(TimeSpan), null);
            IlChecker.AllowedOperands.Add(typeof(Array), null);
            IlChecker.AllowedOperands.Add(typeof(XmlElementAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAttributeAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlArrayAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlArrayItemAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAnyAttributeAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAnyElementAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAnyElementAttributes), null);
            IlChecker.AllowedOperands.Add(typeof(XmlArrayItemAttributes), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAttributeEventArgs), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAttributeOverrides), null);
            IlChecker.AllowedOperands.Add(typeof(XmlAttributes), null);
            IlChecker.AllowedOperands.Add(typeof(XmlChoiceIdentifierAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlElementAttributes), null);
            IlChecker.AllowedOperands.Add(typeof(XmlElementEventArgs), null);
            IlChecker.AllowedOperands.Add(typeof(XmlEnumAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlIgnoreAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlIncludeAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlRootAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlTextAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(XmlTypeAttribute), null);
            IlChecker.AllowedOperands.Add(typeof(RuntimeHelpers), null);
            IlChecker.AllowedOperands.Add(typeof(Stream), null);
            IlChecker.AllowedOperands.Add(typeof(TextWriter), null);
            IlChecker.AllowedOperands.Add(typeof(TextReader), null);
            IlChecker.AllowedOperands.Add(typeof(BinaryReader), null);
            IlChecker.AllowedOperands.Add(typeof(BinaryWriter), null);
            IlChecker.AllowedOperands.Add(typeof(CompilerHelper), null);

            List<MemberInfo> list = new List<MemberInfo>();
            list.Add(typeof(MemberInfo).GetProperty("Name").GetGetMethod());
            IlChecker.AllowedOperands.Add(typeof(MemberInfo), list);

            list = new List<MemberInfo>();
            list.Add(typeof(Type).GetMethod("GetTypeFromHandle"));
            IlChecker.AllowedOperands.Add(typeof(Type), list);

            Type type = typeof(Type).Assembly.GetType("System.RuntimeType");
            IlChecker.AllowedOperands[type] = new List<MemberInfo>
			{
				type.GetMethod("op_Inequality"),
				type.GetMethod("GetFields", new Type[]
				{
					typeof(BindingFlags)
				})
			};
            IlChecker.AllowedOperands[typeof(Type)] = new List<MemberInfo>
			{
				typeof(Type).GetMethod("GetFields", new Type[]
				{
					typeof(BindingFlags)
				}),
				typeof(Type).GetMethod("IsEquivalentTo"),
				typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public),
				typeof(Type).GetMethod("op_Equality")
			};
            Type type2 = typeof(Type).Assembly.GetType("System.Reflection.RtFieldInfo");
            IlChecker.AllowedOperands[type2] = new List<MemberInfo>
			{
				type2.GetMethod("UnsafeGetValue", BindingFlags.Instance | BindingFlags.NonPublic)
			};
            IlChecker.AllowedOperands[typeof(NullReferenceException)] = null;
            IlChecker.AllowedOperands[typeof(ArgumentException)] = null;
            IlChecker.AllowedOperands[typeof(ArgumentNullException)] = null;
            IlChecker.AllowedOperands[typeof(InvalidOperationException)] = null;
            IlChecker.AllowedOperands[typeof(FormatException)] = null;
            IlChecker.AllowedOperands.Add(typeof(Exception), null);
            IlChecker.AllowedOperands.Add(typeof(DivideByZeroException), null);
            IlChecker.AllowedOperands.Add(typeof(InvalidCastException), null);
            IlChecker.AllowedOperands.Add(typeof(FileNotFoundException), null);
            typeof(MethodInfo).Assembly.GetType("System.Reflection.RuntimeMethodInfo");
            IlChecker.AllowedOperands[typeof(ValueType)] = new List<MemberInfo>
			{
				typeof(ValueType).GetMethod("Equals"),
				typeof(ValueType).GetMethod("GetHashCode"),
				typeof(ValueType).GetMethod("ToString"),
				typeof(ValueType).GetMethod("CanCompareBits", BindingFlags.Static | BindingFlags.NonPublic),
				typeof(ValueType).GetMethod("FastEqualsCheck", BindingFlags.Static | BindingFlags.NonPublic)
			};
            Type typeFromHandle = typeof(Environment);
            IlChecker.AllowedOperands[typeFromHandle] = new List<MemberInfo>
			{
				typeFromHandle.GetMethod("GetResourceString", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string),
					typeof(object[])
				}, null),
				typeFromHandle.GetMethod("GetResourceString", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string)
				}, null)
			};
            IlChecker.AllowedOperands[typeof(Path)] = null;
            IlChecker.AllowedOperands[typeof(Random)] = null;
            IlChecker.AllowedOperands[typeof(Convert)] = null;
            IlChecker.AllowedOperands.Add(typeof(Nullable<>), null);
            IlChecker.AllowedOperands.Add(typeof(StringComparer), null);
            IlChecker.AllowedOperands.Add(typeof(IComparable<>), null);
        }

        private static void InitIlCompiler()
        {
            Func<string, string> func = (string x) => Path.Combine(VRage.FileSystem.MyFileSystem.ExePath, x);
            IlCompiler.Options = new System.CodeDom.Compiler.CompilerParameters(new string[]
			{
				func("SpaceEngineers.ObjectBuilders.dll"),
				func("MedievalEngineers.ObjectBuilders.dll"),
				func("Sandbox.Game.dll"),
				func("Sandbox.Common.dll"),
				func("Sandbox.Graphics.dll"),
				func("VRage.dll"),
				func("VRage.Library.dll"),
				func("VRage.Math.dll"),
				func("VRage.Game.dll"),
				"System.Xml.dll",
				"System.Core.dll",
				"System.dll"
			});
        }
    }
}
