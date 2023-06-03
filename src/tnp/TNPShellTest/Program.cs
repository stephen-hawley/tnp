
using ILCodeGeneration;
using TNPSupport.AbstractSyntax;
using System.IO;

var startTime = System.Environment.TickCount;
//var helloNode = new HelloWorldNode ();

// all this is for making the root node
var helloNode = new TopLevelNode () { NameSpace = "NoName" };
var fooClass = new ClassNode () { TypeName = "Foo" };
helloNode.TypeNodes.Add (fooClass);
fooClass.Parent = helloNode;
var ctor = new MethodNode () { MethodName = ".ctor" };
fooClass.Methods.Add (ctor);
ctor.Parent = fooClass;
var main = new MethodNode () { MethodName = "Main" };
fooClass.Methods.Add (main);
main.Parent = fooClass;

// this is the printer
var strNode = new ConstantString ("Hello, world!");
var printer = new PrintLineNode () { Value = strNode };
strNode.Parent = printer;
main.Statements.Add (printer);
printer.Parent = main;

var entry = helloNode.MethodsByName ("NoName.Foo", "Main").FirstOrDefault ();
if (entry is not null)
	helloNode.EntryPoint = entry;


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

var x = System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
var x1 = System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
var x2 = System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);


if (task.Result.ExitCode == 0) {
	var t = task.Result.StandardOutput?.ToString() ?? "no output";
	Console.WriteLine (task.Result.StandardOutput?.ToString () ?? "no output");
	Console.WriteLine ($"Compilation time: {endTime - startTime}ms");
	Trees.WriteTreeAsSExpr (helloNode);
} else {
	Console.WriteLine (task.Result.StandardError?.ToString () ?? "no error output");
}