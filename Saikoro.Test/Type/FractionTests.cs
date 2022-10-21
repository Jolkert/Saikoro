using Saikoro.Types;

namespace Saikoro.Test.Type;

public class FractionTests
{
	#region Reusable Constants
	static readonly Fraction Five = new Fraction(5, 1),

							 OneHalf = new Fraction(1, 2),
							 ThreeHalves = new Fraction(3, 2),
							 NegativeHalf = new Fraction(-1, 2),

							 OneThird = new Fraction(1, 3),
							 TwoThirds = new Fraction(2, 3),
							 FourThirds = new Fraction(4, 3),

							 OneQuarter = new Fraction(1, 4),
							 TwoQuarters = new Fraction(2, 4),

							 TwentysixFifths = new Fraction(26, 5),

							 OneSixth = new Fraction(1, 6),
							 FiveSixths = new Fraction(5, 6);
	#endregion
	const int FloatErrorMargin = 15;

	[Theory]
	[InlineData(1, 4, 1, 4), InlineData(-1, 4, -1, 4), InlineData(-1, 4, 1, -4), InlineData(1, 4, -1, -4)]
	public void ConstructionTest(int expectedNumerator, int expectedDenominator, int numerator, int denominator) => Assert.True(MatchNumeratorAndDenominator((expectedNumerator, expectedDenominator),
																																new Fraction(numerator, denominator)));
	[Fact]
	public void CreationWithDenominatorZero() => Assert.Throws<ArgumentException>(() => new Fraction(1, 0));

	[Fact]
	public void ConversionTest()
	{
		Assert.Equal(Five, (Fraction)5);

		Assert.Equal(5, (int)Five);
		Assert.Equal(1, (int)ThreeHalves);

		Assert.Equal(3.0 / 2, (double)ThreeHalves);
	}

	[Fact]
	public void ComparisonTest()
	{
		Assert.True(OneHalf == TwoQuarters); // equality
		Assert.True(OneHalf != OneQuarter); // inequality

		Assert.True(OneHalf > OneThird); // greater-than
		Assert.True(OneQuarter < OneHalf); // less-than

		Assert.True(OneHalf >= OneThird); // greater or equal: greater
		Assert.True(OneHalf >= TwoQuarters); // greator or equal: equal

		Assert.True(OneQuarter <= OneHalf); // less or equal: less
		Assert.True(OneHalf <= TwoQuarters);   // less or equal: equal
	}

	[Theory]
	[MemberData(nameof(UnaryMinusData))]
	public void UnaryMinus(Fraction expected, Fraction actual) => Assert.Equal(expected, -actual);
	public static IEnumerable<object[]> UnaryMinusData()
	{
		yield return new object[] { NegativeHalf, OneHalf };
		yield return new object[] { OneHalf, NegativeHalf };
	}

	[Fact]
	public void AdditionTest()
	{
		Assert.Equal(FiveSixths, OneHalf + OneThird); // add two fractions
		Assert.Equal(ThreeHalves, OneHalf + 1); // add integer to fraction
		Assert.Equal(ThreeHalves, 1 + OneHalf); // add fraction to integer
	}
	[Fact]
	public void SubtractionTest()
	{
		// Subtraction
		Assert.Equal(OneSixth, OneHalf - OneThird); // subtract two fractions
		Assert.Equal(NegativeHalf, OneHalf - 1); // subtract integer from fraction
		Assert.Equal(OneHalf, 1 - OneHalf); // subtract fraction from integer
	}
	[Fact]
	public void MultiplicationTest()
	{
		Assert.Equal(OneSixth, OneHalf * OneThird); // multiply two fractions
		Assert.Equal(ThreeHalves, OneHalf * 3); // multiply fraction by integer
		Assert.Equal(TwoThirds, 2 * OneThird); // multiply integer by fraction
	}
	[Fact]
	public void DivisionTest()
	{
		Assert.Equal(TwoThirds, OneThird / OneHalf); // divide two fractions
		Assert.Equal(OneQuarter, OneHalf / 2); // divide fraction by integer
		Assert.Equal(FourThirds, 2 / ThreeHalves); // divide integer by fraction
	}
	[Fact]
	public void ModulusTest()
	{
		Assert.Equal(5.2 % 1.5, (double)(TwentysixFifths % ThreeHalves), FloatErrorMargin); // Fraction mod fraction
		Assert.Equal(5.2 % 2, (double)(TwentysixFifths % 2), FloatErrorMargin); // Fraction mod integer
		Assert.Equal(2 % 5.0 / 6, (double)(2 % FiveSixths), FloatErrorMargin); // integer mod fractionS
	}

	[Fact]
	public void ManipulationTests()
	{
		Assert.Equal(OneHalf, TwoQuarters.Reduced());

		Assert.Equal(ThreeHalves, TwoThirds.Reciprocal());
		Assert.Throws<DivideByZeroException>(() => Fraction.Zero.Reciprocal());
	}

	private static bool MatchNumeratorAndDenominator((int numerator, int denominator) expected, Fraction actual) => actual.Numerator == expected.numerator && actual.Denominator == expected.denominator;
}