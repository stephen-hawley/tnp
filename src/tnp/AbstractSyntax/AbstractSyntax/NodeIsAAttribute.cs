using System;
namespace TNPSupport.AbstractSyntax
{
	[AttributeUsage (AttributeTargets.Class)]
	public class NodeIsAAttribute : Attribute
	{
		public NodeClass NodeClass { get; private set; }
		public NodeIsAAttribute (NodeClass nodeClass)
		{
			NodeClass = nodeClass;
		}
	}
}

