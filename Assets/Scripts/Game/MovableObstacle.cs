using Core.EventBus;
using Core.Manager;
using Game.Events;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Collider))]
	public class MovableObstacle : MonoBehaviour
	{
		private Vector3 _mouseOffset;
		
		public void OnMouseDown()
		{
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos) == true)
			{
				this._mouseOffset = this.transform.position - pos;
			}

			EventBus<ObstacleSelectedEvent>.Broadcast(new ObstacleSelectedEvent
			{
				Selected = this,
			});
		}

		public void OnMouseDrag()
		{
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos) == true)
			{
				this.transform.position = pos + this._mouseOffset;
			}
		}
	}
}