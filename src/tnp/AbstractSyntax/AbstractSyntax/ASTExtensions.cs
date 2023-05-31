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
	}
}

