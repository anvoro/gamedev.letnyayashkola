using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game.Obstacles
{
	public class TeleportExit : MovableObstacle,
		IEventReceiver<BallTeleportEvent>
	{
		[SerializeField]
		private Transform _teleportPoint;
		
		protected override void Awake()
		{
			EventBus<BallTeleportEvent>.Subscribe(this);
			
			base.Awake();
		}

		public void ReceiveEvent(in BallTeleportEvent args)
		{
			Ball ball = args.Ball;
			Vector3 enterVelocity = ball.Velocity;
			
			ball.ResetRigidbody();

			ball.transform.position = new Vector3(
				this._teleportPoint.position.x,
				ball.transform.position.y,
				this._teleportPoint.position.z);
			
			ball.Velocity = this.transform.forward * enterVelocity.magnitude;
		}
	}
}