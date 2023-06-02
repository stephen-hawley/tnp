
using ILCodeGeneration;
using TNPSupport.AbstractSyntax;
using System.IO;

var startTime = System.Environment.TickCount;
var helloNode = new HelloWorldNode ();

var generators = new CodeGeneratorsIL ();

var tempDirectory = Directory.CreateTempSubdirectory ();
Directory.CreateDirectory (tempDirectory.FullName);

generators.Begin ("testFunc", tempDirectory.FullName);

if (generators.TryGetGenerator (helloNode, out var codeGenerator)) {
	await codeGenerator.Generate (generators, helloNode);
}

generators.End ();
var endTime = System.Environment.TickCount;

var output = Path.Combine (tempDirectory.FullName, "testFunc.exe");
var task = Xamarin.Utils.Execution.RunAsync ("mono", new List<string> () { output }, null, null, null, null, null, null, null);
task.Wait ();
if (task.Result.ExitCode == 0) {
	var t = task.Result.StandardOutput?.ToString() ?? "no output";
	Console.WriteLine (task.Result.StandardOutput?.ToString () ?? "no output");
	Console.WriteLine ($"Compilation time: {endTime - startTime}ms");
} else {
	Console.WriteLine (task.Result.StandardError?.ToString () ?? "no error output");
}