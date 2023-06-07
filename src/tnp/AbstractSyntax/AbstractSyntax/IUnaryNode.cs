using System;

namespace TNPSupport.AbstractSyntax
{
	public interface IUnaryNode : IASTNode
	{
		public IASTNode Child { get; }
	}

	[NodeIsA (NodeClass.Expression)]
	public class UnaryNode : IUnaryNode {
		public UnaryNode (IASTNode child, IASTNode? parent = null, TNPType? type = null)
		{
			Child = child;
			Child.Parent = this;
			Parent = parent is null ? EmptyNode.Empty : parent;
			Type = type is null ? TNPTypeFactory.NoType : type;
		}

		[NodeWantsA (NodeClass.Expression)]
		public IASTNode Child { get; set; }

		public IASTNode Parent { get; set; }

		public string Name => throw new NotImplementedException ();

		public IEnumerable<IASTNode> Children {
			get {
				yield return Child;
			}
		}

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; }

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			if (oldChild == Child) {
				newChild.Parent = this;
				Child = newChild;
			}
		}
	}
}

