using Saikoro.Types;
using Saikoro.Types.Numeric;
using Saikoro.Types.Operator;

namespace Saikoro.Parsing.Tokenization;
internal class Token<T> : IToken<T> where T : notnull
{
	public TokenType Type { get; }

	public T Value { get; }
	object IToken.Value => Value;

	private Token(T value)
	{
		Type = value switch
		{
			Number => TokenType.Number,
			Operator => TokenType.Operator,
			Delimiter => TokenType.Delimiter,
			_ => TokenType.None
		};
		Value = value;
	}

	public static implicit operator Token<T>(T value) => new Token<T>(value);

	public static Token<T> Parse(string input, Func<string, T> parseDelegate) => parseDelegate(input);

	public override string ToString() => $"{Type} : <{Value}>";
	public override bool Equals(object? obj) => obj is Token<T> token && Type == token.Type && Value.Equals(token.Value);
	public override int GetHashCode() => base.GetHashCode();
}

internal static class Token
{
	public static Token<None> Empty { get; } = None.Instance;
}