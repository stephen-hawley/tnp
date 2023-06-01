using System;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.Semantics
{
	public interface ISemanticChecker
	{
		bool Matches (IASTNode node);
		SemanticResult Check (ISemanticCheckers environment, IASTNode node, List<SemanticError> errors);
	}
}

