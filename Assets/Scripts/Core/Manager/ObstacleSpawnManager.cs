using Core.Config;
using Core.EventBus;
using Game.Events;
using Game.Obstacles;
using UI.ObstacleSelectionUI;
using UnityEngine;

namespace Core.Manager
{
	public class ObstacleSpawnManager : SingletonBase<ObstacleSpawnManager>,
		IEventReceiver<DraggedFromSelectionImageUIEvent>,
		IEventReceiver<EndDragObstacleEvent>
	{
		[SerializeField] private ObstacleDatabase _obstacleDatabase;
		[SerializeField] private ObstacleSelectPanel _obstacleSelectPanel;

		private MovableObstacle _currentSpawnedObstacle;

		protected override void Awake()
		{
			EventBus<DraggedFromSelectionImageUIEvent>.Subscribe(this);
			EventBus<EndDragObstacleEvent>.Subscribe(this);

			base.Awake();
		}

		private void Start()
		{
			this._obstacleSelectPanel.Init(this._obstacleDatabase.Items);
		}

		public void ReceiveEvent(in DraggedFromSelectionImageUIEvent args)
		{
			if (this._currentSpawnedObstacle != null)
			{
				return;
			}

			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos))
			{
				MovableObstacle movableObstacle = Instantiate(args.Prefab, pos, args.Prefab.transform.rotation);
				EventBus<ObstacleSpawnEvent>.Broadcast(new ObstacleSpawnEvent(movableObstacle));

				this._currentSpawnedObstacle = movableObstacle;

				this._currentSpawnedObstacle.SetTriggerMode(true);
				this._currentSpawnedObstacle.Select();
				this._currentSpawnedObstacle.BeginDrag();
			}
		}

		public void ReceiveEvent(in EndDragObstacleEvent args)
		{
			if (args.Sender != this._currentSpawnedObstacle)
			{
				return;
			}

			this._currentSpawnedObstacle.ClearSelection();

			if (this._currentSpawnedObstacle.IsOverlap)
			{
				this._currentSpawnedObstacle.Destroy();
			}
			else
			{
				this._currentSpawnedObstacle.IsPlayerPlaced = true;
				this._currentSpawnedObstacle = null;
			}
		}
	}
}