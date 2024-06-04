using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game.Obstacles
{
	public class FailZoneObstacle : Obstacle
	{
		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Ball") == false)
			{
				return;
			}

			Ball ball = other.gameObject.GetComponent<Ball>();
			ball.ResetRigidbody();

			EventBus<FailEvent>.Broadcast(new FailEvent());
		}
	}
}