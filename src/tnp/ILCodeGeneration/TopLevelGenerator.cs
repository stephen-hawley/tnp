using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class TopLevelGenerator : ICodeGenerator
	{
		public TopLevelGenerator()
		{
		}

		public async Task Generate(ICodeGenerators environment, IASTNode node)
		{
			if (environment is CodeGeneratorsIL gen && node is TopLevelNode tl)
				await Generate (gen, tl);
		}

		public async Task Generate (CodeGeneratorsIL gen, TopLevelNode tl)
		{
			DefineTypes (gen, tl);
			await CompileTypes (gen, tl);
			if (tl.EntryPoint is not null && tl.EntryPoint.Parent is NominalBase nb) {
				var assem = gen.Environment.ThrowOnNoAssembly ();
				var td = assem.MainModule.GetType (nb.FullName);
				if (td is not null) {
					var md = td.Methods.Where (m => m.Name == tl.EntryPoint.MethodName).FirstOrDefault ();
					assem.MainModule.EntryPoint = md;
				}
			}
		}

		public async Task CompileTypes (CodeGeneratorsIL gen, TopLevelNode tl)
		{
			var assem = gen.Environment.ThrowOnNoAssembly ();
			foreach (var cl in tl.TypeNodes) {
				if (gen.TryGetGenerator (cl, out var clGen)) {
					await clGen.Generate (gen, cl);
				}
			}
		}

		void DefineTypes (CodeGeneratorsIL gen, TopLevelNode tl)
		{
			var assem = gen.Environment.ThrowOnNoAssembly ();
			foreach (var cl in tl.Classes ()) {
				var attr = TypeAttributes.AnsiClass;
				attr = FromClassVariety (cl, attr);
				attr = FromClassVisibility (cl, attr);
				var inherits = cl.Inheritance.FullName == "System.Object" ? assem.MainModule.TypeSystem.Object : throw new Exception ("don't have this type of inheritance yet");
				var td = new TypeDefinition (cl.Namespace, cl.TypeName, attr, assem.MainModule.ImportReference (inherits));
				// todo: add interfaces
				gen.Environment.AddType (td);
				// todo: create method stubs
			}
		}

		TypeAttributes FromClassVariety (ClassNode node, TypeAttributes attr)
		{
			switch (node.Variety) {
			case ClassVariety.Abstract:
				attr |= TypeAttributes.Abstract;
				break;
			case ClassVariety.Static:
				attr |= TypeAttributes.Sealed;
				break;
			case ClassVariety.None:
				break;
			}
			return attr;
		}

		TypeAttributes FromClassVisibility (ClassNode cl, TypeAttributes attr)
		{
			switch (cl.Visibility) {
			case Visibility.Public:
				attr |= TypeAttributes.Public;
				break;
			case Visibility.Private:
				attr |= TypeAttributes.NotPublic; // this does nothing
				break;
			case Visibility.Protected:
				attr |= TypeAttributes.NestedFamily;
				break;
			}
			return attr;
		}

		public bool Matches(IASTNode node)
		{
			return node is TopLevelNode;
		}
	}
}

