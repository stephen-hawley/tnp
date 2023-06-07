using TNPSupport.AbstractSyntax;
using TNPSupport.Expressions;

namespace TNPTests;

public class ParserTests
{
	[Test]
	public void ParseSmokeTest ()
	{
		var testExpr = "42";
		var parser = new ExpressionParser ();
		var nodeUntyped = parser.Parse (testExpr);
		Assert.IsNotNull (nodeUntyped);
		var node = nodeUntyped as ConstantInt;
		Assert.IsNotNull (node);
		Assert.That (node!.Value, Is.EqualTo (42));
	}

	[Test]
	public void ParsesLong ()
	{
		var testExpr = "42L";
		var parser = new ExpressionParser ();
		var nodeUntyped = parser.Parse (testExpr);
		Assert.IsNotNull (nodeUntyped);
		var node = nodeUntyped as ConstantLong;
		Assert.IsNotNull (node);
		Assert.That (node!.Value, Is.EqualTo (42));
	}

	[Test]
	public void ParsesFloat ()
	{
		var testExpr = "42.3f";
		var parser = new ExpressionParser ();
		var nodeUntyped = parser.Parse (testExpr);
		Assert.IsNotNull (nodeUntyped);
		var node = nodeUntyped as ConstantSingle;
		Assert.IsNotNull (node);
		Assert.That (node!.Value, Is.EqualTo (42.3f));
	}

	[Test]
	public void ParsesDouble ()
	{
		var testExpr = "42.3";
		var parser = new ExpressionParser ();
		var nodeUntyped = parser.Parse (testExpr);
		Assert.IsNotNull (nodeUntyped);
		var node = nodeUntyped as ConstantDouble;
		Assert.IsNotNull (node);
		Assert.That (node!.Value, Is.EqualTo (42.3));
	}

	[Test]
	public void ParsesString ()
	{
		var testExpr = "\"I like spam\"";
		var parser = new ExpressionParser ();
		var nodeUntyped = parser.Parse (testExpr);
		Assert.IsNotNull (nodeUntyped);
		var node = nodeUntyped as ConstantString;
		Assert.IsNotNull (node);
		Assert.That (node!.Value, Is.EqualTo ("I like spam"));
	}
}
