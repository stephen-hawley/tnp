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

namespace tnp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
	public MainPageViewModel()
	{
	}

	[RelayCommand]
	void AddNode()
	{
		nodes.Add(new Node(NodeType.HelloWorld));
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

	[RelayCommand]
	void RemoveNode(object sender)
	{
		if (sender is Node node)
			nodes.Remove(node);
	}

	[RelayCommand]
	async Task RunPressed()
	{

		var tempDirectory = Directory.CreateTempSubdirectory();
		Directory.CreateDirectory(tempDirectory.FullName);

		await CompileNodes(tempDirectory);

		// TODO clean up the exe file and temp directory
		var output = Path.Combine(tempDirectory.FullName, "testFunc.exe");

		StartProcessAsync(output);

	}

	async Task CompileNodes(DirectoryInfo path)
	{
		//var helloNode = new HelloWorldNode();

		// all this is for making the root node
		var helloNode = new TopLevelNode() { NameSpace = "NoName" };
		var fooClass = new ClassNode() { TypeName = "Foo" };
		helloNode.TypeNodes.Add(fooClass);
		fooClass.Parent = helloNode;

		// default ctor
		var ctor = new MethodNode() { MethodName = ".ctor" };
		fooClass.Methods.Add(ctor);
		ctor.Parent = fooClass;

		// main method
		var main = new MethodNode() { MethodName = "Main" };
		fooClass.Methods.Add(main);
		main.Parent = fooClass;

		// this is the printer
		var strNode = new ConstantString("Hello, world!");
		var printer = new PrintLineNode() { Value = strNode };
		strNode.Parent = printer;
		main.Statements.Add(printer);
		printer.Parent = main;

		var entry = helloNode.MethodsByName("NoName.Foo", "Main").FirstOrDefault();
		if (entry is not null)
			helloNode.EntryPoint = entry;


		var generators = new CodeGeneratorsIL();

		generators.Begin("testFunc", path.FullName);

		// TODO go through the nodes and convert to TNP.Nodes and run the generator on them

		if (generators.TryGetGenerator(helloNode, out var codeGenerator))
		{
			await codeGenerator.Generate(generators, helloNode);
		}

		generators.End();
	}

	void StartProcessAsync(string path)
	{
		try
		{
			// TODO not need to hardcode path to mono
			var psi = new ProcessStartInfo { FileName = "/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono",
				Arguments = path, UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true};
			// TODO see which environment variables are causing the issue
			psi.EnvironmentVariables.Clear();
			var process = Process.Start(psi);

			var o = process.StandardOutput;
			var text = o.ReadToEnd();

			o.Close();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error {ex.Message}");
		}
	}

	public ObservableCollection<Node> nodes { get; set; } = new ObservableCollection<Node>()
	{
		new Node(NodeType.TopLevel),
		new Node(NodeType.Class),
		new Node(NodeType.Method),
		new Node(NodeType.Method),
		new Node(NodeType.PrintLine),
		new Node(NodeType.ConstantString),

	};

	public class CustomViewCell : ViewCell
	{
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var node = (Node)BindingContext;

			View = GetCustomView(node);
		}

		private View GetCustomView(Node node)
		{
			switch (node.Type)
			{
				case NodeType.EmptyNode:
					return new EmptyNodeView(node);
				case NodeType.HelloWorld:
					return new HelloWorldNodeView(node);
				default:
					return new EmptyNodeView(node);
			}
		}
	}
}

public class NodeTemplateSelector : DataTemplateSelector
{
	public DataTemplate HelloWorldNodeTemplate { get; set; }
	public DataTemplate EmptyNodeTemplate { get; set; }
	public DataTemplate TopLevelNodeTemplate { get; set; }
	public DataTemplate ClassNodeTemplate { get; set; }
	public DataTemplate MethodNodeTemplate { get; set; }
	public DataTemplate PrintLineNodeTemplate { get; set; }
	public DataTemplate ConstantStringTemplate { get; set; }

	protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
	{
		var node = (Node)item;
		switch (node.Type)
		{
			case NodeType.EmptyNode:
				return EmptyNodeTemplate;
			case NodeType.HelloWorld:
				return HelloWorldNodeTemplate;
			case NodeType.TopLevel:
				return TopLevelNodeTemplate;
			case NodeType.Class:
				return ClassNodeTemplate;
			case NodeType.Method:
				return MethodNodeTemplate;
			case NodeType.PrintLine:
				return PrintLineNodeTemplate;
			case NodeType.ConstantString:
				return ConstantStringTemplate;
			default:
				throw new Exception($"Template Selector did not find Node type {node.Type}");
		}
	}
}

public class Node
{
	public string Name { get; set; }
	public string NameSpace { get; set; }
	public string TypeName { get; set; }
	public string MethodName { get; set; }
	public string Value { get; set; }
	public string PrintString { get; set; } = "Hello World!";
	public NodeType Type { get; set; }

	public Node(NodeType type)
	{
		Type = type;
	}
}

public enum NodeType
{
	EmptyNode,
	HelloWorld,
	TopLevel,
	Class,
	Method,
	PrintLine,
	ConstantString,
}
