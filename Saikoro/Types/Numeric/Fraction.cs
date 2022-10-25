using static System.Math;

namespace Saikoro.Types;
public readonly record struct Fraction(int Numerator, int Denominator) : IComparable<Fraction>
{
	public static readonly Fraction Zero = new Fraction(0, 1);
	public static readonly Fraction One  = new Fraction(1, 1);

	// Properties
	public int Numerator   { get; } = (Sign(Numerator) == Sign(Denominator)) ? Abs(Numerator) : Sign(Denominator) * Numerator;
	public int Denominator { get; } = (Denominator != 0) ? Abs(Denominator) : throw new ArgumentException("Denominator cannot be zero!", nameof(Denominator));

	public int Sign { get => Sign(Numerator); }

	// Conversions
	public static implicit operator Fraction(int n) => new Fraction(n, 1);
	public static explicit operator int(Fraction fraction) => fraction.Numerator / fraction.Denominator;

	public static explicit operator double(Fraction fraction) => (double)fraction.Numerator / fraction.Denominator;

	// Comparison
	public int CompareTo(Fraction other) => CrossProductDifference(this, other);
	public bool Equals(Fraction other) => CompareTo(other) == 0;

	public static bool operator <(Fraction left, Fraction right) => left.CompareTo(right) < 0;
	public static bool operator >(Fraction left, Fraction right) => left.CompareTo(right) > 0;

	public static bool operator <=(Fraction left, Fraction right) => left.CompareTo(right) <= 0;
	public static bool operator >=(Fraction left, Fraction right) => left.CompareTo(right) >= 0;

	// Unary operators
	public static Fraction operator +(Fraction operand) => operand;
	public static Fraction operator -(Fraction operand) => operand.WithNewNumerator(-operand.Numerator);

	// Binary mathematical operators
	public static Fraction operator +(Fraction left, Fraction right)
	{
		Fraction sum;
		if (left.Denominator == right.Denominator)
			sum = left.WithNewNumerator(left.Numerator + right.Numerator);
		else if (left.Denominator == 1)
			sum = right.WithNewNumerator(right.Numerator + (left.Numerator * right.Denominator));
		else if (left.Denominator == 1)
			sum = left.WithNewNumerator(left.Numerator + (right.Numerator * left.Denominator));
		else
		{
			int lcm = Lcm(left.Denominator, right.Denominator);
			int newNumerator = ((lcm / left.Denominator) * left.Numerator) + ((lcm / right.Denominator) * right.Numerator);
			sum = new Fraction(newNumerator, lcm);
		}

		return sum.Reduced();
	}
	public static Fraction operator -(Fraction left, Fraction right) => left + (-right);
	public static Fraction operator *(Fraction left, Fraction right) => new Fraction(left.Numerator * right.Numerator, left.Denominator * right.Denominator).Reduced();
	public static Fraction operator /(Fraction left, Fraction right) => left * right.Reciprocal();
	public static Fraction operator %(Fraction left, Fraction right) => left - (right * Floor(left / right)); // tbh I don't understand why this formula should hold for nonintegers? but whatever it works -jolk 2022-10-16

	// Member Transformations
	public Fraction Raise(int power)
	{
		if (power == 0)
			return One;

		if (power < 0)
			return Reciprocal().Raise(-power);

		return new Fraction((int)Pow(Numerator, power), (int)Pow(Denominator, power)).Reduced();
	}
	public Fraction Reciprocal() => (Numerator != 0) ? new Fraction(Denominator, Numerator) : throw new DivideByZeroException();
	public Fraction Reduced()
	{
		if (Numerator == 0)
			return Zero;

		int lcm = Lcm(Numerator, Denominator);
		return (lcm == 1) ? this : new Fraction(lcm / Denominator, lcm / Numerator);
	}

	// Static functions
	public static int Floor(Fraction fraction) => (int)Math.Floor((double)fraction);

	// Private helper functions
	private Fraction WithNewNumerator(int newNumerator) => new Fraction(newNumerator, Denominator);
	private static int CrossProductDifference(Fraction left, Fraction right) => (left.Numerator * right.Denominator) - (right.Numerator * left.Denominator);
	private static int Lcm(int a, int b)
	{
		return (a * b) / Gcd(a, b);

		static int Gcd(int a, int b)
		{
			while (b != 0)
				(a, b) = (b, a % b);

			return a;
		}
	}

	// Miscellaneous
	public override string ToString() => (Numerator == 0 || Denominator == 1) ? Numerator.ToString() : $"{Numerator}/{Denominator}";
	public override int GetHashCode() => (Numerator, Denominator).GetHashCode();
	public void Deconstruct(out int numerator, out int denominator)
	{
		numerator = Numerator;
		denominator = Denominator;
	}
}