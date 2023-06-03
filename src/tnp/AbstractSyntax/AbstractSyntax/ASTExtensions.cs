using System;
namespace TNPSupport.AbstractSyntax
{
	public static class ASTExtensions
	{
		public static IEnumerable<ClassNode> Classes (this IASTNode node)
		{
			return Trees.AllNodes (node).OfType<ClassNode> ();
		}

		public static IEnumerable<InstanceBase> NominalTypes (this IASTNode node)
		{
			return Trees.AllNodes (node).OfType<InstanceBase> ();
		}

		public static IEnumerable<MethodNode> MethodsInClass (this ClassNode cl, string methodName)
		{
			return cl.Methods.Where (m => m.MethodName == methodName);
		}

		public static IEnumerable<MethodNode> MethodsByName (this TopLevelNode tl, string fullClassName, string methodName)
		{
			var cl = tl.Classes ().Where (cl => cl.FullName == fullClassName).FirstOrDefault ();
			if (cl is null) {
				return Enumerable.Empty<MethodNode> ();
			}
			return cl.MethodsInClass (methodName);
		}
	}
}

