using System;
namespace TNPSupport.AbstractSyntax
{
	public class TNPType
	{
		public TNPType(string nameSpace, string name)
		{
			NameSpace = nameSpace;
			Name = name;
		}

		public string NameSpace { get; set; }
		public string Name { get; set; }
		public string FullName => $"{NameSpace}.{Name}";
		public override string ToString () => FullName;
	}
}

