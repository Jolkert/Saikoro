using Saikoro.Types.Operator;

namespace Saikoro.Parsing;
internal static class Extensions
{
	public static ValentOperator ToValent(this Operator op, int valency) => new ValentOperator(op.Value, valency);
}