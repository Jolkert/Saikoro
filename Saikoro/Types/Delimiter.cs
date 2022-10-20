namespace Saikoro.Types;
public enum Delimiter
{
	None,
	Open,
	Close
}

public static class DelimiterExtensions
{
	public static Delimiter ToDelimiter(this string self)
	{
		if (self.Length != 1)
			return Delimiter.None;

		return self[0] switch
		{
			'(' => Delimiter.Open,
			')' => Delimiter.Close,
			_ => Delimiter.None
		};
	}
}