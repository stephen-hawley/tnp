using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class ConstantStringGenerator : ICodeGenerator
	{
		public async Task Generate(ICodeGenerators environment, IASTNode node)
		{
			if (environment is CodeGeneratorsIL gen && node is ConstantString strNode) {
				await Generate (gen, strNode);
			}
		}

		async Task Generate (CodeGeneratorsIL gen, ConstantString str)
		{
			gen.Environment.ThrowOnNoMethod ();

			await Task.Run (() => {
				var il = gen.Environment.CurrentILProcessors.Peek ();
				il.Emit (OpCodes.Ldstr, str.Value);
			});
		}

		public bool Matches(IASTNode node)
		{
			return node is ConstantString;
		}
	}
}

