using System;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace ILCodeGeneration
{
	public class Environment
	{
		string outputDirectory;
		public Environment (string name, string outputDirectory)
		{
			Name = name;
			this.outputDirectory = outputDirectory;
			var mp = new ModuleParameters { Architecture = TargetArchitecture.AMD64, Kind = ModuleKind.Console, AssemblyResolver = new SimpleAssemblyResolver () };
			Assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition (name, Version.Parse ("1.0.0.0")), Path.GetFileName (name + ".exe"), mp);
		}

		public string Name { get; private set; }
		public AssemblyDefinition Assembly { get; set; }

		public List<TypeDefinition> Types { get; private set; } = new List<TypeDefinition> ();
	}
}

