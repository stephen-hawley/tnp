using System;
using System.Diagnostics.CodeAnalysis;

namespace TNPSupport.AbstractSyntax
{
	public class TNPTypeFactory
	{
		static object cachelock = new object ();
		static Dictionary<string, TNPType> cache = new Dictionary<string, TNPType> () {
			{ "System.Void", new TNPType ("System", "Void", "void") },
			{ "System.String", new TNPType ("System", "String", "string") },
			{ "System.Boolean", new TNPType ("System", "Boolean", "bool") },
			{ "System.SByte", new TNPType ("System", "SByte", "sbyte") },
			{ "System.Byte", new TNPType ("System", "Byte", "byte") },
			{ "System.Int16", new TNPType ("System", "Int16", "short") },
			{ "System.UInt16", new TNPType ("System", "UInt16", "ushort") },
			{ "System.Int32", new TNPType ("System", "Int32", "int") },
			{ "System.UInt32", new TNPType ("System", "UInt32", "uint") },
			{ "System.Int64", new TNPType ("System", "Int64", "long") },
			{ "System.UInt64", new TNPType ("System", "UInt64", "ulong") },
		};

		public static TNPType FromType (Type t)
		{
			lock (cachelock) {
				if (cache.TryGetValue (t.FullName!, out var type)) {
					return type;
				}
				var ns = t.Namespace!;
				var n = t.Name;
				var newType = new TNPType (ns, n);
				cache.Add (t.FullName!, newType);
				return newType;
			}
		}

		public static TNPType FromTypeName (string nameSpace, string typeName)
		{
			var ty = new TNPType (nameSpace, typeName);
			lock (cache) {
				cache.Add ($"{nameSpace}.{typeName}", ty);
				return ty;
			}
		}

		public static bool TryRemove (string fullName, out TNPType? ty)
		{
			lock (cachelock) {
				if (cache.TryGetValue (fullName, out ty)) {
					cache.Remove (fullName);
					return true;
				}
				ty = null;
				return false;
			}
		}

		public static bool TryRemove (TNPType ty)
		{
			lock (cachelock) {
				var fullName = ty.FullName;
				if (cache.ContainsKey (fullName)) {
					cache.Remove (fullName);
					return true;
				}
				return false;
			}
		}

		public bool TryGet (string fullName, [NotNullWhen (returnValue: true)] out TNPType? ty)
		{
			lock (cachelock) {
				return cache.TryGetValue (fullName, out ty);
			}
		}

		public static TNPType Void => cache.GetValueOrDefault ("System.Void")!;
	}
}

