namespace Saikoro.Randomization;
internal struct Roll
{
	public int Value { get; }
	public bool Removed { get; }

	public Roll(int value, bool removed = false)
	{
		Value = value;
		Removed = removed;
	}


	public static implicit operator Roll(int value) => new Roll(value);

	public Roll Remove() => new Roll(Value, true);

	public override bool Equals(object? obj) => obj is Roll roll && Value == roll.Value && Removed == roll.Removed;
	public override int GetHashCode() => (Value, Removed).GetHashCode();
}