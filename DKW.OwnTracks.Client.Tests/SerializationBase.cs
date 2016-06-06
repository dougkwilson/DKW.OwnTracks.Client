using Xunit.Abstractions;

namespace DKW.OwnTracks.Client.Tests
{
	public abstract class SerializationBase
	{
		protected readonly ITestOutputHelper Output;

		protected SerializationBase(ITestOutputHelper output)
		{
			Output = output;
		}

		protected abstract T RoundTrip<T>(T before);
	}
}
