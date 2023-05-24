using System;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.CodeGeneration
{
	public interface IGenerators
	{
		string Name { get; }
		bool TryGetGenerator (IASTNode node, out ICodeGenerator generator);
	}
}

