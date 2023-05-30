using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;

namespace ILCodeGeneration
{
	public class GeneratorsIL : IGenerators
	{
		List<ICodeGenerator> generators = new List<ICodeGenerator> () {
			new HelloWorldGenerator ()
		};

		public GeneratorsIL ()
		{
		}

		public string Name => "IL";

		public bool TryGetGenerator(IASTNode node, out ICodeGenerator? generator)
		{
			foreach (var gen in generators) {
				if (gen.Matches (node)) {
					generator = gen;
					return true;
				}
			}
			generator = null;
			return false;
		}

		public void Begin (string name, string outputDirectory)
		{

		}

		public void End ()
		{

		}
	}
}

