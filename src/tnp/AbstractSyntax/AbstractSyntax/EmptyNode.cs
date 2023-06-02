using System;

namespace TNPSupport.AbstractSyntax
{
	public class EmptyNode : IASTNode
	{
		public IASTNode Parent { get => this; set { } }

		public string Name => "";

		public IEnumerable<IASTNode> Children {
			get {
				yield break;
			}
		}

		public IList<Binding> Bindings => bindings;

		public TNPType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			throw new NotImplementedException();
		}

		static IList<Binding> bindings = new List<Binding> ().AsReadOnly ();

		static EmptyNode emptyNode = new EmptyNode ();

		public static EmptyNode Empty => emptyNode;

		public override string ToString()
		{
			return "<[]>";
		}
	}
}

