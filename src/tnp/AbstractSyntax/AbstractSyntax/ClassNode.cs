using System;
namespace TNPSupport.AbstractSyntax
{
	[NodeIsA (NodeClass.Type)]
	public class ClassNode : InstanceBase {
		public ClassNode ()
		{
		}

		public override string Name => "class";

		[NodeWantsA (NodeClass.Type)]
		public ClassNode? Inheritance { get; set; } = null;
	}
}

