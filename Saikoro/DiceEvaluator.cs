using Saikoro.Expressions;
using Saikoro.Parsing;
using Saikoro.Randomization;
using Saikoro.Types.Operator;
using System.Diagnostics;

namespace Saikoro;
public sealed class DiceEvaluator
{
	private readonly DiceRoller _roller;
	private readonly Parser _parser = new Parser();

	public DiceEvaluator(Random? random = null)
	{
		_roller = new DiceRoller(random ?? new Random());
		_delegateMap = new Dictionary<ValentOperator, Func<IntermediateValue[], IntermediateValue>>()
		{
			{ BinaryPlus,  args => args[0].AsNumber + args[1].AsNumber },
			{ BinaryMinus, args => args[0].AsNumber - args[1].AsNumber },
			{ Multiply,    args => args[0].AsNumber * args[1].AsNumber },
			{ Divide,      args => args[0].AsNumber / args[1].AsNumber },
			{ Modulus,     args => args[0].AsNumber % args[1].AsNumber },
			{ Power,       args => args[0].AsNumber.Raise(args[1].AsNumber) },

			{ UnaryPlus,  args => +args[0].AsNumber },
			{ UnaryMinus, args => -args[0].AsNumber },

			{ BinaryDice, args => _roller.Roll((int)args[1].AsNumber, (int)args[0].AsNumber) },
			{ UnaryDice,  args => _roller.Roll((int)args[0].AsNumber, 1) },

			{ Equal,          args => args[0].AsRollResult.RemoveWhere(roll => !(roll.Value == args[1].AsNumber)) },
			{ NotEqual,       args => args[0].AsRollResult.RemoveWhere(roll => !(roll.Value != args[1].AsNumber)) },
			{ LessThan,       args => args[0].AsRollResult.RemoveWhere(roll => !(roll.Value < args[1].AsNumber)) },
			{ GreaterThan,    args => args[0].AsRollResult.RemoveWhere(roll => !(roll.Value > args[1].AsNumber)) },
			{ LessOrEqual,    args => args[0].AsRollResult.RemoveWhere(roll => !(roll.Value <= args[1].AsNumber)) },
			{ GreaterOrEqual, args => args[0].AsRollResult.RemoveWhere(roll => !(roll.Value >= args[1].AsNumber)) },
		};
	}

	public string Evaluate(string input)
	{
		Queue<NumberOrOperator> parseQueue = _parser.Parse(input);
		Stack<IntermediateValue> evaluationStack = new Stack<IntermediateValue>();

		DiceResultBuilder result = new DiceResultBuilder();

		while (parseQueue.Count > 0)
		{
			NumberOrOperator item = parseQueue.Dequeue();

			if (item.IsNumber)
				evaluationStack.Push(item.AsNumber);
			else
			{
				IntermediateValue intermediate = _delegateMap[item.AsOperator](evaluationStack.Multipop(item.AsOperator.Valency));
				if (item.AsOperator.Value == OperatorValue.Dice)
					result.AddRoll(intermediate.AsRollResult);

				evaluationStack.Push(intermediate);
			}
		}

		result.Total = evaluationStack.Pop().AsNumber;

		return result.Build().ToString();
	}

	private readonly IReadOnlyDictionary<ValentOperator, Func<IntermediateValue[], IntermediateValue>> _delegateMap;

	#region Operators
	private static readonly ValentOperator
		BinaryPlus     = new ValentOperator(OperatorValue.Plus, 2),
		BinaryMinus    = new ValentOperator(OperatorValue.Minus, 2),
		Multiply       = new ValentOperator(OperatorValue.Multiply, 2),
		Divide         = new ValentOperator(OperatorValue.Divide, 2),
		Modulus        = new ValentOperator(OperatorValue.Modulus, 2),
		Power          = new ValentOperator(OperatorValue.Power, 2),
		BinaryDice     = new ValentOperator(OperatorValue.Dice, 2),
		Equal          = new ValentOperator(OperatorValue.Equal, 2),
		NotEqual       = new ValentOperator(OperatorValue.NotEqual, 2),
		LessThan       = new ValentOperator(OperatorValue.LessThan, 2),
		GreaterThan    = new ValentOperator(OperatorValue.GreaterThan, 2),
		LessOrEqual    = new ValentOperator(OperatorValue.LessOrEqual, 2),
		GreaterOrEqual = new ValentOperator(OperatorValue.GreaterOrEqual, 2),

		UnaryPlus  = new ValentOperator(OperatorValue.Plus, 1),
		UnaryMinus = new ValentOperator(OperatorValue.Minus, 1),
		UnaryDice  = new ValentOperator(OperatorValue.Dice, 1);
	#endregion
}

internal static class EvaluationExtensions
{
	public static T[] Multipop<T>(this Stack<T> stack, int count)
	{
		T[] arr = new T[count];
		for (int i = count - 1; i >= 0; i--)
			arr[i] = stack.Pop();
		return arr;
	}
}