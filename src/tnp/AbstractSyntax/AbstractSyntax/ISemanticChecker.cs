using System;
namespace TNPSupport.AbstractSyntax
{
	public interface ISemanticChecker
	{
		bool Matches (IASTNode node);
		SemanticResult Check (ISemantics environment, IASTNode node, List<SemanticError> errors);
	}
}

