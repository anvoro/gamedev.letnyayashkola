namespace Game.Events
{
	public readonly struct MovableObstacleSelectedEvent
	{
		public readonly MovableObstacle Sender;

		public MovableObstacleSelectedEvent(MovableObstacle sender)
		{
			this.Sender = sender;
		}
	}
	
	//todo: всё таки разделить на 2 эвента, для чистоты кода
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