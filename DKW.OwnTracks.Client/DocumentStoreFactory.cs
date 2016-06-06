using System;
using System.Configuration;
using System.Linq;
using Marten;
using Serilog;

namespace DKW.OwnTracks.Client
{
	internal class DocumentStoreFactory
	{
		private readonly ILogger _logger;

		public DocumentStoreFactory(ILogger logger)
		{
			_logger = logger;
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));
		}

		internal IDocumentStore Create()
		{
			var count = ConfigurationManager.ConnectionStrings.Count;
			if (count <= 0)
				throw new ConfigurationErrorsException("No connection strings have been defined.");

			ConnectionStringSettings conn = null;
			for (var index = 0; index < count; index++) {
				conn = ConfigurationManager.ConnectionStrings[index];
				if (String.Equals(conn.Name, AppConstants.DocumentStoreConnectionName, StringComparison.CurrentCultureIgnoreCase)) {
					break;
				}
			}
			if (conn == null) {
				conn = ConfigurationManager.ConnectionStrings[0];
			}
			return DocumentStore.For(_ => {
				_.Connection(conn.ConnectionString);
				_.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
			});
		}
	}
}
