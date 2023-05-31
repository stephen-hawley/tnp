using System;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.CodeGeneration
{
	public interface ICodeGenerator
	{
		bool Matches (IASTNode node);
		Task Generate (ICodeGenerators environment, IASTNode node);
	}
}

