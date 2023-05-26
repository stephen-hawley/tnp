using System;
using System.Collections.Generic;
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

		static IList<Binding> emptyReadOnly = new List<Binding> ().AsReadOnly ();
		public static IList<Binding> EmptyReadOnly => emptyReadOnly;
	}
}

