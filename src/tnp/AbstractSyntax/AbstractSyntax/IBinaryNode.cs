using System;
namespace TNPSupport.AbstractSyntax
{
	public interface IBinaryNode : IASTNode, IExprNode
	{
		IASTNode Left { get; set; }
		IASTNode Right { get; set; }
	}

	public class BinaryNode : IBinaryNode
	{
		protected BinaryNode(string name, IASTNode parent, IASTNode left, IASTNode right, TNPType type)
		{
			Name = name;
			Parent = parent;
			Left = left;
			Right = right;
			Type = type;
		}

		public string Name { get; private set; }
		public virtual IASTNode Left { get; set; }
		public virtual IASTNode Right { get; set; }
		public IASTNode Parent { get; set; }
		public TNPType Type { get; set; }
		public IList<Binding> Bindings => Binding.EmptyReadOnly;
		public IEnumerable<IASTNode> Children {
			get {
				yield return Left;
				yield return Right;
			}
		}

		public bool IsConstant {
			get {
				return Left is IExprNode lex && lex.IsConstant &&
					Right is IExprNode rex && rex.IsConstant;
			}
		}
		
		public void ReplaceChild (IASTNode oldNode, IASTNode newNode)
		{
			if (oldNode == Left) {
				Left = newNode;
				newNode.Parent = this;
			} else if (oldNode == Right) {
				Right = newNode;
				newNode.Parent = this;
			} else {
				throw new ArgumentException ($"node {oldNode} is neither Left nor Right", nameof (oldNode));
			}
		}

		public override string ToString ()
		{
			return $"{Left} {Name} {Right}";
		}
	}
}

