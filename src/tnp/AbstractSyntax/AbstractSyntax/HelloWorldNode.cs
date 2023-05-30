using System;

namespace TNPSupport.AbstractSyntax
{
	[NodeIsA (NodeClass.TopLevel)]
	public class HelloWorldNode : IASTNode
	{
		public HelloWorldNode()
		{
		}

		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public string Name => "helloworld";

		public IEnumerable<IASTNode> Children {
			get {
				yield break;
			}
		}

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; } = TNPTypeFactory.Void;

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			throw new NotImplementedException();
		}
	}
}

