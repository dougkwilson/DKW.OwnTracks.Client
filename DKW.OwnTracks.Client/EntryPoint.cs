using StructureMap;
using Topshelf;

namespace DKW.OwnTracks.Client
{
	internal class EntryPoint
	{
		private static void Main()
		{
			using (var container = new Container(new ServiceRegistry())) {
				HostFactory.Run(x => {
					x.Service<Service>(s => {
						s.ConstructUsing(name => container.GetInstance<Service>());
						s.WhenStarted(ots => ots.Start());
						s.WhenStopped(ots => ots.Stop());
					});
					x.RunAsLocalSystem();

					x.SetDescription("OwnTracks logger.");
					x.SetDisplayName("OwnTracks");
					x.SetServiceName("owntracks");
				});
			}
		}
	}
}
