using System;

namespace DKW.OwnTracks.Client.Messages
{
	[Serializable]
	public class Location
	{
		public MessageType Type { get; } = MessageType.Location;

		/// <summary>
		/// Accuracy of the reported location in meters.
		/// </summary>
		public Int32 Acc { get; set; }

		/// <summary>
		/// Altitude measured in meters above sea level.
		/// </summary>
		public Int32 Alt { get; set; }

		/// <summary>
		/// The device's battery level in percent.
		/// </summary>
		public Int32 Batt { get; set; }

		/// <summary>
		/// The heading (course over ground) in degrees, 0 = North.
		/// </summary>
		public Int32 Cog { get; set; }

		/// <summary>
		/// The description of a way-point.
		/// </summary>
		public String Desc { get; set; }

		/// <summary>
		/// One of "enter" or "leave" and tells if the device is entering or leaving a geo-fence.
		/// </summary>
		public String Event { get; set; }

		/// <summary>
		/// Latitude
		/// </summary>
		public Double Lat { get; set; }

		/// <summary>
		/// Longitude
		/// </summary>
		public Double Lon { get; set; }

		/// <summary>
		/// The radius in meters around the geo-fence when entering/leaving a geo-fence.
		/// </summary>
		public Int32 Rad { get; set; }

		/// <summary>
		/// The trigger for the publish.
		/// </summary>
		public Trigger T { get; set; }

		/// <summary>
		/// A two character configurable tracker-ID.
		/// </summary>
		public String Tid { get; set; }

		/// <summary>
		/// UNIX epoch timestamp of the event as it occurs which may be different from the time it is published.
		/// </summary>
		public Int32 Tst { get; set; }

		/// <summary>
		/// The vertical accuracy of the reported altitude in meters.
		/// </summary>
		public Int32 Vacc { get; set; }

		/// <summary>
		/// The velocity (speed) in km/h.
		/// </summary>
		public Int32 Vel { get; set; }

		/// <summary>
		/// Barometric pressure in kPa.
		/// </summary>
		public Double P { get; set; }
	}
}
