using OneOf;
using Saikoro.Types.Numeric;
using Saikoro.Types.Operator;

namespace Saikoro.Parsing;
public struct NumberOrOperator
{
	private readonly OneOf<Number, ValentOperator> _value;
	public object Value => _value.Value;

	private NumberOrOperator(OneOf<Number, ValentOperator> value) => _value = value;

	// conversions
	public static implicit operator NumberOrOperator(Number value) => new NumberOrOperator(value);
	public static implicit operator NumberOrOperator(ValentOperator value) => new NumberOrOperator(value);

	// type checks
	public bool IsNumber => _value.IsT0;
	public bool IsOperator => _value.IsT1;

	// casts
	public Number AsNumber => _value.AsT0;
	public ValentOperator AsOperator => _value.AsT1;
}
