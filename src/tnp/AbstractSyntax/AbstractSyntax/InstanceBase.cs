using System;
using System.Text;

namespace TNPSupport.AbstractSyntax
{
	public abstract class InstanceBase : NominalBase
	{
		public InstanceBase ()
		{
		}

		// TODO: these need to be stronly typed
		public IList<IASTNode> Fields { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Properties { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Indexers { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Constructors { get; private set; } = new List<IASTNode> ();
		public IList<MethodNode> Methods { get; private set; } = new List<MethodNode> ();
		public IList<IASTNode> Operators { get; private set; } = new List<IASTNode> ();

		public override IEnumerable<IASTNode> Children {
			get {
				foreach (var f in Fields)
					yield return f;
				foreach (var p in Properties)
					yield return p;
				foreach (var i in Indexers)
					yield return i;
				foreach (var c in Constructors)
					yield return c;
				foreach (var m in Methods)
					yield return m;
				foreach (var o in Operators)
					yield return o;
			}
		}

		public override void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			if (ReplaceChildInSet (Fields, oldChild, newChild))
				return;
			if (ReplaceChildInSet (Properties, oldChild, newChild))
				return;
			if (ReplaceChildInSet (Indexers, oldChild, newChild))
				return;
			if (ReplaceChildInSet (Constructors, oldChild, newChild))
				return;
			if (ReplaceChildInSet (Methods, oldChild, newChild))
				return;
			if (ReplaceChildInSet (Operators, oldChild, newChild))
				return;
		}

		bool ReplaceChildInSet <T> (IList<T> l, IASTNode oldChild, IASTNode newChild) where T:IASTNode
		{
			for (var i = 0; i < l.Count; i++) {
				if ((IASTNode)l [i] == oldChild) {
					l [i] = (T)newChild;
					return true;
				}
			}
			return false;
		}
	}
}

