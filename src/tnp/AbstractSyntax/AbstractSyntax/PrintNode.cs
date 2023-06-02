using System;

namespace TNPSupport.AbstractSyntax
{
	[NodeIsA(NodeClass.Statement)]
	public class PrintNode : PrintBase
	{
		public override string Name => "Print";
		public override bool IncludeNewline => false;
	}

	[NodeIsA (NodeClass.Statement)]
	public class PrintLineNode : PrintBase {
		public override string Name => "PrintLine";
		public override bool IncludeNewline => true;
	}

}

