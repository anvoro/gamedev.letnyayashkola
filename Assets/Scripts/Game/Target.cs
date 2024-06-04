using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class Target : MonoBehaviour
	{
		[SerializeField] private float _sqrMinGoalDistance = .25f;

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(this.transform.position, this._sqrMinGoalDistance);
		}
#endif

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag("Ball") == false)
			{
				return;
			}

			if (Vector3.SqrMagnitude(other.transform.position - this.transform.position) <= this._sqrMinGoalDistance)
			{
				//todo: заменить на метод (скрипт), который возвращает Ball без использования GetComponentInParent
				Ball ball = other.gameObject.GetComponentInParent<Ball>();
				ball.ResetRigidbody();

				EventBus<TargetReachedEvent>.Broadcast(new TargetReachedEvent());
			}
		}
	}
}