using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class GameManager : SingletonBase<GameManager>,
		IEventReceiver<TargetReachedEvent>,
		IEventReceiver<LaunchRequestedUIEvent>
	{
		protected override void Awake()
		{
			EventBus<LaunchRequestedUIEvent>.Subscribe(this);
			EventBus<TargetReachedEvent>.Subscribe(this);
			
			base.Awake();
		}

		public void ReceiveEvent(in TargetReachedEvent args)
		{
			Debug.Log("GOOOOOOOOOOOOOOOOOL");
		}

		public void ReceiveEvent(in LaunchRequestedUIEvent args)
		{
			EventBus<PrepareToLaunchEvent>.Broadcast(new PrepareToLaunchEvent());
			EventBus<LaunchBallEvent>.Broadcast(new LaunchBallEvent());
		}
	}
}