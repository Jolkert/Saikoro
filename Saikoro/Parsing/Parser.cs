using OneOf;
using Saikoro.Parsing.Tokenization;
using Saikoro.Types;
using Saikoro.Types.Numeric;
using Saikoro.Types.Operator;

namespace Saikoro.Parsing;
internal sealed class Parser
{
	private static readonly ValentOperator BinaryMultiply = ((Operator)OperatorValue.Multiply).ToValent(2);

	private readonly ITokenizer<string, IToken> _tokenizer;

	public Parser(ITokenizer<string, IToken>? tokenizer = null)
	{
		_tokenizer = tokenizer ?? new Tokenizer();
	}

	public Queue<NumberOrOperator> Parse(string input)
	{
		// fix these OneOf's to have better names?
		Queue<NumberOrOperator> outputQueue = new Queue<NumberOrOperator>();
		Stack<OperatorOrDelimiter> operatorStack = new Stack<OperatorOrDelimiter>();

		IToken previous = Token.Empty;
		foreach (IToken token in _tokenizer.GetTokenStream(input))
		{
			switch (token.Type)
			{
				case TokenType.Number:
				{
					outputQueue.Enqueue(((IToken<Number>)token).Value);
					break;
				}

				case TokenType.Operator:
				{
					Operator op = ((IToken<Operator>)token).Value;
					int valency = previous.Type == TokenType.Number || previous.IsDelimiter(Delimiter.Close) ? 2 : 1;

					PushOperatorToStack(op.ToValent(valency));
					break;
				}

				case TokenType.Delimiter:
				{
					Delimiter delimiter = ((IToken<Delimiter>)token).Value;

					if (delimiter == Delimiter.Open)
					{
						if (previous.Type == TokenType.Number || previous.IsDelimiter(Delimiter.Close))
							PushOperatorToStack(BinaryMultiply);
						operatorStack.Push(delimiter);
					}
					else
					{
						while (!operatorStack.Peek().Value.Equals(Delimiter.Open))
						{
							ValentOperator op = operatorStack.Pop().AsOperator; // this *should* always be an operator? -jolk 2022-10-20
							outputQueue.Enqueue(op);
						}
						operatorStack.Pop();
					}
					break;
				}

				default:
					throw new Exception("Unexpected token!");
			}
			previous = token;
		}

		while (operatorStack.Count > 0)
			outputQueue.Enqueue(operatorStack.Pop().AsOperator);

		return outputQueue;

		void PushOperatorToStack(ValentOperator op)
		{
			while (operatorStack.TryPeek(out OperatorOrDelimiter stackTop) &&
						   !stackTop.IsDelimiter &&
						   stackTop.AsOperator.Valency <= op.Valency &&
						   stackTop.AsOperator.Value.Priority >= (op.Value.Priority + (int)stackTop.AsOperator.Value.Associativity))
			{
				outputQueue.Enqueue(operatorStack.Pop().AsOperator);
			}

			operatorStack.Push(op);
		}
	}

	private struct OperatorOrDelimiter
	{
		private readonly OneOf<ValentOperator, Delimiter> _value;
		public object Value => _value.Value;

		private OperatorOrDelimiter(OneOf<ValentOperator, Delimiter> value) => _value = value;

		// conversions
		public static implicit operator OperatorOrDelimiter(Delimiter value) => new OperatorOrDelimiter(value);
		public static implicit operator OperatorOrDelimiter(ValentOperator value) => new OperatorOrDelimiter(value);

		// type checks
		public bool IsOperator => _value.IsT0;
		public bool IsDelimiter => _value.IsT1;

		// casts
		public ValentOperator AsOperator => _value.AsT0;
		public Delimiter AsDelimiter => _value.AsT1;
	}
}