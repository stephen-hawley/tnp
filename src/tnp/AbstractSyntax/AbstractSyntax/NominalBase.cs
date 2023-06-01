using System;
using System.Text;

namespace TNPSupport.AbstractSyntax
{
	public abstract class NominalBase : IASTNode
	{
		public NominalBase()
		{
		}

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

		public abstract IEnumerable<IASTNode> Children { get; }
		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; } = TNPTypeFactory.Void;

		public abstract void ReplaceChild (IASTNode oldChild, IASTNode newChild);

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

