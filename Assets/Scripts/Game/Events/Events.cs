using UnityEngine;

namespace Game.Events
{
	public struct LaunchBallEvent { }
	
	public struct TargetReachedEvent { }

	public struct ObstacleSelectedEvent
	{
		public MovableObstacle Selected;
	}
}