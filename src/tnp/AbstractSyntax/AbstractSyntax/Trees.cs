using System;
using TNPSupport.Semantics;

namespace TNPSupport.AbstractSyntax
{
	public class Trees
	{
		public static IEnumerable <IASTNode> AllNodes (IASTNode node)
		{
			yield return node;
			foreach (var child in node.Children) {
				foreach (var subchild in AllNodes (child)) {
					yield return subchild;
				}
			}
		}

		public static bool WalkTree (IASTNode node, Func<IASTNode, bool> action)
		{
			if (!action (node))
				return false;
			foreach (var child in node.Children) {
				if (!WalkTree (child, action))
					return false;
			}
			return true;
		}

		public static bool WalkTreeNR (IASTNode node, Func<IASTNode, bool> action)
		{
			var stack = new Stack<IASTNode> ();
			stack.Push (node);

			while (stack.Count > 0) {
				var curr = stack.Pop ();
				if (!action (curr))
					return false;
				foreach (var child in curr.Children.Reverse ())
					stack.Push (child);
			}
			return true;
		}

		public static IASTNode? FindNodeWithChild (IASTNode node, IASTNode child)
		{
			IASTNode? found = null;
			WalkTreeNR (node, n => {
				foreach (var localChild in n.Children) {
					if (localChild == child) {
						found = node;
					}
					return false;
				}
				return true;
			});
			return found;
		}

		public static void WriteTreeAsSExpr (IASTNode node)
		{
			WriteTreeAsSExpr (node, Console.Out);
		}

		public static void WriteTreeAsSExpr (IASTNode node, TextWriter writer)
		{
			writer.Write ('(');
			writer.Write (node.GetType ().Name);
			writer.Write (' ');
			writer.Write (node.Name);
			foreach (var binding in node.Bindings) {
				WriteBinding (binding, writer);
			}
			foreach (var child in node.Children) {
				WriteTreeAsSExpr (child, writer);
			}
			writer.Write (')');
		}

		static void WriteBinding (Binding binding, TextWriter writer)
		{
			writer.Write (binding.Name);
			writer.Write (": ");
			writer.Write (binding.Type.FullName);
		}

		// todo: make this async and work get a list of semantics
		// so we can make the rules implementation short
		public static SemanticResult CheckSemantics (ISemanticCheckers environment, IASTNode node, List<SemanticError> errors)
		{
			if (environment.TryGetSemanticChecker (node, out var checker)) {
				return checker.Check (environment, node, errors);
			} else {
				throw new NotImplementedException ($"no semantic checker for {node.Name}");
			}
		}

		public static Tuple<IASTNode, Binding>? FindBindingInAndUp (IASTNode node, string name)
		{
			Tuple<IASTNode, Binding>? result = null;
			while (true) {
				if (node == EmptyNode.Empty)
					break;
				foreach (var binding in node.Bindings) {
					if (binding.Name == name) {
						result = new Tuple<IASTNode, Binding> (node, binding);
						break;
					}
				}
				node = node.Parent;
			}
			return result;
		}
	}
}

