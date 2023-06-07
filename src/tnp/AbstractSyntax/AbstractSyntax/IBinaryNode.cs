using System;
namespace TNPSupport.AbstractSyntax
{
	public interface IBinaryNode : IASTNode, IExprNode
	{
		IASTNode Left { get; set; }
		IASTNode Right { get; set; }
	}

	[NodeIsA (NodeClass.Expression)]
	public class BinaryNode : IBinaryNode
	{
		public BinaryNode(string name, IASTNode left, IASTNode right, IASTNode? parent = null, TNPType? type = null)
		{
			Name = name;
			Parent = parent is null ? EmptyNode.Empty : parent;
			Left = left;
			Left.Parent = this;
			Right = right;
			Right.Parent = this;
			Type = type is null ? TNPTypeFactory.NoType : type;
		}

		public string Name { get; private set; }

		[NodeWantsA (NodeClass.Expression)]
		public virtual IASTNode Left { get; set; }

		[NodeWantsA (NodeClass.Expression)]
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

