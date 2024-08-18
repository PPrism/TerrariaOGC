using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

public class StorageDeviceManager : GameComponent
{
	private bool WasDeviceConnected;

	private bool ShowDeviceSelector;

	public bool IsDone = true;

	public StorageDevice Device;

	public PlayerIndex? Player;

	public PlayerIndex PlayerToPrompt;

	public int NumRequiredBytes;

	public event EventHandler DeviceSelected;

	public event EventHandler<EventArgs> DeviceSelectorCanceled;

	public event EventHandler<EventArgs> DeviceDisconnected;

	public StorageDeviceManager(Game Instance) : this(Instance, null, 0) { }

	public StorageDeviceManager(Game Instance, PlayerIndex PlayerIdx) : this(Instance, PlayerIdx, 0) { }

	public StorageDeviceManager(Game Instance, int RequiredBytes) : this(Instance, null, RequiredBytes) { }

	public StorageDeviceManager(Game Instance, PlayerIndex PlayerIdx, int RequiredBytes) : this(Instance, (PlayerIndex?)PlayerIdx, RequiredBytes) { }

	private StorageDeviceManager(Game Instance, PlayerIndex? PlayerIdx, int RequiredBytes) : base(Instance)
	{
		Player = PlayerIdx;
		NumRequiredBytes = RequiredBytes;
		PlayerToPrompt = PlayerIndex.One;
	}

	public void PromptForDevice()
	{
		if (IsDone)
		{
			IsDone = false;
			ShowDeviceSelector = true;
		}
	}

	public override void Update(GameTime Gt)
	{
		bool Flag = false;
		if (Device != null)
		{
			Flag = Device.IsConnected;
			if (!Flag && WasDeviceConnected)
			{
				FireDeviceDisconnectedEvent();
			}
		}
		try
		{
			if (!Guide.IsVisible && ShowDeviceSelector)
			{
				ShowDeviceSelector = false;
				if (Player.HasValue)
				{
					StorageDevice.BeginShowSelector(Player.Value, NumRequiredBytes, 0, DeviceSelectorCallback, null);
				}
				else
				{
					StorageDevice.BeginShowSelector(NumRequiredBytes, 0, DeviceSelectorCallback, null);
				}
			}
		}
		catch (GamerServicesNotAvailableException)
		{
		}
		catch (GuideAlreadyVisibleException)
		{
		}
		WasDeviceConnected = Flag;
	}

	private void DeviceSelectorCallback(IAsyncResult Result)
	{
		Device = StorageDevice.EndShowSelector(Result);
		if (Device != null)
		{
            DeviceSelected?.Invoke(this, null);
            WasDeviceConnected = true;
		}
		else
		{
            DeviceSelectorCanceled?.Invoke(this, null);
            ShowDeviceSelector = false;
		}
		IsDone = true;
	}

	private void FireDeviceDisconnectedEvent()
	{
		Device = null;
        DeviceDisconnected?.Invoke(this, null);
        ShowDeviceSelector = false;
	}
}
