using System.Collections.Generic;
using Core.EventBus;
using Game;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class ObstacleMoveManager : SingletonBase<ObstacleMoveManager>,
		IEventReceiver<PrepareToLaunchEvent>,
		IEventReceiver<ObstacleSpawnEvent>,
		IEventReceiver<ObstacleDestroyEvent>,
		IEventReceiver<ObstacleSelectedEvent>,
		IEventReceiver<BeginDragObstacleEvent>,
		IEventReceiver<EndDragObstacleEvent>
	{
		private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
		private readonly List<Obstacle> _obstacles = new();
		
		[SerializeField] private LayerMask _movePointDetectionMask;
		[SerializeField] private ObjectRotator _rotator;
		
		private Camera _camera;
		private MovableObstacle _currentSelectedObstacle;

		private Vector3 _startMovePosition;
		
		public bool MoveAllowed { get; private set; }

		protected override void Awake()
		{
			EventBus<PrepareToLaunchEvent>.Subscribe(this);
			EventBus<ObstacleSpawnEvent>.Subscribe(this);
			EventBus<ObstacleDestroyEvent>.Subscribe(this);
			EventBus<ObstacleSelectedEvent>.Subscribe(this);
			EventBus<BeginDragObstacleEvent>.Subscribe(this);
			EventBus<EndDragObstacleEvent>.Subscribe(this);
			
			this._camera = Camera.main;
			this._rotator.gameObject.SetActive(false);

			base.Awake();
		}

		private void Start()
		{
			findAllObstacle();
			
			this.EnableMoveObstacles(true);

			void findAllObstacle()
			{
				this._obstacles.Clear();

				foreach (GameObject go in GameObject.FindGameObjectsWithTag("Obstacle"))
				{
					if (go.TryGetComponent(out Obstacle obstacle) == true)
					{
						this._obstacles.Add(obstacle);
					}
				}
			}
		}

		private void Update()
		{
			if (this._currentSelectedObstacle == null)
			{
				return;
			}
			
			moveRotator();
			
			void moveRotator()
			{
				if (this._rotator.gameObject.activeSelf == false)
				{
					return;
				}

				Vector3 position = this._currentSelectedObstacle.transform.position;
				this._rotator.transform.position =
					new Vector3(
						position.x,
						this._rotator.transform.position.y,
						position.z);
			}
		}

		private void EnableMoveObstacles(bool enable)
		{
			this.MoveAllowed = enable;
			
			enableObstacleTriggers(enable);

			if (enable == false)
			{
				this._currentSelectedObstacle?.ClearSelection();
			}

			void enableObstacleTriggers(bool enable)
			{
				foreach (Obstacle obstacle in this._obstacles)
				{
					obstacle.SetTriggerMode(enable);
				}
			}
		}

		public bool TryGetMousePosition(out Vector3 pos)
		{
			Ray ray = this._camera.ScreenPointToRay(Input.mousePosition);
			int count = Physics.RaycastNonAlloc(ray, this._raycastHits, 1000f, this._movePointDetectionMask);

			if (count > 0)
			{
				pos = this._raycastHits[0].point;
				return true;
			}

			pos = Vector3.zero;
			return false;
		}
		
		public void ReceiveEvent(in PrepareToLaunchEvent args)
		{
			this.EnableMoveObstacles(false);
		}

		public void ReceiveEvent(in ObstacleSelectedEvent args)
		{
			this._currentSelectedObstacle = args.Sender;

			if (this._currentSelectedObstacle == null)
			{
				this._rotator.gameObject.SetActive(false);
				return;
			}

			this._rotator.ObjectToRotate = this._currentSelectedObstacle.transform;
			this._rotator.gameObject.SetActive(true);
		}
		
		public void ReceiveEvent(in BeginDragObstacleEvent args)
		{
			this._startMovePosition = this._currentSelectedObstacle.transform.position;
		}

		public void ReceiveEvent(in EndDragObstacleEvent args)
		{
			if (this._currentSelectedObstacle.IsOverlap == true)
			{
				this._currentSelectedObstacle.transform.position = this._startMovePosition;
			}
		}

		public void ReceiveEvent(in ObstacleSpawnEvent args)
		{
			this._obstacles.Add(args.Sender);
		}

		public void ReceiveEvent(in ObstacleDestroyEvent args)
		{
			this._obstacles.Remove(args.Sender);
		}
	}
}