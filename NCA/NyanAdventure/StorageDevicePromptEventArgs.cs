using System;

namespace NyanAdventure;

public class StorageDevicePromptEventArgs : EventArgs
{
	public bool PromptForDevice { get; set; }
}
