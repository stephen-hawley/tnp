using System;

namespace TNPSupport.AbstractSyntax
{
	[NodeIsA (NodeClass.TopLevel)]
	public class HelloWorldNode : IASTNode
	{
		public HelloWorldNode()
		{
			var strNode = new ConstantString ("Hello world!");
			var printer = new PrintLineNode ();
			printer.Value = strNode;
			strNode.Parent = printer;
			printer.Parent = this;
			Printer = printer;
		}

		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public string Name => "helloworld";

		public PrintBase Printer { get; private set; }

		public IEnumerable<IASTNode> Children {
			get {
				yield return Printer;
			}
		}

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; } = TNPTypeFactory.Void;

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			if (oldChild == Printer && newChild is PrintNode p) {
				Printer = p;
				p.Parent = this;
			}
		}

		public override string ToString()
		{
			return "HelloWorld";
		}
	}
}

