namespace Saikoro.Parsing.Tokenization;
public interface IToken
{
	public TokenType Type { get; }
	public object Value { get; }
}

public interface IToken<T> : IToken
{
	public new T Value { get; }
}