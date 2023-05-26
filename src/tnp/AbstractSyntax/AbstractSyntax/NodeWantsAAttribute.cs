using System;
namespace TNPSupport.AbstractSyntax
{
	[AttributeUsage (AttributeTargets.Property)]
	public class NodeWantsAAttribute : Attribute
	{
		public NodeClass NodeClass { get; private set; }
		public NodeWantsAAttribute(NodeClass nodeClass)
		{
			NodeClass = nodeClass;
		}
	}
}

