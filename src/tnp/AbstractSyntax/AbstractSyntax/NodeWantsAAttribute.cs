using System;
namespace TNPSupport.AbstractSyntax
{
	[AttributeUsage (AttributeTargets.Property)]
	public class NodeWantsAAttribute : Attribute
	{
		public NodeClass NodeClass { get; private set; }
		// todo: make a standard type that is no type at all - not part of the
		// type system
		public string? OfType { get; private set; }
		public NodeWantsAAttribute(NodeClass nodeClass, string? ofType = null)
		{
			NodeClass = nodeClass;
			OfType = ofType;
		}
	}
}

