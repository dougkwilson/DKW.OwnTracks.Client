using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DKW.OwnTracks.Client.Messages;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace DKW.OwnTracks.Client.Tests
{
	public class Serialization_to
	{
		public class Binary : Serialization
		{
			public Binary(ITestOutputHelper output)
				: base(output)
			{
			}

			protected override T RoundTrip<T>(T before)
			{
				Byte[] bytes;
				var formatter = new BinaryFormatter();
				using (var stream = new MemoryStream()) {
					formatter.Serialize(stream, before);
					bytes = stream.ToArray();
				}
				using (var stream = new MemoryStream(bytes)) {
					return (T)formatter.Deserialize(stream);
				}
			}
		}

		public class Json : Serialization
		{
			private readonly ITraceWriter _traceWriter;
			private readonly JsonSerializerSettings _jss;

			public Json(ITestOutputHelper output)
				: base(output)
			{
				_traceWriter = new MemoryTraceWriter();
				_jss = new JsonSerializerSettings {
					TypeNameHandling = TypeNameHandling.All,
					TraceWriter = _traceWriter
				};
			}

			protected override T RoundTrip<T>(T before)
			{
				var serializedObject = JsonConvert.SerializeObject(before, _jss);
				var after = JsonConvert.DeserializeObject<T>(serializedObject, _jss);
				Output.WriteLine(_traceWriter.ToString());
				return after;
			}
		}
	}

	[SuppressMessage("General", "RCS1060:Consider declaring each type in separate file.", Justification = "I did consider it. -dw", Scope = "type", Target = "~T:DKW.OwnTracks.Client.Tests.Serialization")]
	public abstract class Serialization : SerializationBase
	{
		protected Serialization(ITestOutputHelper output)
				: base(output)
		{
		}

		[Fact]
		public void Location()
		{
			var before = new Location() {
				Acc = 75,
				Alt = 13,
				Batt = 80,
				Cog = 270,
				Desc = "Entered a way-point.",
				Event = "enter",
				Lat = 49.92766555403352,
				Lon = -119.4390890469024,
				Rad = 30,
				T = Trigger.A,
				Tid = "YY",
				Tst = 1376715317,
				Vacc = 10,
				Vel = 54,
				P = 100
			};

			var after = RoundTrip(before);

			after.Acc.Should().Be(before.Acc);
			after.Alt.Should().Be(before.Alt);
			after.Batt.Should().Be(before.Batt);
			after.Cog.Should().Be(before.Cog);
			after.Desc.Should().Be(before.Desc);
			after.Event.Should().Be(before.Event);
			after.Lat.Should().Be(before.Lat);
			after.Lon.Should().Be(before.Lon);
			after.Rad.Should().Be(before.Rad);
			after.T.Should().Be(before.T);
			after.Tid.Should().Be(before.Tid);
			after.Tst.Should().Be(before.Tst);
			after.Vacc.Should().Be(before.Vacc);
			after.Vel.Should().Be(before.Vel);
			after.P.Should().Be(before.P);
		}
	}
}
