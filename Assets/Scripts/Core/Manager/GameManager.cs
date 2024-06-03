using System.Collections;
using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class GameManager : SingletonBase<GameManager>,
		IEventReceiver<TargetReachedEvent>,
		IEventReceiver<FailEvent>,
		IEventReceiver<LaunchRequestedUIEvent>,
		IEventReceiver<ResetBallRequestedUIEvent>
	{
		[SerializeField]
		private float _nextLaunchCooldown = 5f;

		private Coroutine _cooldownCoroutine;
		private bool _cooldownEndBroadcasted = false;
		
		protected override void Awake()
		{
			EventBus<LaunchRequestedUIEvent>.Subscribe(this);
			EventBus<ResetBallRequestedUIEvent>.Subscribe(this);
			EventBus<TargetReachedEvent>.Subscribe(this);
			EventBus<FailEvent>.Subscribe(this);
			
			base.Awake();
		}
		
		private IEnumerator WaitLaunchCooldown()
		{
			EventBus<NextLaunchCooldownStartEvent>.Broadcast(new NextLaunchCooldownStartEvent(this._nextLaunchCooldown));
			
			this._cooldownEndBroadcasted = false;
			
			float goalTime = Time.unscaledTime + this._nextLaunchCooldown;
			while (Time.unscaledTime < goalTime)
			{
				yield return null;
			}
			
			this._cooldownCoroutine = null;
			this.BroadcastCooldownEnd();
		}
		
		private void BroadcastCooldownEnd()
		{
			if (this._cooldownEndBroadcasted == false)
			{
				this._cooldownEndBroadcasted = true;
				EventBus<NextLaunchCooldownEndEvent>.Broadcast(new NextLaunchCooldownEndEvent());
			}
		}

		private void ForceEndCooldown()
		{
			if (this._cooldownCoroutine != null)
			{
				this.StopCoroutine(this._cooldownCoroutine);
			}
			
			this.BroadcastCooldownEnd();
		}
		
		public void ReceiveEvent(in TargetReachedEvent args)
		{
			this.ForceEndCooldown();
			Debug.Log("GOOOOOOOOOOOOOOOOOL");
		}
		
		public void ReceiveEvent(in FailEvent args)
		{
			this.ForceEndCooldown();
			Debug.Log("FAIL");
		}

		public void ReceiveEvent(in LaunchRequestedUIEvent args)
		{
			EventBus<PrepareToLaunchEvent>.Broadcast(new PrepareToLaunchEvent());
			EventBus<LaunchBallEvent>.Broadcast(new LaunchBallEvent());

			this._cooldownCoroutine = this.StartCoroutine(this.WaitLaunchCooldown());
		}

		public void ReceiveEvent(in ResetBallRequestedUIEvent args)
		{
			EventBus<ResetBallEvent>.Broadcast(new ResetBallEvent());
		}
	}
}