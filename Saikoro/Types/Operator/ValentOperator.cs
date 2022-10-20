using System.Diagnostics.CodeAnalysis;

namespace Saikoro.Types.Operator;
public readonly struct ValentOperator
{
	public Operator Value { get; }
	public int Valency{ get; }

	public ValentOperator(Operator value, int valency)
	{
		Value = value;
		Valency = valency;
	}

	public override bool Equals([NotNullWhen(true)] object? obj) => obj is ValentOperator op && (Value == op.Value && Valency == op.Valency);
	public override int GetHashCode() => (Value, Valency).GetHashCode();

	public static bool operator ==(ValentOperator left, ValentOperator right) => left.Equals(right);
	public static bool operator !=(ValentOperator left, ValentOperator right) => !(left == right);

	public override string ToString() => $"{Value}_{ValencyString(Valency)}";
	private static string ValencyString(int valency)
	{
		return valency switch
		{
			0 => "nullary",
			1 => "unary",
			2 => "binary",
			3 => "ternary",
			4 => "quaternary",
			_ => $"{valency}-ary"
		};
	}
}