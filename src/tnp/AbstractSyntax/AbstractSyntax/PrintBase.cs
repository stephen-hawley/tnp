using System;
namespace TNPSupport.AbstractSyntax
{
	public abstract class PrintBase : IASTStatement
	{
		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public abstract string Name { get; }

		public IEnumerable<IASTNode> Children => Enumerable.Empty<IASTNode> ();

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get => TNPTypeFactory.Void; set => throw new NotImplementedException (); }

		[NodeWantsA (NodeClass.StrongTyped, ofType: "System.String")]
		public IASTNode Value { get; set; } = EmptyNode.Empty;

		public abstract bool IncludeNewline { get; }

		public void ReplaceChild (IASTNode oldChild, IASTNode newChild)
		{
			if (oldChild == Value) {
				Value = newChild;
				newChild.Parent = this;
			}
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}

