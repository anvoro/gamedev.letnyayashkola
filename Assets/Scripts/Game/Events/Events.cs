namespace Game.Events
{
	public struct LaunchRequestedUIEvent { }
	
	public struct LaunchBallEvent { }
	
	public struct TargetReachedEvent { }

	public struct MovableObstacleSelectedEvent
	{
		public MovableObstacle Sender;
	}
	
	public struct MovableObstacleDragEvent
	{
		public MovableObstacle Sender;
		public bool IsDrag;
	}
	
	public struct ObstacleOverlapEvent
	{
		public Obstacle Sender;
		public bool IsOverlap;
	}
}