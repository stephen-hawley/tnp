using System;
using tnp.Models;

namespace tnp;

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
				throw new NotImplementedException(node.Type.ToString());
		}
	}
}
