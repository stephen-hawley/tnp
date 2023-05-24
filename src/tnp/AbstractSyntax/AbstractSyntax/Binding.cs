using System;
namespace TNPSupport.AbstractSyntax
{
	public class Binding
	{
		public Binding(string name, TNPType type)
		{
			Name = name;
			Type = type;
		}

		public string Name { get; set; }
		public TNPType Type { get; set; }
	}
}

