using Saikoro.Expressions;

namespace Saikoro;
internal sealed class Result
{

}

public interface IResult
{
	DiceResult? Value { get; }
	Exception? Exception { get; }

	bool IsSuccess { get; }
}