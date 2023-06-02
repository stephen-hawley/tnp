using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;
using TNPSupport.AbstractSyntax;

namespace ILCodeGeneration
{
	public class Environment
	{
		string outputDirectory;
		public Environment (string name, string outputDirectory)
		{
			Name = name;
			this.outputDirectory = outputDirectory;
		}

		public void Begin ()
		{
			var mp = new ModuleParameters { Architecture = TargetArchitecture.AMD64, Kind = ModuleKind.Console, AssemblyResolver = new SimpleAssemblyResolver () };
			Assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition (Name, Version.Parse ("1.0.0.0")), Path.GetFileName (Name + ".exe"), mp);
		}

		public void End ()
		{
			if (Assembly is not null) {
				Assembly.Write (Path.Combine (outputDirectory, Name + ".exe"));
				Assembly.Dispose ();
			}
		}

		public string Name { get; private set; }
		public AssemblyDefinition? Assembly { get; set; }

		public void AddType (TypeDefinition type)
		{
			if (Assembly is not null) {
				var ty = Assembly.MainModule.Types.FirstOrDefault (t => t.FullName == type.FullName);
				if (ty == type)
					return;
				if (ty is not null)
					Assembly.MainModule.Types.Remove (ty);
				Assembly.MainModule.Types.Add (type);
			}
		}

		public bool TryGetType (string fullName, [NotNullWhen (returnValue: true)] out TypeDefinition? type)
		{
			type = Assembly?.MainModule.Types.FirstOrDefault (t => t.FullName == fullName);
			return type is not null;
		}

		[return: NotNull]
		public AssemblyDefinition ThrowOnNoAssembly ()
		{
			if (Assembly is null)
				throw new Exception ("No current assembly");
			return Assembly;
		}

		public void ThrowOnNoMethod ()
		{
			if (CurrentILProcessors.Count <= 0 || CurrentMethods.Count <= 0)
				throw new Exception ("no current method being compiled");
		}

		public void MethodBegin (MethodDefinition m)
		{
			CurrentMethods.Push (m);
			CurrentILProcessors.Push (m.Body.GetILProcessor ());
			CurrentMethodReturn.Push (Instruction.Create (OpCodes.Ret));
		}

		public void MethodEnd ()
		{
			CurrentMethods.Pop ();
			CurrentILProcessors.Pop ();
			CurrentMethodReturn.Pop ();
		}

		public Stack<ILProcessor> CurrentILProcessors { get; private set; } = new Stack<ILProcessor> ();
		public Stack<MethodDefinition> CurrentMethods { get; private set; } = new Stack<MethodDefinition> ();
		public Stack<Instruction> CurrentMethodReturn { get; private set; } = new Stack<Instruction> ();

	}
}

