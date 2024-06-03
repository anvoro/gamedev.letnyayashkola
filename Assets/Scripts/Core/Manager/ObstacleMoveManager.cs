using System.Collections.Generic;
using Core.EventBus;
using Game;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class ObstacleMoveManager : SingletonBase<ObstacleMoveManager>,
		IEventReceiver<MovableObstacleSelectedEvent>,
		IEventReceiver<MovableObstacleDragEvent>,
		IEventReceiver<PrepareToLaunchEvent>,
		IEventReceiver<EndDragMouseEvent>,
		IEventReceiver<BeginDragMouseEvent>
	{
		private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
		private readonly List<Obstacle> _obstacles = new();
		
		[SerializeField] private LayerMask _movePointDetectionMask;
		[SerializeField] private ObjectRotator _rotator;
		
		private Camera _camera;
		private MovableObstacle _currentSelectedObstacle;

		private Vector3 _initMovePosition;
		private Vector3 _mouseOffset;
		
		private bool _isSelectedDrag;

		public bool MoveAllowed { get; private set; }

		protected override void Awake()
		{
			EventBus<MovableObstacleSelectedEvent>.Subscribe(this);
			EventBus<MovableObstacleDragEvent>.Subscribe(this);
			EventBus<PrepareToLaunchEvent>.Subscribe(this);

			EventBus<EndDragMouseEvent>.Subscribe(this);
			EventBus<BeginDragMouseEvent>.Subscribe(this);

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
					if (go.TryGetComponent<Obstacle>(out Obstacle obstacle) == true)
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

			moveSelected();
			moveRotator();

			void moveSelected()
			{
				if (this._isSelectedDrag == true)
				{
					if (this.TryGetMousePosition(out Vector3 pos) == true)
					{
						this._currentSelectedObstacle.transform.position = pos + this._mouseOffset;
					}
				}
			}
            
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
				this.ClearSelection();
			}

			void enableObstacleTriggers(bool enable)
			{
				foreach (Obstacle obstacle in this._obstacles)
				{
					obstacle.SetTriggerMode(enable);
				}
			}
		}

		private void ClearSelection()
		{
			EventBus<MovableObstacleSelectedEvent>.Broadcast(new MovableObstacleSelectedEvent(null));

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
		
		public void ReceiveEvent(in PrepareToLaunchEvent args)
		{
			this.EnableMoveObstacles(false);
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
		
		public void ReceiveEvent(in BeginDragMouseEvent args)
		{
			if (this._currentSelectedObstacle == null || this._currentSelectedObstacle.InMouseFocus == false)
			{
				return;
			}
			
			if (this.TryGetMousePosition(out Vector3 pos) == true)
			{
				this._mouseOffset = this._currentSelectedObstacle.transform.position - pos;
				
				this._isSelectedDrag = true;
				EventBus<MovableObstacleDragEvent>.Broadcast(new MovableObstacleDragEvent(this._currentSelectedObstacle, true));
			}
		}

		public void ReceiveEvent(in EndDragMouseEvent args)
		{
			if (this._currentSelectedObstacle == null)
			{
				return;
			}
			
			//todo: вынести вызовы этих методов всё таки в сам класс ovableObstacle, здесь они не к месте, т.к. очень плохо, что класс кидает тот же эвент, на который сам подписан
			this._isSelectedDrag = false;
			EventBus<MovableObstacleDragEvent>.Broadcast(new MovableObstacleDragEvent(this._currentSelectedObstacle, false));
		}
	}
}