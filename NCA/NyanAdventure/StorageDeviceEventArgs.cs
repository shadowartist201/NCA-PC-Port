using System;

namespace NyanAdventure;

public class StorageDeviceEventArgs : EventArgs
{
	public StorageDeviceSelectorEventResponse EventResponse { get; set; }
}
