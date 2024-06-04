using Core.Manager;
using UnityEngine;

namespace Game.Obstacles
{
	public class SlowField : MovableObstacle
	{
		[SerializeField] private float _slowFactor = .4f;

		protected override void OnTriggerEnter(Collider other)
		{
			base.OnTriggerEnter(other);

			if (ObstacleMoveManager.I.MoveAllowed)
			{
				return;
			}

			if (other.gameObject.CompareTag("Ball") == false)
			{
				return;
			}

			//todo: заменить на метод (скрипт), который возвращает Ball без использования GetComponentInParent
			Ball ball = other.gameObject.GetComponentInParent<Ball>();
			ball.Velocity *= this._slowFactor;
		}
	}
}