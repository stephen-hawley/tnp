using System;

namespace TNPSupport.AbstractSyntax
{
	[NodeIsA(NodeClass.Block)]
	public class MethodNode : IASTNode
	{
		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public string Name => "method";

		public string MethodName { get; set; } = "";

		public IEnumerable<IASTNode> Children => Statements;

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; } = TNPTypeFactory.Void;

		[NodeWantsA (NodeClass.Statement)]
		public IList<IASTStatement> Statements { get; private set; } = new List<IASTStatement> ();

		// todo: add parameters, visibility, and flags

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			if (!(newChild is IASTStatement))
				throw new Exception ($"new child {newChild.GetType ().Name} is not a statement");
			for (int i=0; i < Statements.Count; i++) {
				if (Statements [i] == oldChild) {
					Statements [i] = (IASTStatement)newChild;
					newChild.Parent = this;
				}
			}
		}

		public override string ToString()
		{
			return $"{Type} {MethodName} ()";
		}
	}
}

