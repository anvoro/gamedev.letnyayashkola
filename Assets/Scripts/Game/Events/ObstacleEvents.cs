using Game.Obstacles;

namespace Game.Events
{
	public readonly struct ObstacleSpawnEvent
	{
		public readonly MovableObstacle Sender;

		public ObstacleSpawnEvent(MovableObstacle sender)
		{
			this.Sender = sender;
		}
	}

	public readonly struct ObstacleDestroyEvent
	{
		public readonly MovableObstacle Sender;

		public ObstacleDestroyEvent(MovableObstacle sender)
		{
			this.Sender = sender;
		}
	}

	public readonly struct ObstacleSelectedEvent
	{
		public readonly MovableObstacle Sender;
		public readonly bool NeedRotate;

		public ObstacleSelectedEvent(MovableObstacle sender, bool needRotate)
		{
			this.Sender = sender;
			this.NeedRotate = needRotate;
		}
	}

	public readonly struct BeginDragObstacleEvent
	{
		public readonly MovableObstacle Sender;

		public BeginDragObstacleEvent(MovableObstacle sender)
		{
			this.Sender = sender;
		}
	}

	public readonly struct EndDragObstacleEvent
	{
		public readonly MovableObstacle Sender;

		public EndDragObstacleEvent(MovableObstacle sender)
		{
			this.Sender = sender;
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