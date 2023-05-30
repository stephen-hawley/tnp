using System;
using System.Text;

namespace TNPSupport.AbstractSyntax
{
	public abstract class NominalBase : IASTNode
	{
		public NominalBase()
		{
		}

		// TODO: these need to be stronly typed
		public IList<IASTNode> Fields { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Properties { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Indexers { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Constructors { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Methods { get; private set; } = new List<IASTNode> ();
		public IList<IASTNode> Operators { get; private set; } = new List<IASTNode> ();

		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public abstract string Name { get; }

		string typeName = "";
		public string TypeName {
			get => typeName;
			set {
				ChangeType (typeName); 
			}
		}

		public Visibility Visibility { get; set; }

		public virtual IEnumerable<IASTNode> Children {
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

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; } = TNPTypeFactory.Void;

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
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

		bool ReplaceChildInSet (IList<IASTNode> l, IASTNode oldChild, IASTNode newChild)
		{
			for (var i = 0; i < l.Count; i++) {
				if (l [i] == oldChild) {
					l [i] = newChild;
					return true;
				}
			}
			return false;
		}

		void ChangeType (string newName)
		{
			TNPTypeFactory.TryRemove (FullName, out var oldType);
			var sb = new StringBuilder ();
			sb.Append (Namespace);
			if (sb.Length > 0)
				sb.Append ('.');
			sb.Append (newName);
			fullName = sb.ToString ();
			Type = TNPTypeFactory.FromTypeName (Namespace, newName);
			typeName = newName;
		}

		string fullName = ""; 

		public string FullName => fullName;

		string Namespace {
			get {
				var parent = Parent;
				while (parent != EmptyNode.Empty) {
					if (parent is TopLevelNode tl) {
						return tl.NameSpace;
					}
				}
				return "";
			}
		}
	}
}

