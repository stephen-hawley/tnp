using System;

namespace TNPSupport.AbstractSyntax
{
	public class ConstantNode<T> : IASTNode {
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
	}

	public class ConstantString : ConstantNode<string>
	{
		public ConstantString (string value)
			: base (value) { }
	}

	public class ConstantBool : ConstantNode<bool>
	{
		public ConstantBool (bool value)
			: base (value) { }
		static ConstantBool trueVal = new ConstantBool (true);
		public static ConstantBool True => trueVal;
		static ConstantBool falseVal = new ConstantBool (true);
		public static ConstantBool False => falseVal;
	}

	public class ConstantByte : ConstantNode<byte> {
		public ConstantByte (byte value)
			: base (value) { }
	}

	public class ConstantSByte : ConstantNode<sbyte> {
		public ConstantSByte (sbyte value)
			: base (value) { }
	}

	public class ConstantShort : ConstantNode<short> {
		public ConstantShort (short value)
			: base (value) { }
	}

	public class ConstantUShort : ConstantNode<ushort> {
		public ConstantUShort (ushort value)
			: base (value) { }
	}

	public class ConstantInt : ConstantNode<int> {
		public ConstantInt (int value)
			: base (value) { }
	}

	public class ConstantUInt : ConstantNode<uint> {
		public ConstantUInt (uint value)
			: base (value) { }
	}

	public class ConstantLong : ConstantNode<long> {
		public ConstantLong (long value)
			: base (value) { }
	}

	public class ConstantULong : ConstantNode<ulong> {
		public ConstantULong (ulong value)
			: base (value) { }
	}
}

