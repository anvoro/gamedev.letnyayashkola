using Core.EventBus;
using Core.Manager;
using Game.Events;
using UnityEngine;

namespace Game.Obstacles
{
	public class TeleportEnter : MovableObstacle
	{
		protected override void OnTriggerEnter(Collider other)
		{
			base.OnTriggerEnter(other);

			if (ObstacleMoveManager.I.MoveAllowed == true)
			{
				return;
			}
			
			if (other.gameObject.CompareTag("Ball") == false)
			{
				return;
			}

			//todo: заменить на метод (скрипт), который возвращает Ball без использования GetComponentInParent
			var ball = other.gameObject.GetComponentInParent<Ball>();
			
			EventBus<BallTeleportEvent>.Broadcast(new BallTeleportEvent(ball));
		}
	}
}