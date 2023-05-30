using System;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.CodeGeneration
{
	public interface IGenerators
	{
		string Name { get; }
		bool TryGetGenerator (IASTNode node, out ICodeGenerator? generator);

		// maybe make these async
		void Begin (string name, string outputDirectory);
		void End ();
	}
}

