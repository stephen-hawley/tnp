using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using TNPSupport.AbstractSyntax;
using TNPSupport.GeneratedParser;
using static TNPSupport.GeneratedParser.TnpExpressionsParser;

using NN = Antlr4.Runtime.Misc.NotNullAttribute;

namespace TNPSupport.Expressions
{
	public class ExpressionParser : TnpExpressionsBaseListener {
		Stack<IASTNode> nodes = new Stack<IASTNode> ();
		public ExpressionParser ()
		{
		}

		public IASTNode? Parse (string text)
		{
			var charStm = CharStreams.fromString (text);
			var lexer = new TnpExpressionsLexer (charStm);
			var tokenStm = new CommonTokenStream (lexer);
			var parser = new TnpExpressionsParser (tokenStm);
			var walker = new ParseTreeWalker ();
			try {
				// todo - better (any) error handling
				walker.Walk (this, parser.tnp_expression ());
				if (nodes.Count () == 1) {
					return nodes.Pop ();
				} else {
				}
				return null;
			} catch (Exception e) {
				return null;
			}
		}
	

		public override void ExitString([NN] TnpExpressionsParser.StringContext context)
		{
			base.ExitString(context);
			var text = context.GetText ();
			// drop the quotes
			text = text.Substring (1, text.Length - 2);
			// todo - handle escape codes
			nodes.Push (new ConstantString (text));
		}

		public override void ExitNumber([NN] TnpExpressionsParser.NumberContext context)
		{
			base.ExitNumber(context);
			var text = context.GetText ().Replace ("_", "", StringComparison.InvariantCultureIgnoreCase);
			if (context.FLOAT_LITERAL () is not null) {
				var isFloat = text.EndsWith ('f') || text.EndsWith ('F');
				if (isFloat || text.EndsWith ('d') || text.EndsWith ('D')) {
					text = text.Substring (0, text.Length - 1);
				}
				if (isFloat) {
					if (Single.TryParse (text, CultureInfo.InvariantCulture, out var f)) {
						nodes.Push (new ConstantSingle (f));
					}
				} else {
					if (Double.TryParse (text, out var d)) {
						nodes.Push (new ConstantDouble (d));
					}
				}
			} else if (context.HEX_LITERAL () is not null) {
				var isLong = text.EndsWith ('l') || text.EndsWith ('L');
				if (isLong) {
					nodes.Push (new ConstantLong (Convert.ToInt64 (text, 16)));
				} else {
					nodes.Push (new ConstantInt (Convert.ToInt32 (text, 16)));
				}

			} else if (context.DECIMAL_LITERAL () is not null) {
				var isLong = text.EndsWith ('l') || text.EndsWith ('L');
				if (isLong) {
					text = text.Substring (0, text.Length - 1);
					if (Int64.TryParse (text, CultureInfo.InvariantCulture, out var l)) {
						nodes.Push (new ConstantLong (l));
					}
				} else {
					if (Int32.TryParse (text, CultureInfo.InvariantCulture, out var i)) {
						nodes.Push (new ConstantInt (i));
					}
				}
			}
		}

		public override void ExitBoolean([NN] TnpExpressionsParser.BooleanContext context)
		{
			base.ExitBoolean(context);
			nodes.Push (new ConstantBool (context.GetText () == "true"));
		}

		public override void ExitPrefix([NN] TnpExpressionsParser.PrefixContext context)
		{
			base.ExitPrefix(context);
			var op = context.prefix.Text;
			if (IsUnarySimplifiable (op)) {
				// turns +number or -number into the actual number instead of a runtime op
				NegOrPos (op);
			} else {
				var child = nodes.Pop ()!;
				var unary = new UnaryNode (child);
				nodes.Push (unary);
			}

		}

		void BinaryOp (string op)
		{
			var right = nodes.Pop ()!;
			var left = nodes.Pop ()!;
			var add = new BinaryNode (op, left, right);
			nodes.Push (add);
		}

		public override void ExitAdd([NN] TnpExpressionsParser.AddContext context)
		{
			base.ExitAdd (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitMult([NN] TnpExpressionsParser.MultContext context)
		{
			base.ExitMult (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitAnd([NN] TnpExpressionsParser.AndContext context)
		{
			base.ExitAnd (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitBitAnd([NN] BitAndContext context)
		{
			base.ExitBitAnd (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitOr ([NN] OrContext context)
		{
			base.ExitOr (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitBitOr([NN] BitOrContext context)
		{
			base.ExitBitOr (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitXor([NN] XorContext context)
		{
			base.ExitXor (context);
			BinaryOp (context.binaryop.Text);
		}

		public override void ExitShift([NN] ShiftContext context)
		{
			base.ExitShift(context);
			BinaryOp ("<<");
		}

		bool IsUnarySimplifiable (string op)
		{
			return (op == "+" || op == "-") && TopIsNumber ();
		}

		bool TopIsNumber ()
		{
			if (nodes.Count () == 0)
				return false;
			var top = nodes.Peek ();
			return top is ConstantInt ||
				top is ConstantUInt ||
				top is ConstantLong ||
				top is ConstantULong ||
				top is ConstantByte ||
				top is ConstantSByte ||
				top is ConstantShort ||
				top is ConstantUShort ||
				top is ConstantSingle ||
				top is ConstantDouble;
		}

		void NegOrPos (string op)
		{
			var isPos = op == "+";
			IASTNode? newNode = null;
			var top = nodes.Peek ();
			if (top is ConstantInt i) {
				newNode = isPos ? new ConstantInt (+i.Value) : new ConstantInt (-i.Value);
			} else if (top is ConstantUInt u) {
				newNode = isPos ? top : new ConstantInt (-(int)u.Value);
			} else if (top is ConstantLong l) {
				newNode = isPos ? new ConstantLong (+l.Value) : new ConstantLong (-l.Value);
			} else if (top is ConstantULong ul) {
				newNode = isPos ? top : new ConstantLong (-(long)ul.Value);
			} else if (top is ConstantByte b) {
				newNode = isPos ? top : new ConstantSByte ((sbyte)-b.Value);
			} else if (top is ConstantSByte sb) {
				newNode = isPos ? new ConstantSByte ((sbyte)+sb.Value) : new ConstantSByte ((sbyte)-sb.Value);
			} else if (top is ConstantShort s) {
				newNode = isPos ? new ConstantShort ((short)+s.Value) : new ConstantShort ((short)-s.Value);
			} else if (top is ConstantUShort us) {
				newNode = isPos ? top : new ConstantShort ((short)-us.Value);
			} else if (top is ConstantSingle f) {
				newNode = isPos ? new ConstantSingle (+f.Value) : new ConstantSingle (-f.Value);
			} else if (top is ConstantDouble d) {
				newNode = isPos ? new ConstantDouble (+d.Value) : new ConstantDouble (-d.Value);
			}
			if (newNode is not null) {
				nodes.Pop ();
				nodes.Push (newNode);
			}
		}


	}
}

