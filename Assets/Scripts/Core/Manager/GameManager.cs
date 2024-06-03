﻿using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class GameManager : SingletonBase<GameManager>,
		IEventReceiver<TargetReachedEvent>,
		IEventReceiver<LaunchRequestedUIEvent>
	{
		private bool _launched;
		
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
			if (this._launched == true)
			{
				return;
			}
			
			EventBus<PrepareToLaunchEvent>.Broadcast(new PrepareToLaunchEvent());
			EventBus<LaunchBallEvent>.Broadcast(new LaunchBallEvent());

			this._launched = true;
		}
	}
}