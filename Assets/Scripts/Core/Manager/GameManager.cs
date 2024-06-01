using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class GameManager : SingletonBase<GameManager>, IEventReceiver<TargetReachedEvent>
	{
		protected override void Awake()
		{
			EventBus<TargetReachedEvent>.Subscribe(this);
			
			base.Awake();
		}

		public void ReceiveEvent(in TargetReachedEvent args)
		{
			Debug.Log("GOOOOOOOOOOOOOOOOOL");
		}
	}
}