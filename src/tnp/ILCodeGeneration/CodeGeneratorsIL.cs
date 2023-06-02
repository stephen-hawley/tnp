using System;
using System.Diagnostics.CodeAnalysis;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;

namespace ILCodeGeneration
{
	public class CodeGeneratorsIL : ICodeGenerators
	{
		List<ICodeGenerator> generators = new List<ICodeGenerator> () {
			new HelloWorldGenerator (),
			new PrintGenerator (),
			new ConstantStringGenerator (),
			new ClassGenerator (),
			new MethodGenerator (),
			new TopLevelGenerator (),
		};

		public CodeGeneratorsIL ()
		{
		}

		public string Name => "IL";

		public bool TryGetGenerator(IASTNode node, [NotNullWhen (returnValue: true)] out ICodeGenerator? generator)
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
			Environment = new Environment (name, outputDirectory);
			Environment.Begin ();
		}

		public void End ()
		{
			Environment.End ();
		}

		public Environment Environment { get; private set; } = new Environment ("", "");
	}
}

