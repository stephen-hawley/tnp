using System;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.Semantics
{
	public interface ISemanticCheckers
	{
		public bool TryGetSemanticChecker (IASTNode node, out ISemanticChecker checker);
	}
}

