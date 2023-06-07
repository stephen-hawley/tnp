using System;

namespace TNPSupport.AbstractSyntax
{
	public class ConstantNode<T> : IASTNode, IExprNode {
		public ConstantNode (T value)
		{
			Value = value;
			Type = TNPTypeFactory.FromType (typeof (T));
		}

		public T Value { get; private set; }

		public IASTNode Parent { get; set; } = EmptyNode.Empty;

		public string Name => $"Constant{typeof (T).Name}";

		public IEnumerable<IASTNode> Children => Enumerable.Empty<IASTNode> ();

		public IList<Binding> Bindings => Binding.EmptyReadOnly;

		public TNPType Type { get; set; }

		public void ReplaceChild(IASTNode oldChild, IASTNode newChild)
		{
			throw new NotImplementedException();
		}

		public bool IsConstant => true;

		public override string ToString()
		{
			return $"{Type} {Value}";
		}
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantString : ConstantNode<string>
	{
		public ConstantString (string value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantBool : ConstantNode<bool>
	{
		public ConstantBool (bool value)
			: base (value) { }
		static ConstantBool trueVal = new ConstantBool (true);
		public static ConstantBool True => trueVal;
		static ConstantBool falseVal = new ConstantBool (true);
		public static ConstantBool False => falseVal;
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantByte : ConstantNode<byte> {
		public ConstantByte (byte value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantSByte : ConstantNode<sbyte> {
		public ConstantSByte (sbyte value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantShort : ConstantNode<short> {
		public ConstantShort (short value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantUShort : ConstantNode<ushort> {
		public ConstantUShort (ushort value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantInt : ConstantNode<int> {
		public ConstantInt (int value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantUInt : ConstantNode<uint> {
		public ConstantUInt (uint value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantLong : ConstantNode<long> {
		public ConstantLong (long value)
			: base (value) { }
	}

	[NodeIsA (NodeClass.Expression)]
	public class ConstantULong : ConstantNode<ulong> {
		public ConstantULong (ulong value)
			: base (value) { }
	}

	public class ConstantSingle : ConstantNode<float>
	{
		public ConstantSingle (float value)
			: base (value) { }
	}

	public class ConstantDouble : ConstantNode<double>
	{
		public ConstantDouble (double value)
			: base (value) { }
	}
}

