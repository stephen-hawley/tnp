using System;

namespace TNPSupport.AbstractSyntax
{
	[NodeIsA(NodeClass.Statement)]
	public class PrintNode : IASTNode
	{
		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public string Name => "Print";

		public IEnumerable<IASTNode> Children => throw new NotImplementedException();

		public IList<Binding> Bindings => throw new NotImplementedException();

		public TNPType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		[NodeWantsA (NodeClass.StrongTyped, ofType: "System.String")]
		public IASTNode Value { get; set; } = EmptyNode.Empty;

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			if (oldChild == Value) {
				oldChild = newChild;
				newChild.Parent = this;
			}
		}
	}
}

