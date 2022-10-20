using OneOf;
using Saikoro.Parsing.Tokenization;
using Saikoro.Types;
using Saikoro.Types.Numeric;
using Saikoro.Types.Operator;

namespace Saikoro.Parsing;
public sealed class Parser
{
	private readonly ITokenizer<string, IToken> _tokenizer;

	public Parser(ITokenizer<string, IToken>? tokenizer = null)
	{
		_tokenizer = tokenizer ?? new Tokenizer();
	}

	public string Parse(string input)
	{
		// fix these OneOf's to have better names?
		Queue<QueueItem> outputQueue = new Queue<QueueItem>();
		Stack<StackItem> operatorStack = new Stack<StackItem>();

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

					while (operatorStack.TryPeek(out StackItem stackTop) && !stackTop.IsDelimiter && (stackTop.AsOperator.Value.Priority >= (op.Priority + (int)stackTop.AsOperator.Value.Associativity)))
						outputQueue.Enqueue(operatorStack.Pop().AsOperator);

					int valency = previous.Type != TokenType.Number ? 1 : 2;
					operatorStack.Push(op.ToValent(valency)); 
					break;
				}

				case TokenType.Delimiter:
				{
					Delimiter delimiter = ((IToken<Delimiter>)token).Value;

					if (delimiter == Delimiter.Open)
						operatorStack.Push(delimiter);
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

		return string.Join(' ', outputQueue.Select(val => val.Value.ToString()));
	}

	private struct QueueItem
	{
		private readonly OneOf<Number, ValentOperator> _value;
		public object Value => _value.Value;

		private QueueItem(OneOf<Number, ValentOperator> value) => _value = value;

		// conversions
		public static implicit operator QueueItem(Number value) => new QueueItem(value);
		public static implicit operator QueueItem(ValentOperator value) => new QueueItem(value);

		// type checks
		public bool IsNumber => _value.IsT0;
		public bool IsOperator => _value.IsT1;

		// casts
		public Number AsNumber => _value.AsT0;
		public ValentOperator AsOperator => _value.AsT1;
	}
	private struct StackItem
	{
		private readonly OneOf<ValentOperator, Delimiter> _value;
		public object Value => _value.Value;

		private StackItem(OneOf<ValentOperator, Delimiter> value) => _value = value;

		// conversions
		public static implicit operator StackItem(Delimiter value) => new StackItem(value);
		public static implicit operator StackItem(ValentOperator value) => new StackItem(value);

		// type checks
		public bool IsOperator => _value.IsT0;
		public bool IsDelimiter => _value.IsT1;

		// casts
		public ValentOperator AsOperator => _value.AsT0;
		public Delimiter AsDelimiter => _value.AsT1;
	}
}