using Saikoro.Randomization;
using Saikoro.Types.Numeric;

namespace Saikoro.Expressions;
internal sealed class IntermediateValue
{
	private readonly Number? _valueIfNumber;
	private readonly RollResult? _valueIfRoll;

	private IntermediateValue(Number value)
	{
		_valueIfNumber = value;
		_valueIfRoll = null;
	}
	private IntermediateValue(RollResult value)
	{
		_valueIfRoll = value;
		_valueIfNumber = null;
	}

	public static implicit operator IntermediateValue(Number value) => new IntermediateValue(value);
	public static implicit operator IntermediateValue(RollResult value) => new IntermediateValue(value);

	public bool IsNumber => _valueIfNumber is not null;
	public bool IsRollResult => _valueIfRoll is not null;

	public object Value => IsNumber ? AsNumber : AsRollResult;
	public Number AsNumber => _valueIfNumber ?? _valueIfRoll!.Sum();
	public RollResult AsRollResult => _valueIfRoll ?? throw new InvalidCastException($"Cannot convert {typeof(Number)} to {typeof(RollResult)}");
}