using System;
using tnp.ViewModels;

namespace tnp.Models;

public class Node
{
	public string Name { get; set; }
	public string NameSpace { get; set; }
	public string TypeName { get; set; }
	public string MethodName { get; set; }
	public string Value { get; set; }
	public string PrintString { get; set; }
	public NodeType Type { get; set; }

	// TODO we only want one method to be able to have this set
	public bool IsEntryPoint { get; set; }

	public Node(NodeType type)
	{
		Type = type;
	}
}
