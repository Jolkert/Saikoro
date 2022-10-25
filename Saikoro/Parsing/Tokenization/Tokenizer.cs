using Saikoro.Types;
using Saikoro.Types.Numeric;
using Saikoro.Types.Operator;
using System.Text.RegularExpressions;

namespace Saikoro.Parsing.Tokenization;
internal sealed class Tokenizer : ITokenizer<string, IToken>
{
	public IEnumerable<IToken> GetTokenStream(string input)
	{
		int position = 0;
		while (position < input.Length)
		{
			string match = ""; TokenType type = TokenType.None;
			foreach ((Regex pattern, TokenType tokenType) in TokenDictionary)
			{
				Match matchAttempt = pattern.Match(input[position..]);

				if (matchAttempt.Success)
				{
					match = matchAttempt.Value;
					type = tokenType;
					position += matchAttempt.Length;
					break;
				}
			}

			if (type == TokenType.Whitespace)
				continue;

			yield return type switch
			{
				TokenType.Delimiter => Token<Delimiter>.Parse(match, DelimiterExtensions.ToDelimiter),
				TokenType.Number => Token<Number>.Parse(match, str => (Number)int.Parse(str)),
				TokenType.Operator => Token<Operator>.Parse(match, Operator.FromString),
				_ => Token.Empty
			};
		}
	}

	private static readonly RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
	private static readonly IReadOnlyDictionary<Regex, TokenType> TokenDictionary = new Dictionary<Regex, TokenType>()
	{
		{ new Regex(@"^(\s+)", Options),                             TokenType.Whitespace },
		{ new Regex(@"^(\(|\))", Options),                           TokenType.Delimiter },
		{ new Regex(@"^(\d+)", Options),                             TokenType.Number },
		{ new Regex(@"^(\+|-|\*|/|%|\^|d|<=|>=|<|>|!=|=)", Options), TokenType.Operator }
	};
}

public interface ITokenizer<TInput, TToken>
{
	public IEnumerable<TToken> GetTokenStream(TInput input);
}