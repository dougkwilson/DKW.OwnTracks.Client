namespace DKW.OwnTracks.Client.Messages
{
	public enum Trigger
	{
		/// <summary>
		/// Ping
		/// </summary>
		P,

		/// <summary>
		/// Circular region enter/leave event
		/// </summary>
		C,

		/// <summary>
		/// Beacon region enter/leave event
		/// </summary>
		B,

		/// <summary>
		/// Response to a "reportLocation" request
		/// </summary>
		R,

		/// <summary>
		/// Manual publish requested by the user
		/// </summary>
		U,

		/// <summary>
		/// Timer based publish in move move
		/// </summary>
		T,

		/// <summary>
		/// Automatic location update
		/// </summary>
		A
	}
}
