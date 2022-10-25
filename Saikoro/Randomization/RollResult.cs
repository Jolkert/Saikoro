using System.Collections;

namespace Saikoro.Randomization;
public sealed class RollResult : IEnumerable<Roll>
{
	public int Count { get; }
	public int Faces { get; }
	public string DieExpression => $"{(Count > 1 ? Count.ToString() : "")}d{Faces}";

	private readonly Roll[] _rolls;
	Roll this[int index] => _rolls[index];

	public RollResult(int count, int faces, Roll[] rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;
		
		_rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			_rolls[i] = rolls[i];
	}
	public RollResult(int count, int faces, Span<Roll> rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;

		_rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			_rolls[i] = rolls[i];
	}

	public RollResult(int count, int faces, int[] rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;

		_rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			_rolls[i] = rolls[i];
	}
	public RollResult(int count, int faces, Span<int> rolls)
	{
		if (count != rolls.Length)
			throw new ArgumentException($"{nameof(RollResult)}.{nameof(Count)} does not match {nameof(rolls)}.Length");

		Count = count;
		Faces = faces;

		_rolls = new Roll[rolls.Length];
		for (int i = 0; i < rolls.Length; i++)
			_rolls[i] = rolls[i];
	}

	public int Sum() => this.Where(roll => !roll.Removed).Sum(roll => roll.Value);

	public RollResult RemoveWhere(Func<Roll, bool> predicate)
	{
		for (int i = 0; i < _rolls.Length; i++)
			if (predicate(_rolls[i]))
				_rolls[i] = _rolls[i].Remove();

		return this;
	}

	public override string ToString() => $"{DieExpression}: {{{string.Join(", ", _rolls)}}}";

	public IEnumerator<Roll> GetEnumerator() => ((IEnumerable<Roll>)_rolls).GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => _rolls.GetEnumerator();
}