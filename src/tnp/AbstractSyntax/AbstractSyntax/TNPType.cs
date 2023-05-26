using System;
namespace TNPSupport.AbstractSyntax
{
	public class TNPType
	{
		public TNPType(string nameSpace, string name, string alias = "")
		{
			NameSpace = nameSpace;
			Name = name;
			Alias = alias;
		}

		public string NameSpace { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public bool HasAlias => !string.IsNullOrEmpty (Alias);
		public string FullName => $"{NameSpace}.{Name}";
		public override string ToString () => HasAlias ? Alias : FullName;
	}
}

