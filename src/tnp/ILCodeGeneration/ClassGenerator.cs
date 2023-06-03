using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class ClassGenerator : ICodeGenerator
	{
		public async Task Generate(ICodeGenerators environment, IASTNode node)
		{
			if (environment is CodeGeneratorsIL gen && node is ClassNode cl)
				await Generate (gen, cl);
		}

		async Task Generate (CodeGeneratorsIL gen, ClassNode cl)
		{
			if (gen.Environment.TryGetType (cl.FullName, out var td)) {
				foreach (var method in cl.Methods) {
					var md = ToMethodDefinition (gen, method);
					td.Methods.Add (md);
					if (gen.TryGetGenerator (method, out var methodGen)) {
						gen.Environment.MethodBegin (md);
						await methodGen.Generate (gen, method);
						gen.Environment.MethodEnd ();
					}
				}
			}
		}

		MethodDefinition ToMethodDefinition (CodeGeneratorsIL gen, MethodNode method)
		{
			var typeSystem = gen.Environment.ThrowOnNoAssembly ().MainModule.TypeSystem;
			if (method.MethodName == ".ctor") {
				return new MethodDefinition (".ctor", MethodAttributes.Public | MethodAttributes.HideBySig
								| MethodAttributes.RTSpecialName | MethodAttributes.SpecialName, typeSystem.Void);
			} else if (method.MethodName == "Main") {
				var md = new MethodDefinition ("Main", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, typeSystem.Void);
				var parameter = new ParameterDefinition ("args", ParameterAttributes.None, typeSystem.String.MakeArrayType ());
				md.Parameters.Add (parameter);
				return md;
			} else {
				// todo - actually define the full method
				throw new NotImplementedException ();
			}
		}

		public bool Matches(IASTNode node)
		{
			return node is ClassNode;
		}
	}
}

