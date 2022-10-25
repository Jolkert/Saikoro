using Saikoro.Parsing.Tokenization;
using Saikoro.Types;
using Saikoro.Types.Operator;

namespace Saikoro.Parsing;
internal static class Extensions
{
	public static ValentOperator ToValent(this Operator op, int valency) => new ValentOperator(op.Value, valency);
	public static bool IsDelimiter(this IToken token, Delimiter delimiterType) => token.Type == TokenType.Delimiter && ((IToken<Delimiter>)token).Value == delimiterType;
}