using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Mono.Cecil;
namespace ILCodeGeneration
{
	public static class CecilExtensions
	{
		public static bool DefaultCtor (this TypeReference tr, [NotNullWhen (returnValue: true)] out MethodDefinition? method)
		{
			var ty = tr.Resolve ();
			if (ty is not null) {
				foreach (var ctor in ty.Methods.Where (m => m.Name == ".ctor")) {
					if (ctor.Parameters.Count == 0) {
						method = ctor;
						return true;
					}
				}
			}
			method = null;
			return false;
		}

		public static MethodBase ResolveMethod (this Type type, string methodName, BindingFlags bindingFlags, params string [] paramTypes)
		{
			var resolvedParameters = ToTypes (paramTypes).ToArray ();
			if (methodName == ".ctor") {
				MethodBase? resolvedCtor = type.GetConstructor (bindingFlags, null, resolvedParameters, null);
				return ThrowOnInvalidMethod (methodName, paramTypes, resolvedCtor); 
			}

			MethodBase? method = type.GetMethod (methodName, bindingFlags, null, resolvedParameters, null);
			return ThrowOnInvalidMethod (methodName, paramTypes, method);

		}

		static MethodBase ThrowOnInvalidMethod (string methodName, string[] paramTypes, MethodBase? method)
		{
			if (method is null) {
				throw new Exception ($"Unable to find method {methodName}(${string.Join (',', paramTypes)})");
			}
			return method;
		}

		static IEnumerable<Type> ToTypes (string[] names)
		{
			foreach (var name in names) {
				var type = Type.GetType (name);
				if (type is not null)
					yield return type;
				else
					throw new Exception ($"Unable to find type {name}");
			}
		}
	}
}

