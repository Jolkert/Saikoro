using System.Diagnostics.CodeAnalysis;

namespace Saikoro.Types.Operator;
public readonly struct ValentOperator
{
	public Operator Value { get; }
	public int Valency { get; }
	// private Func<IntermediateValue[], IntermediateValue> _evaluationDelegate;

	public ValentOperator(Operator value, int valency/*, Func<IntermediateValue[], IntermediateValue>? evaluationDelegate*/)
	{
		Value = value;
		Valency = valency;
		// _evaluationDelegate = evaluationDelegate ?? throw new ArgumentNullException(nameof(evaluationDelegate));
	}

	/*public static ValentOperator From(OperatorValue value, int valency)
	{
		ValentOperator? op = _cache.GetValueOrDefault((value, valency));
		if (op is not null)
			return op;

		op = new ValentOperator(value, valency, _delegateMap.GetValueOrDefault((value, valency), _unsupportedOperation));
		_cache.Add((value, valency), op);
		return op;
	}
	private static readonly IReadOnlyDictionary<(OperatorValue, int), Func<IntermediateValue[], IntermediateValue>> _delegateMap = new Dictionary<(OperatorValue, int), Func<IntermediateValue[], IntermediateValue>>()
	{
		{ (OperatorValue.Plus,     2), args => args[0].AsNumber + args[1].AsNumber },
		{ (OperatorValue.Minus,    2), args => args[0].AsNumber - args[1].AsNumber },
		{ (OperatorValue.Multiply, 2), args => args[0].AsNumber * args[1].AsNumber },
		{ (OperatorValue.Divide,   2), args => args[0].AsNumber / args[1].AsNumber },
		{ (OperatorValue.Modulus,  2), args => args[0].AsNumber % args[1].AsNumber },
		{ (OperatorValue.Power,    2), args => args[0].AsNumber.Raise(args[1].AsNumber)},

		{ (OperatorValue.Plus,  1), args => +args[0].AsNumber },
		{ (OperatorValue.Minus, 1), args => -args[0].AsNumber },
	};
	private static readonly Dictionary<(OperatorValue, int), ValentOperator> _cache = new Dictionary<(OperatorValue, int), ValentOperator>();
	private static readonly Func<IntermediateValue[], IntermediateValue> _unsupportedOperation = args => throw new NotSupportedException();

	internal IntermediateValue Evaluate(IntermediateValue[] args)
	{
		if (args.Length != Valency)
			throw new ArgumentException($"{this} expected {Valency} arguments, but got {args.Length}!");

		return _evaluationDelegate(args);
	}*/

	public override bool Equals([NotNullWhen(true)] object? obj) => obj is ValentOperator op && (Value == op.Value && Valency == op.Valency);
	public override int GetHashCode() => (Value, Valency).GetHashCode();

	public override string ToString() => $"{Value}_{ValencyString(Valency)}";
	private static string ValencyString(int valency)
	{
		return valency switch
		{
			0 => "nullary",
			1 => "unary",
			2 => "binary",
			3 => "ternary",
			4 => "quaternary",
			_ => $"{valency}-ary"
		};
	}
}