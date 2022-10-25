using System.Diagnostics.CodeAnalysis;

namespace Saikoro.Types.Operator;
public readonly struct Operator
{
	public OperatorValue Value { get; }
	public OperatorPriority Priority { get; }
	public Associativity Associativity { get; }

	public Operator(OperatorValue value)
	{
		Value = value;
		Priority = value switch
		{
			OperatorValue.None => OperatorPriority.None,
			OperatorValue.Plus or OperatorValue.Minus => OperatorPriority.Additive,
			OperatorValue.Multiply or OperatorValue.Divide or OperatorValue.Modulus => OperatorPriority.Multiplicative,
			OperatorValue.Power => OperatorPriority.Exponential,
			OperatorValue.Dice => OperatorPriority.Dice,
			_ => OperatorPriority.Comparison,
		};
		Associativity = value switch
		{
			OperatorValue.Power => Associativity.Right,
			_ => Associativity.Left
		};
	}

	public static implicit operator Operator(OperatorValue value) => new Operator(value);

	public override bool Equals([NotNullWhen(true)] object? obj) => obj is Operator op && (Value == op.Value && Priority == op.Priority);
	public override int GetHashCode() => (Value, Priority).GetHashCode();

	public static bool operator ==(Operator left, Operator right) => left.Equals(right);
	public static bool operator !=(Operator left, Operator right) => !(left == right);

	public override string ToString() => Value.ToString();

	public static Operator FromString(string input) => _operatorMap.GetValueOrDefault(input.ToUpperInvariant(), OperatorValue.None);
	private static readonly Dictionary<string, Operator> _operatorMap = new Dictionary<string, Operator>()
	{
		{ "+", OperatorValue.Plus },
		{ "-", OperatorValue.Minus },

		{ "*", OperatorValue.Multiply },
		{ "/", OperatorValue.Divide },
		{ "%", OperatorValue.Modulus },

		{ "^", OperatorValue.Power },

		{ "D", OperatorValue.Dice },

		{ "=", OperatorValue.Equal },
		{ "!=", OperatorValue.NotEqual },

		{ "<", OperatorValue.LessThan },
		{ ">", OperatorValue.GreaterThan },

		{ "<=", OperatorValue.LessOrEqual },
		{ ">=", OperatorValue.GreaterOrEqual },
	};
}