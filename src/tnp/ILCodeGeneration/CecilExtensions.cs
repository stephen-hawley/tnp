using System;
using System.Diagnostics.CodeAnalysis;
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
	}
}

