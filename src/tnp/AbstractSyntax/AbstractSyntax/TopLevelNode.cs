using System;

namespace TNPSupport.AbstractSyntax
{
	[NodeIsA (NodeClass.TopLevel)]
	public class TopLevelNode : IASTNode
	{
		public IASTNode Parent { get => EmptyNode.Empty; set => throw new NotImplementedException (); }

		public string Name => "TopLevel";

		public IEnumerable<IASTNode> Children => TypeNodes;

		[NodeWantsA (NodeClass.Type)]
		public IList<IASTNode> TypeNodes { get; } = new List<IASTNode> ();

		// TODO: more than one namespace
		public string NameSpace { get; set; } = string.Empty;

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get => TNPTypeFactory.Void; set => throw new NotImplementedException(); }

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			for (var i = 0; i < TypeNodes.Count (); i++) {
				if (TypeNodes [i] == oldChild) {
					TypeNodes [i] = newChild;
					newChild.Parent = this;
					oldChild.Parent = EmptyNode.Empty;
				}
			}
		}
	}
}

