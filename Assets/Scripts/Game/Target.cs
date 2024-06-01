using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class Target : MonoBehaviour
	{
		[SerializeField]
		private float _sqrMinGoalDistance = .25f;

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag("Ball") == false)
			{
				return;
			}

			if (Vector3.SqrMagnitude(other.transform.position - this.transform.position) <= this._sqrMinGoalDistance)
			{
				EventBus<TargetReachedEvent>.Broadcast(new TargetReachedEvent());
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(this.transform.position, this._sqrMinGoalDistance);
		}
#endif
	}
}