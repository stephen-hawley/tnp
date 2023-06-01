using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class PrintGenerator : ICodeGenerator
	{
		public async Task Generate (ICodeGenerators environment, IASTNode node)
		{
			if (environment is CodeGeneratorsIL gen && node is PrintNode print)
				await Generate (gen, print);
		}

		public async Task Generate (CodeGeneratorsIL gen, PrintNode print)
		{
			gen.Environment.ThrowOnNoMethod ();
			await Task.Run (async () => {
				if (gen.TryGetGenerator (print.Value, out var strGen)) {
					await strGen.Generate (gen, print.Value);
				} else {
					throw new Exception ("");
				}

				// TODO: check the type of the value and if it's a value type
				// box it and change the flavor of Write to the one that
				// takes System.Object
				
				var assembly = gen.Environment.ThrowOnNoAssembly ();
				var il = gen.Environment.CurrentILProcessors.Peek ();
				var writeLine = typeof (System.Console).ResolveMethod ("Write",
				System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
					"System.String");
				il.Emit (OpCodes.Call, assembly.MainModule.ImportReference (writeLine));

			});
		}

		public bool Matches(IASTNode node)
		{
			return node is PrintNode;
		}
	}
}

