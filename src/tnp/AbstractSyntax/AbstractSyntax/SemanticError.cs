using System;
namespace TNPSupport.AbstractSyntax
{
	public class SemanticError
	{
		public SemanticError(string error, IASTNode where)
		{
			Error = error;
			Where = where;
		}

		public string Error { get; }
		public IASTNode Where { get; }
	}
}

