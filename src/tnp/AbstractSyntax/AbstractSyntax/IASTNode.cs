using System;
namespace TNPSupport.AbstractSyntax
{
	public interface IASTNode
	{
		IASTNode Parent { get; set; }
		string Name { get; }
		IEnumerable<IASTNode> Children { get; }
		IList<Binding> Bindings { get; }
		TNPType Type { get; set; }
		void ReplaceChild (IASTNode oldChild, IASTNode newChild);
	}
}

