using System;
using Core.EventBus;
using Core.Manager;
using Game.Events;

namespace Game
{
	public class MovableObstacle : Obstacle
	{
		public bool InMouseFocus { get; private set; }
        
		private void OnMouseDown()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			EventBus<MovableObstacleSelectedEvent>.Broadcast(new MovableObstacleSelectedEvent(this));
		}

		private void OnMouseEnter()
		{
			this.InMouseFocus = true;
		}

		private void OnMouseExit()
		{
			this.InMouseFocus = false;
		}
	}
}