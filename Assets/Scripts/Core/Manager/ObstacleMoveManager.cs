using Core.EventBus;
using Game;
using Game.Events;
using UnityEngine;

namespace Core.Manager
{
	public class ObstacleMoveManager : SingletonBase<ObstacleMoveManager>, IEventReceiver<ObstacleSelectedEvent>
	{
		private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
		
		private Camera _camera;
		private MovableObstacle _currentSelectedObstacle;
		
		[SerializeField]
		private LayerMask _movePointDetectionMask;

		[SerializeField]
		private ObjectRotator _rotator;
		
		protected override void Awake()
		{
			EventBus<ObstacleSelectedEvent>.Subscribe(this);
			
			this._camera = Camera.main;
			
			this._rotator.gameObject.SetActive(false);
			
			base.Awake();
		}

		private void Update()
		{
			if (this._currentSelectedObstacle == null)
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

		public void ReceiveEvent(in ObstacleSelectedEvent args)
		{
			this._currentSelectedObstacle = args.Selected;
			
			this._rotator.ObjectToRotate = this._currentSelectedObstacle.transform;
			this._rotator.gameObject.SetActive(true);
		}
	}
}