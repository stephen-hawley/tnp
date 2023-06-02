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
		try
		{
			var startTime = System.Environment.TickCount;
			var helloNode = new HelloWorldNode();

			var generators = new CodeGeneratorsIL();

			var t = Directory.GetCurrentDirectory();
			var t1 = Path.GetTempPath();
			var tempDirectory = Directory.CreateTempSubdirectory();
			string tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

			//tempDirectory = Directory.CreateDirectory(Path.Combine(Directory.GetParent(t).FullName, "TempDir"));
			tempDirectory = Directory.CreateDirectory(Path.Combine("/Users/tjlambert/Desktop/", "TempDir"));

			Directory.CreateDirectory(tempDirectory.FullName);

			generators.Begin("testFunc", tempDirectory.FullName);

			// TODO go through the nodes and convert to TNP.Nodes and run the generator on them

			if (generators.TryGetGenerator(helloNode, out var codeGenerator))
			{
				codeGenerator.Generate(generators, helloNode).Wait();
				//taskg.RunSynchronously();
				//taskg.Wait();
			}

			generators.End();
			var endTime = System.Environment.TickCount;

			var output = Path.Combine(tempDirectory.FullName, "testFunc.exe");

			var task = Xamarin.Utils.Execution.RunAsync("/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono", new List<string>() { output }, null, null, null, null, null, null, null);
			//var task = await Xamarin.Utils.Execution.RunAsync($"mono {output}", new List<string>() { }, null, null, null, null, Directory.GetParent(output).FullName, null, null);
			//task.RunSynchronously();
			task.Wait();
			if (task.Result.ExitCode == 0)
			{
				var p = task.Result.StandardOutput?.ToString() ?? "no output";
				Console.WriteLine(task.Result.StandardOutput?.ToString() ?? "no output");
			}
			else
			{
				var p = task.Result.StandardOutput?.ToString() ?? "no output";
				Console.WriteLine(task.Result.StandardError?.ToString() ?? "no error output");
			}
		}
		catch (Exception e)
		{

		}
	}

	[RelayCommand]
	void AddNode()
	{
		nodes.Add(new Node() { Type = NodeType.HelloWorld });
	}

	[RelayCommand]
	async Task RunPressed()
	{
		var startTime = System.Environment.TickCount;
		var helloNode = new HelloWorldNode();

		var generators = new CodeGeneratorsIL();

		var t = Directory.GetCurrentDirectory();
		var t1 = Path.GetTempPath();
		var tempDirectory = Directory.CreateTempSubdirectory();
		string tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

		//tempDirectory = Directory.CreateDirectory(Path.Combine(Directory.GetParent(t).FullName, "TempDir"));
		tempDirectory = Directory.CreateDirectory(Path.Combine("/Users/tjlambert/Desktop/", "TempDir"));

		Directory.CreateDirectory(tempDirectory.FullName);

		generators.Begin("testFunc", tempDirectory.FullName);

		// TODO go through the nodes and convert to TNP.Nodes and run the generator on them

		if (generators.TryGetGenerator(helloNode, out var codeGenerator))
		{
			await codeGenerator.Generate(generators, helloNode);
		}

		generators.End();
		var endTime = System.Environment.TickCount;

		var output = Path.Combine(tempDirectory.FullName, "testFunc.exe");



		await StartProcessAsync(output);



		// Create a new process instance
		//Process process = new Process();

		//// Set the process start information
		//process.StartInfo.FileName = output; // Specify the path to the executable you want to run
		//									 //process.StartInfo.Arguments = "optional_arguments"; // Specify any command-line arguments if required

		//// Start the process
		//process.Start();

		//// Wait for the process to exit
		//process.WaitForExit();

		//// Get the exit code of the process
		//int exitCode = process.ExitCode;

		//// Output the exit code
		//Console.WriteLine("Exit Code: " + exitCode);


		//try
		//{

		//	var task = await Xamarin.Utils.Execution.RunAsync("/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono", new List<string>() { output }, null, null, null, null, null, null, null);
		//	//var task = await Xamarin.Utils.Execution.RunAsync($"mono {output}", new List<string>() { }, null, null, null, null, Directory.GetParent(output).FullName, null, null);
		//	//task.RunSynchronously();
		//	//task.Wait();
		//	if (task.ExitCode == 0)
		//	{
		//		var p = task.StandardOutput?.ToString() ?? "no output";
		//		Console.WriteLine(task.StandardOutput?.ToString() ?? "no output");
		//		Console.WriteLine($"Compilation time: {endTime - startTime}ms");
		//	}
		//	else
		//	{
		//		var p = task.StandardOutput?.ToString() ?? "no output";
		//		Console.WriteLine(task.StandardError?.ToString() ?? "no error output");
		//	}
		//}
		//catch (Exception e)
		//{

		//}



		//var task = await Xamarin.Utils.Execution.RunAsync("mono", new List<string>() { output }, null, null, null, null, null, null, null);
		//if (task.ExitCode == 0)
		//{
		//	Console.WriteLine(task.StandardOutput?.ToString() ?? "no output");
		//	Console.WriteLine($"Compilation time: {endTime - startTime}ms");
		//}
		//else
		//{
		//	Console.WriteLine(task.StandardError?.ToString() ?? "no error output");
		//}
	}

	async Task StartProcessAsync(string path)
	{
		try
		{
			//var psi = new ProcessStartInfo { FileName = "mono.exe", Arguments = path, UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true };
			//var psi = new ProcessStartInfo { FileName = "/Users/tjlambert/Desktop/TempDir/testFunc.exe", Arguments = path, UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true };
			var psi = new ProcessStartInfo { FileName = "/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono", Arguments = path, UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true };
			var process = Process.Start(psi);

			//Process.Start("bash", "-c \"echo Hello, TJ!\"");
			//Process.Start("bash", $"-c \"/Library/Frameworks/Mono.framework/Versions/Current/Commands/mono {path}\"");
			////Process.Start("bash", "echo 'Hello, world!'");


			////string command = "mono";
			////string arguments = "/Users/tjlambert/Desktop/TempDir/testFunc.exe";

			////Directory.SetCurrentDirectory(Directory.GetParent(path).FullName);

			////ProcessStartInfo startInfo = new ProcessStartInfo(command, arguments);
			////Process.Start(startInfo);

			//// Create a new process instance
			//Process process = new Process();

			//// Set the process start information
			//process.StartInfo.FileName = $"mono {path}"; // Specify the path to the executable you want to run
			////process.StartInfo.FileName = "cmd.exe"; // Specify the path to the executable you want to run
			////process.StartInfo.UseShellExecute = false; // Specify any command-line arguments if required
			////process.StartInfo.Arguments = $"/C mono {path}"; // Specify any command-line arguments if required

			////// Start the process
			//process.Start();
			////process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });


			//// Wait asynchronously for the process to exit
			//await Task.Run(() => process.WaitForExit());

			//// Get the exit code of the process
			//int exitCode = process.ExitCode;

			//// Output the exit code
			//Console.WriteLine($"Process Completed. Exit Code: {exitCode}");

			var o = process.StandardOutput;
			var text = o.ReadToEnd();

			o.Close();
		}
		catch (Exception ex)
		{
			// Handle any exceptions that occurred during process execution
			Console.WriteLine($"Error {ex.Message}");
		}
		//	ex	{System.ComponentModel.Win32Exception (13): An error occurred trying to start process '/var/folders/nj/446lm_hs4zz72gfhvwglxzvr0000gn/T/0HHzBc/testFunc.exe' with working directory '/Users/tjlambert/Microsoft/TNP/tnp/src/tnp/tnp/bin/Debug/net7.0-maccatalyst/macc…}	System.ComponentModel.Win32Exception
		// System.ComponentModel.Win32Exception (13): An error occurred trying to start process with working directory 
	}

	public static ObservableCollection<Node> nodes = new ObservableCollection<Node>()
	{
		new Node
		{
			Type = NodeType.EmptyNode,
		},

		new Node
		{
			Type = NodeType.HelloWorld,
		},
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

public class Node
{
	public string Name { get; set; }
	public string PrintString { get; set; }
	public NodeType Type { get; set; }
}

public enum NodeType
{
	EmptyNode,
	HelloWorld,
}
