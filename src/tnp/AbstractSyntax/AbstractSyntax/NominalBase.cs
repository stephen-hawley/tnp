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
				ChangeType (value);
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
			Type = TNPTypeFactory.FromTypeName (Namespace, newName);
			typeName = newName;
		}

		public string FullName => $"{Namespace}.{typeName}";

		public string Namespace {
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

		public override string ToString()
		{
			return FullName;
		}
	}
}

