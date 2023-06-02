using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class MethodGenerator : ICodeGenerator
	{
		public async Task Generate(ICodeGenerators environment, IASTNode node)
		{
			if (environment is CodeGeneratorsIL gen && node is MethodNode m)
				await Generate (gen, m);
		}

		async Task Generate (CodeGeneratorsIL gen, MethodNode m)
		{
			if (m.Parent is InstanceBase cl && gen.Environment.TryGetType (cl.FullName, out var td)) {
				var il = gen.Environment.CurrentILProcessors.Peek ();
				if (m.MethodName == ".ctor") {
					// todo: match the ctor
					var monoCl = gen.Environment.CurrentMethods.Peek ().DeclaringType;
					var assembly = gen.Environment.ThrowOnNoAssembly ();
					if (monoCl.BaseType.DefaultCtor (out var baseCtor)) {
						il.Emit (OpCodes.Ldarg_0);
						il.Emit (OpCodes.Call, assembly.MainModule.ImportReference (baseCtor));
					}

				}
				// todo:
				// 1. create a map of MethodNode -> MethodDefinition in ClassGenerator
				// 2. call MethodBegin ()/MethodEnd () here
				// 3. add bindings
				foreach (var st in m.Statements) {
					if (gen.TryGetGenerator (st, out var stGen))
						await stGen.Generate (gen, st);
				}
				il.Append (gen.Environment.CurrentMethodReturn.Peek ());
			}
		}

		public bool Matches(IASTNode node)
		{
			return node is MethodNode;
		}
	}
}

