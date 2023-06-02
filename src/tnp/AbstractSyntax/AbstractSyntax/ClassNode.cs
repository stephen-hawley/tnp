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
		public TNPType Inheritance { get; set; } = TNPTypeFactory.Object;

		public ClassVariety Variety { get; set; } = ClassVariety.Static;
	}
}

