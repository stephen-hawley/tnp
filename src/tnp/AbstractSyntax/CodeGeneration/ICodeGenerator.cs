using System;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.CodeGeneration
{
	public interface ICodeGenerator
	{
		bool Matches (IASTNode node);
		void Generate (IGenerators environment, IASTNode node);
	}
}

