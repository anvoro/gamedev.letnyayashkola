namespace Game.Events
{
	public readonly struct PrepareToLaunchEvent { }
	
	public readonly struct LaunchBallEvent { }
	
	public readonly struct ResetBallEvent { }

	public readonly struct NextLaunchCooldownStartEvent
	{
		public readonly float Cooldown;

		public NextLaunchCooldownStartEvent(float cooldown)
		{
			this.Cooldown = cooldown;
		}
	}

	public readonly struct NextLaunchCooldownEndEvent { }

	public readonly struct FailEvent { }
	
	public readonly struct TargetReachedEvent { }
}