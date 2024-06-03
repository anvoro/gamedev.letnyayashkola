using Core.Config;
using Core.EventBus;
using Game;
using Game.Events;
using UI;
using UnityEngine;

namespace Core.Manager
{
	public class ObstaclePlacementManager : SingletonBase<ObstaclePlacementManager>,
		IEventReceiver<DraggedFromSelectionImageUIEvent>,
		IEventReceiver<MovableObstacleDragEvent>
	{
		[SerializeField]
		private ObstacleDatabase _obstacleDatabase;

		[SerializeField]
		private ObstacleSelectPanel _obstacleSelectPanel;

		protected override void Awake()
		{
			EventBus<DraggedFromSelectionImageUIEvent>.Subscribe(this);
			EventBus<MovableObstacleDragEvent>.Subscribe(this);
			
			base.Awake();
		}

		private void Start()
		{
			this._obstacleSelectPanel.Init(this._obstacleDatabase.Items);
		}

		public void ReceiveEvent(in DraggedFromSelectionImageUIEvent args)
		{
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos) == true)
			{
				MovableObstacle movableObstacle = Instantiate(args.Prefab, pos, args.Prefab.transform.rotation);
				
				EventBus<MovableObstacleSelectedEvent>.Broadcast(new MovableObstacleSelectedEvent(movableObstacle));
				EventBus<MovableObstacleDragEvent>.Broadcast(new MovableObstacleDragEvent(movableObstacle, true));
			}
		}
		
		public void ReceiveEvent(in MovableObstacleDragEvent args)
		{
			if (args.IsDrag == false)
			{
				if (args.Sender.IsOverlap == true)
				{
					Destroy(args.Sender.gameObject);
				}
				else
				{
					//todo: нужно добавлять обстакл в общий список обстаклов, при его размещении
				}
			}
		}
	}
}