using System;
using TNPSupport.AbstractSyntax;
using TNPSupport.CodeGeneration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class HelloWorldGenerator : ICodeGenerator
	{
		public void Generate(IGenerators environment, IASTNode node)
		{
			if (environment is GeneratorsIL env && node is HelloWorldNode hello)
				Generate (env, hello);
		}

		void Generate (GeneratorsIL env, HelloWorldNode node)
		{
			
		}

		public bool Matches(IASTNode node)
		{
			return node is HelloWorldNode;
		}
	}
}

