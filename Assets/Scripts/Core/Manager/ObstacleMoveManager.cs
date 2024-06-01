using System.Collections.Generic;
using Core.EventBus;
using Game;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class ObstacleMoveManager : SingletonBase<ObstacleMoveManager>,
		IEventReceiver<MovableObstacleSelectedEvent>,
		IEventReceiver<MovableObstacleDragEvent>
	{
		private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
		private readonly List<Obstacle> _obstacles = new();
		
		private Camera _camera;
		private MovableObstacle _currentSelectedObstacle;
		
		private Vector3 _initMovePosition;
		
		[SerializeField]
		private LayerMask _movePointDetectionMask;

		[SerializeField]
		private ObjectRotator _rotator;
		
		public bool MoveAllowed { get; private set; }
		
		protected override void Awake()
		{
			EventBus<MovableObstacleSelectedEvent>.Subscribe(this);
			EventBus<MovableObstacleDragEvent>.Subscribe(this);
			
			this._camera = Camera.main;
			this._rotator.gameObject.SetActive(false);
			
			base.Awake();
		}
		
		private void Start()
		{
			this.FindAllObstacle();
			this.EnableMove(true);
		}

		private void FindAllObstacle()
		{
			this._obstacles.Clear();
			
			foreach (var go in GameObject.FindGameObjectsWithTag("Obstacle"))
			{
				if (go.TryGetComponent<Obstacle>(out var obstacle) == true)
				{
					this._obstacles.Add(obstacle);
				}
			}
		}

		private void EnableObstacleTriggers(bool enable)
		{
			foreach (var obstacle in this._obstacles)
			{
				obstacle.SetTriggerMode(enable);
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

		public void EnableMove(bool enable)
		{
			this.MoveAllowed = enable;
			this.EnableObstacleTriggers(enable);

			if (enable == false)
			{
				this.ClearSelection();
			}
		}

		private void ClearSelection()
		{
			EventBus<MovableObstacleSelectedEvent>.Broadcast(new MovableObstacleSelectedEvent
			{
				Sender = null,
			});
			
			this._rotator.gameObject.SetActive(false);
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

		public void ReceiveEvent(in MovableObstacleSelectedEvent args)
		{
			this._currentSelectedObstacle = args.Sender;

			if (args.Sender == null)
			{
				return;
			}
            
			this._rotator.ObjectToRotate = this._currentSelectedObstacle.transform;
			this._rotator.gameObject.SetActive(true);
		}
		
		public void ReceiveEvent(in MovableObstacleDragEvent args)
		{
			if (args.IsDrag == true)
			{
				this._initMovePosition = args.Sender.transform.position;
			}
			else
			{
				if (args.Sender.IsOverlap == true)
				{
					args.Sender.transform.position = this._initMovePosition;
				}
			}
		}
	}
}