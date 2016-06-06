using System;
using System.Diagnostics.Contracts;
using DKW.OwnTracks.Client.Messages;
using Marten;
using Serilog;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DKW.OwnTracks.Client
{
	public class Service : IDisposable
	{
		private readonly IDocumentStore _documentStore;
		private readonly ILogger _logger;
		private MqttClient _client;
		private Boolean _isDisposed; // To detect redundant calls

		public Service(IDocumentStore documentStore, ILogger logger)
		{
			Contract.Requires(documentStore != null);
			Contract.Requires(logger != null);
			if (documentStore == null) {
				throw new ArgumentNullException(nameof(documentStore));
			}

			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}

			_documentStore = documentStore;
			_logger = logger;
		}

		public void Start()
		{
			EnsureNotDisposed();

			// TODO: Put the magic strings in a config file or something. -dw
			_client = new MqttClient("spartan.dkw.io");
			_client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
			_client.Connect("home.dkw.io-", "OwnTracks", "armageddon");
			_client.Subscribe(new string[] { "owntracks.*.*" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
			_client.Subscribe(new string[] { "owntracks.*.*.event" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
			_client.Subscribe(new string[] { "owntracks.*.*.info" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		}

		public void Stop()
		{
			_client.Unsubscribe(new string[] { "owntracks.*.*" });
			_client.Unsubscribe(new string[] { "owntracks.*.*.event" });
			_client.Unsubscribe(new string[] { "owntracks.*.*.info" });
			_client.Disconnect();
		}

		public void Dispose()
		{
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is implemented.
			// GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed) {
				if (disposing) {
					if (_client.IsConnected) {
						_client.Disconnect();
					}

					_client = null;
				}
				_isDisposed = true;
			}
		}

		private void EnsureNotDisposed()
		{
			if (_isDisposed) {
				throw new ObjectDisposedException(GetType().FullName, "The service is not reusable once it has been disposed.");
			}
		}

		private void Client_MqttMsgPublishReceived(Object sender, MqttMsgPublishEventArgs e)
		{
			using (var session = _documentStore.LightweightSession()) {
				var msg = System.Text.Encoding.ASCII.GetString(e.Message);
				var location = Newtonsoft.Json.JsonConvert.DeserializeObject<Location>(msg);
				_logger.Information($"{msg}");
			}
		}
	}
}
