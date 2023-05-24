using System;
namespace TNPSupport.AbstractSyntax
{
	public interface ISemantics
	{
		public bool TryGetSemanticChecker (IASTNode node, out ISemanticChecker checker);
	}
}

