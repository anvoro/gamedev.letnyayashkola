namespace Game.Events
{
	public readonly struct LaunchRequestedUIEvent { }
	
	public readonly struct LaunchBallEvent { }
	
	public readonly struct TargetReachedEvent { }

	public readonly struct MovableObstacleSelectedEvent
	{
		public readonly MovableObstacle Sender;

		public MovableObstacleSelectedEvent(MovableObstacle sender)
		{
			this.Sender = sender;
		}
	}
	
	public readonly struct MovableObstacleDragEvent
	{
		public readonly MovableObstacle Sender;
		public readonly bool IsDrag;

		public MovableObstacleDragEvent(MovableObstacle sender, bool isDrag)
		{
			this.Sender = sender;
			this.IsDrag = isDrag;
		}
	}
	
	public readonly struct ObstacleOverlapEvent
	{
		public readonly Obstacle Sender;
		public readonly bool IsOverlap;

		public ObstacleOverlapEvent(Obstacle sender, bool isOverlap)
		{
			this.Sender = sender;
			this.IsOverlap = isOverlap;
		}
	}
}