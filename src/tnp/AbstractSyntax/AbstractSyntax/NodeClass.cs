using System;
namespace TNPSupport.AbstractSyntax
{
	[Flags]
	public enum NodeClass
	{
		TopLevel = 1 << 0,
		Type = 1 << 1,
		Block = 1 << 2,
		Statement = 1 << 3,
		Expression = 1 << 4,
		StrongTyped = 1 << 5,
	}
}

