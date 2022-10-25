namespace Saikoro.Randomization;
public sealed class DiceRoller
{
	private readonly Random _random;
	public DiceRoller(Random random)
	{
		_random = random;
	}

	public RollResult Roll(int faces, int count = 1)
	{
		Span<Roll> rolls = stackalloc Roll[count]; // fuck reference types. all my homies hate reference types -jolk 2022-10-21
		for (int i = 0; i < count; i++)
			rolls[i] = _random.Next(faces) + 1;

		return new RollResult(count, faces, rolls);
	}
}