using System;
namespace TNPSupport.AbstractSyntax
{
	[AttributeUsage (AttributeTargets.Property)]
	public class OfTypeAttribute : Attribute
	{
		public string TypeName { get; private set; }
		public OfTypeAttribute(string typeName)
		{
			TypeName = typeName;
		}
	}
}

