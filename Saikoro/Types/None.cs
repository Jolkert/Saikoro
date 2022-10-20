namespace Saikoro.Types;
public sealed class None
{
	private static None? _instance;

	public static None Instance
	{
		get
		{
			_instance ??= new None();
			return _instance;
		}
	}
}