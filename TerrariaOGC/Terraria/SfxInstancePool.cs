using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Terraria
{
	public sealed class SfxInstancePool
	{
		private uint LastFramePlayed;

		private short CurrentIndex;

		private readonly short MaxInstanceMask;

		private readonly SoundEffectInstance[] Instances;

		private SoundEffectInstance CurrentInstance;

		private readonly SoundEffect Effect;

		public SfxInstancePool(ContentManager Content, string Name, int MaxInstances)
		{
			CurrentIndex = 0;
			MaxInstanceMask = (short)(MaxInstances - 1);
			Instances = new SoundEffectInstance[MaxInstances];
			Effect = Content.Load<SoundEffect>(Name);
			for (int InstanceIdx = 0; InstanceIdx < MaxInstances; InstanceIdx++)
			{
				Instances[InstanceIdx] = Effect.CreateInstance();
			}
		}

		public bool IsPlaying()
		{
			if (CurrentInstance != null)
			{
				return CurrentInstance.State == SoundState.Playing;
			}
			return false;
		}

		public void Play(double Volume, double Pan = 0.0, double Pitch = 0.0)
		{
			if (LastFramePlayed != Main.FrameCounter)
			{
				LastFramePlayed = Main.FrameCounter;
				SoundEffectInstance Instance = Instances[CurrentIndex];
				int InstanceIdx = (CurrentIndex + 1) & MaxInstanceMask;
				Instances[InstanceIdx].Stop(immediate: true);
				CurrentIndex = (short)InstanceIdx;
				Instance.Volume = (float)Volume;
				Instance.Pan = (float)Pan;
				Instance.Pitch = (float)Pitch;
				Instance.Play();
				CurrentInstance = Instance;
			}
		}

		public void Update(double Volume, double Pan = 0.0, double Pitch = 0.0)
		{
			if (CurrentInstance != null && CurrentInstance.State == SoundState.Playing)
			{
				CurrentInstance.Volume = (float)Volume;
				CurrentInstance.Pan = (float)Pan;
				CurrentInstance.Pitch = (float)Pitch;
			}
		}

		public void UpdateOrPlay(double Volume, double Pan = 0.0, double Pitch = 0.0)
		{
			if (CurrentInstance != null && CurrentInstance.State == SoundState.Playing)
			{
				CurrentInstance.Volume = (float)Volume;
				CurrentInstance.Pan = (float)Pan;
				CurrentInstance.Pitch = (float)Pitch;
			}
			else
			{
				Play(Volume, Pan, Pitch);
			}
		}
	}
}
