using System.Data;
using Marten;
using Serilog;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace DKW.OwnTracks.Client
{
	public class ServiceRegistry : Registry
	{
		public ServiceRegistry()
		{
			Scan(s => {
				s.TheCallingAssembly();
				s.WithDefaultConventions();
			});

			For<ILogger>().Singleton()
				.Use(new LoggerConfiguration()
					.Enrich.WithMachineName()
					.MinimumLevel.Debug()
					.WriteTo.ColoredConsole()
					.WriteTo.Trace()
					.CreateLogger());

			For<IDocumentStore>()
				.LifecycleIs<SingletonLifecycle>()
				.Use(c => c.GetInstance<DocumentStoreFactory>().Create());

			For<IDocumentSession>()
				.LifecycleIs<UniquePerRequestLifecycle>()
				.Use(c => c.GetInstance<IDocumentStore>().LightweightSession(IsolationLevel.ReadCommitted));
		}
	}
}
