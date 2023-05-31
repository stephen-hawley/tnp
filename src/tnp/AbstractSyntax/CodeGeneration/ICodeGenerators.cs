using System;
using System.Diagnostics.CodeAnalysis;
using TNPSupport.AbstractSyntax;

namespace TNPSupport.CodeGeneration
{
	public interface ICodeGenerators
	{
		string Name { get; }
		bool TryGetGenerator (IASTNode node, [NotNullWhen (returnValue: true)] out ICodeGenerator? generator);

		// maybe make these async
		void Begin (string name, string outputDirectory);
		void End ();
	}
}

