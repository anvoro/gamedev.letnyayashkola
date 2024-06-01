using Core.EventBus;
using Core.Manager;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class MovableObstacle : Obstacle
	{
		private Vector3 _mouseOffset;
		private bool _isDrag;
		
		private void OnMouseDown()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos) == true)
			{
				this._mouseOffset = this.transform.position - pos;
			}

			EventBus<MovableObstacleSelectedEvent>.Broadcast(new MovableObstacleSelectedEvent
			{
				Sender = this,
			});
		}
		
		private void OnMouseUp()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			if (this._isDrag == true)
			{
				this._isDrag = false;
				
				EventBus<MovableObstacleDragEvent>.Broadcast(new MovableObstacleDragEvent
				{
					Sender = this,
					IsDrag = false,
				});
			}
		}

		private void OnMouseDrag()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
            
			if (this._isDrag == false)
			{
				this._isDrag = true;
				
				EventBus<MovableObstacleDragEvent>.Broadcast(new MovableObstacleDragEvent
				{
					Sender = this,
					IsDrag = true,
				});
			}
			
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos) == true)
			{
				this.transform.position = pos + this._mouseOffset;
			}
		}
	}
}