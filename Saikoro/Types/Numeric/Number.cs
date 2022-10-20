using System.Diagnostics.CodeAnalysis;

namespace Saikoro.Types.Numeric;
public readonly struct Number : IEquatable<Number>, IComparable<Number>
{
	private readonly Fraction? _valueIfFraction;
	private readonly double? _valueIfDouble;

	private Number(Fraction value)
	{
		_valueIfFraction = value;
		_valueIfDouble = null;
	}
	private Number(double value)
	{
		_valueIfDouble = value;
		_valueIfFraction = null;
	}

	public static implicit operator Number(Fraction value) => new Number(value);
	public static implicit operator Number(double value) => new Number(value);

	public bool IsFraction => _valueIfFraction is not null;

	// Casts
	public Fraction AsFraction => _valueIfFraction ?? throw new InvalidCastException($"Cannot convert {ToString()} to a fraction!");
	public double AsDouble => _valueIfDouble ?? (double)AsFraction;

	// Comparison
	public int CompareTo(Number other)
	{
		return BothAreFractions(this, other) ?
			this.AsFraction.CompareTo(other.AsFraction) :
			this.AsDouble.CompareTo(other.AsDouble);
	}
	public bool Equals(Number other) => CompareTo(other) == 0;

	public static bool operator ==(Number left, Number right) => left.Equals(right);
	public static bool operator !=(Number left, Number right) => !(left == right);

	public static bool operator <(Number left, Number right) => left.CompareTo(right) < 0;
	public static bool operator >(Number left, Number right) => left.CompareTo(right) > 0;

	public static bool operator <=(Number left, Number right) => left.CompareTo(right) <= 0;
	public static bool operator >=(Number left, Number right) => left.CompareTo(right) >= 0;

	// Binary math operators
	public static Number operator +(Number left, Number right)
	{
		return BothAreFractions(left, right) ?
			left.AsFraction + right.AsFraction :
			left.AsDouble + right.AsDouble;
	}
	public static Number operator -(Number left, Number right)
	{
		return BothAreFractions(left, right) ?
			left.AsFraction - right.AsFraction :
			left.AsDouble - right.AsDouble;
	}
	public static Number operator *(Number left, Number right)
	{
		return BothAreFractions(left, right) ?
			left.AsFraction * right.AsFraction :
			left.AsDouble * right.AsDouble;
	}
	public static Number operator /(Number left, Number right)
	{
		return BothAreFractions(left, right) ?
			left.AsFraction / right.AsFraction :
			left.AsDouble / right.AsDouble;
	}
	public static Number operator %(Number left, Number right)
	{
		return BothAreFractions(left, right) ?
			left.AsFraction % right.AsFraction :
			left.AsDouble % right.AsDouble;
	}

	// Overrides
	public override string ToString() => IsFraction ? AsFraction.ToString() : AsDouble.ToString();
	public override bool Equals([NotNullWhen(true)] object? obj) => obj is Number num && Equals(num);
	public override int GetHashCode() => IsFraction ? _valueIfFraction.GetHashCode() : _valueIfDouble.GetHashCode();

	// Private helpers
	private static bool BothAreFractions(Number num1, Number num2) => num1.IsFraction && num2.IsFraction;
}