﻿using Game.Obstacles;

namespace Game.Events
{
	public readonly struct LaunchRequestedUIEvent
	{
	}

	public readonly struct ResetBallRequestedUIEvent
	{
	}

	public readonly struct ResetLevelRequestedUIEvent
	{
	}

	public readonly struct DeleteObstacleRequestUIEvent
	{
	}

	public readonly struct DraggedFromSelectionImageUIEvent
	{
		public readonly MovableObstacle Prefab;

		public DraggedFromSelectionImageUIEvent(MovableObstacle prefab)
		{
			this.Prefab = prefab;
		}
	}
}