using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Interop
{
    public class IlChecker
    {
        public static Dictionary<Type, List<MemberInfo>> AllowedOperands;

        public static Dictionary<Assembly, List<string>> AllowedNamespacesCommon;

        public static Dictionary<Assembly, List<string>> AllowedNamespacesModAPI;

        static IlChecker()
        {
            IlChecker.AllowedOperands = new Dictionary<Type, List<MemberInfo>>();
            IlChecker.AllowedNamespacesCommon = new Dictionary<Assembly, List<string>>();
            IlChecker.AllowedNamespacesModAPI = new Dictionary<Assembly, List<string>>();
            IlChecker.AllowedOperands.Add(typeof(object), null);
            IlChecker.AllowedOperands.Add(typeof(IDisposable), null);
            IlChecker.AllowNamespaceOfTypeCommon(typeof(IEnumerator));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(IEnumerable<>));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(HashSet<>));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Queue<>));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(ListExtensions));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Enumerable));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(StringBuilder));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Regex));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Timer));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Calendar));
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
            List<MemberInfo> list = new List<MemberInfo>();
            list.Add(typeof(MemberInfo).GetProperty("Name").GetGetMethod());
            IlChecker.AllowedOperands.Add(typeof(MemberInfo), list);
            IlChecker.AllowedOperands.Add(typeof(RuntimeHelpers), null);
            IlChecker.AllowedOperands.Add(typeof(Stream), null);
            IlChecker.AllowedOperands.Add(typeof(TextWriter), null);
            IlChecker.AllowedOperands.Add(typeof(TextReader), null);
            IlChecker.AllowedOperands.Add(typeof(BinaryReader), null);
            IlChecker.AllowedOperands.Add(typeof(BinaryWriter), null);
            IlChecker.AllowedOperands.Add(typeof(CompilerHelper), null);
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

        public static void AllowNamespaceOfTypeModAPI(Type type)
        {
            if (!IlChecker.AllowedNamespacesModAPI.ContainsKey(type.Assembly))
            {
                IlChecker.AllowedNamespacesModAPI.Add(type.Assembly, new List<string>());
            }
            IlChecker.AllowedNamespacesModAPI[type.Assembly].Add(type.Namespace);
        }

        public static void AllowNamespaceOfTypeCommon(Type type)
        {
            if (!IlChecker.AllowedNamespacesCommon.ContainsKey(type.Assembly))
            {
                IlChecker.AllowedNamespacesCommon.Add(type.Assembly, new List<string>());
            }
            IlChecker.AllowedNamespacesCommon[type.Assembly].Add(type.Namespace);
        }

        /// <summary>
        /// Checks list of IL instructions against dangerous types
        /// </summary>
        /// <param name="dangerousTypeNames">Full names of dangerous types</param>
        public static bool CheckIl(List<VRage.Compiler.IlReader.IlInstruction> instructions, out Type failed, bool isIngameScript, Dictionary<Type, List<MemberInfo>> allowedTypes = null)
        {
            failed = null;
            foreach (KeyValuePair<Type, List<MemberInfo>> current in allowedTypes)
            {
                if (!IlChecker.AllowedOperands.Contains(current))
                {
                    IlChecker.AllowedOperands.Add(current.Key, current.Value);
                }
            }
            foreach (VRage.Compiler.IlReader.IlInstruction current2 in instructions)
            {
                MethodInfo methodInfo = current2.Operand as MethodInfo;
                if (methodInfo != null && IlChecker.HasMethodInvalidAtrributes(methodInfo.Attributes))
                {
                    bool result = false;
                    return result;
                }
                if (!IlChecker.CheckMember(current2.Operand as MemberInfo, isIngameScript) || current2.OpCode == OpCodes.Calli)
                {
                    failed = ((MemberInfo)current2.Operand).DeclaringType;
                    bool result = false;
                    return result;
                }
            }
            return true;
        }

        private static bool CheckMember(MemberInfo memberInfo, bool isIngameScript)
        {
            return memberInfo == null || IlChecker.CheckTypeAndMember(memberInfo.DeclaringType, isIngameScript, memberInfo);
        }

        public static bool CheckTypeAndMember(Type type, bool isIngameScript, MemberInfo memberInfo = null)
        {
            return type == null || IlChecker.IsDelegate(type) || (!type.IsGenericTypeDefinition && type.IsGenericType && IlChecker.CheckGenericType(type.GetGenericTypeDefinition(), memberInfo, isIngameScript)) || (IlChecker.CheckNamespace(type, isIngameScript) || IlChecker.CheckOperand(type, memberInfo, IlChecker.AllowedOperands));
        }

        private static bool IsDelegate(Type type)
        {
            Type typeFromHandle = typeof(MulticastDelegate);
            return typeFromHandle.IsAssignableFrom(type.BaseType) || type == typeFromHandle || type == typeFromHandle.BaseType;
        }

        private static bool CheckNamespace(Type type, bool isIngameScript)
        {
            if (type == null)
            {
                return false;
            }
            bool flag = IlChecker.AllowedNamespacesCommon.ContainsKey(type.Assembly) && IlChecker.AllowedNamespacesCommon[type.Assembly].Contains(type.Namespace);
            if (!flag && !isIngameScript)
            {
                flag = (IlChecker.AllowedNamespacesModAPI.ContainsKey(type.Assembly) && IlChecker.AllowedNamespacesModAPI[type.Assembly].Contains(type.Namespace));
            }
            return flag;
        }

        private static bool CheckOperand(Type type, MemberInfo memberInfo, Dictionary<Type, List<MemberInfo>> op)
        {
            return op != null && op.ContainsKey(type) && (memberInfo == null || op[type] == null || op[type].Contains(memberInfo));
        }

        private static bool CheckGenericType(Type declType, MemberInfo memberInfo, bool isIngameScript)
        {
            if (IlChecker.CheckTypeAndMember(declType, isIngameScript, memberInfo))
            {
                if (memberInfo != null)
                {
                    Type[] genericArguments = memberInfo.DeclaringType.GetGenericArguments();
                    for (int i = 0; i < genericArguments.Length; i++)
                    {
                        Type type = genericArguments[i];
                        if (!type.IsGenericParameter && !IlChecker.CheckTypeAndMember(type, isIngameScript, null))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public static bool HasMethodInvalidAtrributes(MethodAttributes Attributes)
        {
            return (Attributes & (MethodAttributes.PinvokeImpl | MethodAttributes.UnmanagedExport)) != MethodAttributes.PrivateScope;
        }

        public static bool IsMethodFromParent(Type classType, MethodBase method)
        {
            return classType.IsSubclassOf(method.DeclaringType);
        }

        public static void Clear()
        {
            IlChecker.AllowedOperands.Clear();
            IlChecker.AllowedNamespacesCommon.Clear();
            IlChecker.AllowedNamespacesModAPI.Clear();
        }
    }
}
