using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using tnp.Models;
using ILCodeGeneration;
using TNPSupport.AbstractSyntax;
using System.IO;
using Xamarin.Utils;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using Mono.Cecil.Cil;
using System.Threading.Channels;

namespace tnp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
	public MainPageViewModel()
	{
	}
	[RelayCommand]
	void AddNode()
	{
		nodes.Add(new Node(NodeType.TopLevel));
	}

	[RelayCommand]
	void AddClassNode(object sender)
	{
		if (sender is Node node)
		{
			var index = nodes.IndexOf(node);
			nodes.Insert(index + 1, new Node(NodeType.Class));
		}
	}

	// TODO await Application.Current.MainPage.DisplayActionSheet() is not the most MVVM way to do it

	[RelayCommand]
	async Task AddTopLevelChild(object sender)
	{
		if (sender is Node node)
		{
			var requestedNode = await Application.Current.MainPage.DisplayActionSheet("Add Child Node", null, null,
				"ClassNode", "OtherNode"
			);

			if (requestedNode == "ClassNode")
			{
				var index = nodes.IndexOf(node);
				nodes.Insert(index + 1, new Node(NodeType.Class));
			}
		}
	}

	[RelayCommand]
	async Task AddClassChild(object sender)
	{
		if (sender is Node node)
		{
			var requestedNode = await Application.Current.MainPage.DisplayActionSheet("Add Child Node", null, null,
				"MethodNode", "OtherNode"
			);

			if (requestedNode == "MethodNode")
			{
				var index = nodes.IndexOf(node);
				nodes.Insert(index + 1, new Node(NodeType.Method));
			}
		}
	}

	[RelayCommand]
	async Task AddPrintChild(object sender)
	{
		if (sender is Node node)
		{
			var requestedNode = await Application.Current.MainPage.DisplayActionSheet("Add Child Node", null, null,
				"ConstantString", "OtherNode"
			);

			if (requestedNode == "ConstantString")
			{
				var index = nodes.IndexOf(node);
				nodes.Insert(index + 1, new Node(NodeType.ConstantString));
			}
		}
	}

	[RelayCommand]
	async Task AddMethodChild(object sender)
	{
		if (sender is Node node)
		{
			var requestedNode = await Application.Current.MainPage.DisplayActionSheet("Add Child Node", null, null,
				"PrintLineNode", "OtherNode"
			);

			if (requestedNode == "PrintLineNode")
			{
				var index = nodes.IndexOf(node);
				nodes.Insert(index + 1, new Node(NodeType.PrintLine));
			}
		}
	}

	[RelayCommand]
	void RemoveNode(object sender)
	{
		if (sender is Node node)
			nodes.Remove(node);
	}

	[ObservableProperty]
	string output;

	[RelayCommand]
	async Task RunPressed()
	{
		var tempDirectory = Directory.CreateTempSubdirectory();
		var tempDirFullName = tempDirectory.FullName;

		await CompileNodes(tempDirectory);

		var exePath = Path.Combine(tempDirFullName, "testFunc.exe");

		// TODO not need to hardcode path to mono
		Output = StartProcess("/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono", exePath);

		if (File.Exists(exePath))
			File.Delete(exePath);

		if (Directory.Exists(tempDirFullName))
			Directory.Delete(tempDirFullName);

		// clean up the TNPTypeFactory Cache between compiles
		TNPTypeFactory.ResetCache();
	}

	async Task CompileNodes(DirectoryInfo path)
	{
		var preparedNodes = PrepareNodes();

		var generators = new CodeGeneratorsIL();

		try
		{
			generators.Begin("testFunc", path.FullName);

			foreach (var node in preparedNodes)
			{
				if (generators.TryGetGenerator(node, out var codeGenerator))
				{
					await codeGenerator.Generate(generators, node);
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error while compiling: {e}");
		}
		finally
		{
			generators.End();
		}
	}

	List<IASTNode> PrepareNodes()
	{
		List<IASTNode> TopLevelNodes = new List<IASTNode>();

		TopLevelNode currentTopLevelNode = null;
		ClassNode currentClassNode = null;
		MethodNode currentMethodNode = null;

		List<TopLevelNode> entryTopLevelNodes = new List<TopLevelNode>();
		List<ClassNode> entryClassNodes = new List<ClassNode>();
		List<MethodNode> entryMethodNodes = new List<MethodNode>();

		IASTNode savedPreviousNode = null;

		foreach (var node in nodes)
		{

			if (node.Type == NodeType.TopLevel)
			{
				// add the old TopLevelNode to the list
				if (currentTopLevelNode is TopLevelNode tln)
				{
					TopLevelNodes.Add(tln);
				}

				currentTopLevelNode = new TopLevelNode()
				{
					NameSpace = node.NameSpace,
				};
			}

			else if (node.Type == NodeType.Class)
			{
				currentClassNode = new ClassNode();

				// Note: we must set Parent before setting TypeName
				currentClassNode.Parent = currentTopLevelNode;
				currentTopLevelNode.TypeNodes.Add(currentClassNode);
				currentClassNode.TypeName = node.TypeName;

			}

			else if (node.Type == NodeType.Method)
			{
				currentMethodNode = new MethodNode
				{
					MethodName = node.MethodName,
				};

				currentClassNode.Methods.Add(currentMethodNode);
				currentMethodNode.Parent = currentClassNode;

				if (node.IsEntryPoint)
				{
					entryTopLevelNodes.Add(currentTopLevelNode);
					entryClassNodes.Add(currentClassNode);
					entryMethodNodes.Add(currentMethodNode);
				}
			}

			else if (node.Type == NodeType.PrintLine)
			{
				var printLineNode = new PrintLineNode();
				currentMethodNode.Statements.Add(printLineNode);
				printLineNode.Parent = currentMethodNode;
				savedPreviousNode = printLineNode;
			}

			else if (node.Type == NodeType.ConstantString)
			{
				var constantString = new ConstantString(node.Value);
				if (savedPreviousNode is PrintLineNode pln)
				{
					pln.Value = constantString;
					constantString.Parent = pln;
					savedPreviousNode = null;
				}
			}
		}

		for (int i = 0; i < entryTopLevelNodes.Count; i++)
		{
			var entryTopLevel = entryTopLevelNodes[i];
			var entry = entryTopLevel.MethodsByName($"{entryTopLevel.NameSpace}.{entryClassNodes[i].TypeName}", entryMethodNodes[i].MethodName).FirstOrDefault();
			if (entry is not null)
				entryTopLevel.EntryPoint = entry;
		}

		if (!TopLevelNodes.Contains(currentTopLevelNode))
			TopLevelNodes.Add(currentTopLevelNode);

		return TopLevelNodes;
	}

	string StartProcess(string filename, string path)
	{
		var output = string.Empty;
		try
		{
			var psi = new ProcessStartInfo
			{
				FileName = filename,
				Arguments = path,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};

			// TODO see which environment variables are causing the issue
			psi.EnvironmentVariables.Clear();
			var process = Process.Start(psi);
			var stream = process.StandardOutput;
			var text = stream.ReadToEnd();
			stream.Close();
			output = string.IsNullOrEmpty(text) ? "No Output" : text;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error {ex.Message}");
		}

		return output;
	}

	public ObservableCollection<Node> nodes { get; set; } = new ObservableCollection<Node>()
	{
		//new Node(NodeType.TopLevel)
		//{
		//	NameSpace = "NameSpace1",
		//},
		//new Node(NodeType.Class)
		//{
		//	TypeName = "Class1",
		//},
		//new Node(NodeType.Method)
		//{
		//	MethodName = ".ctor",
		//},
		//new Node(NodeType.Method)
		//{
		//	MethodName = "Main",
		//	IsEntryPoint = true,
		//},
		//new Node(NodeType.PrintLine),
		//new Node(NodeType.ConstantString)
		//{
		//	Value = "Hello World!",
		//},

	};
}
