namespace Saikoro.Randomization;
public sealed class RollResult
{
	public int Count { get; }
	public int Faces { get; }
	public string DieExpression => $"{(Count > 1 ? Count.ToString() : "")}d{Faces}";

	public Roll[] Rolls { get; }

	public RollResult(int count, int faces, Roll[] rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;
		
		Rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			Rolls[i] = rolls[i];
	}
	public RollResult(int count, int faces, Span<Roll> rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;

		Rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			Rolls[i] = rolls[i];
	}

	public RollResult(int count, int faces, int[] rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;

		Rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			Rolls[i] = rolls[i];
	}
	public RollResult(int count, int faces, Span<int> rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;

		Rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			Rolls[i] = rolls[i];
	}

	public int Sum() => Rolls.Where(roll => !roll.Removed).Select(roll => roll.Value).Sum();

	public void RemoveWhere(Func<Roll, bool> predicate)
	{
		for (int i = 0; i < Rolls.Length; i++)
			if (predicate(Rolls[i]))
				Rolls[i] = Rolls[i].Remove();
	}

	public override string ToString() => $"{DieExpression}: {{{string.Join(", ", Rolls)}}}";
}