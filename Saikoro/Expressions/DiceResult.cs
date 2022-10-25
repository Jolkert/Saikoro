using Saikoro.Randomization;
using Saikoro.Types.Numeric;

namespace Saikoro.Expressions;
public sealed class DiceResult
{// TOOD: this could probably use a better name? -jolk 2022-10-22
	public Number Total { get; }
	public IReadOnlyCollection<RollResult> Rolls { get; }

	public DiceResult(Number total, RollResult[] rolls)
	{
		Total = total;

		RollResult[] copiedRolls = new RollResult[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			copiedRolls[i] = rolls[i];

		Rolls = copiedRolls;
	}

	public override string ToString()
	{
		return $"<{Total}>\n{string.Join('\n', Rolls)}";
	}
}

internal sealed class DiceResultBuilder
{
	public Number Total { get; set; }
	public readonly List<RollResult> _rolls;

	public DiceResultBuilder()
	{
		Total = 0;
		_rolls = new List<RollResult>();
	}

	public DiceResultBuilder AddRoll(RollResult rollResult)
	{
		_rolls.Add(rollResult);
		return this;
	}

	public DiceResult Build() => new DiceResult(Total, _rolls.ToArray());

	public static implicit operator DiceResult(DiceResultBuilder builder) => builder.Build();
}