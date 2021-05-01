namespace Loggo.Api
{
	public interface ILoggerFactory<in T>
	{
		ILogger<T> CreateLogger();
	}
}
