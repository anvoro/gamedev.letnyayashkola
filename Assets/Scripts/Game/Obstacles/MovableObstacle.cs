using Core.EventBus;
using Core.Manager;
using Game.Events;
using UnityEngine;

namespace Game.Obstacles
{
	public class MovableObstacle : Obstacle,
		IEventReceiver<EndDragMouseEvent>,
		IEventReceiver<BeginDragMouseEvent>,
		IEventReceiver<ResetLevelRequestedUIEvent>
	{
		private Vector3 _startPosition;
		private Quaternion _startRotation;
		
		private Vector3 _mouseDragOffset;
		private bool _isDrag;

		private bool _inMouseFocus;
		
		public bool IsPlayerPlaced { get; set; }
		
		protected override void Awake()
		{
			this._startPosition = this.transform.position;
			this._startRotation = this.transform.rotation;
			
			EventBus<EndDragMouseEvent>.Subscribe(this);
			EventBus<BeginDragMouseEvent>.Subscribe(this);
			EventBus<ResetLevelRequestedUIEvent>.Subscribe(this);
			
			base.Awake();
		}

		private void OnDestroy()
		{
			EventBus<EndDragMouseEvent>.Unsubscribe(this);
			EventBus<BeginDragMouseEvent>.Unsubscribe(this);
			EventBus<ResetLevelRequestedUIEvent>.Unsubscribe(this);
		}

		private void Update()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			tryApplyDrag();
			
			void tryApplyDrag()
			{
				if (this._isDrag == true)
				{
					if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 pos) == true)
					{
						this.transform.position = pos + this._mouseDragOffset;
					}
				}
			}
		}

		public void Select()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			EventBus<ObstacleSelectedEvent>.Broadcast(new ObstacleSelectedEvent(this));
		}
		
		public void ClearSelection()
		{
			EventBus<ObstacleSelectedEvent>.Broadcast(new ObstacleSelectedEvent(null));
		}

		public void Destroy()
		{
			EventBus<ObstacleDestroyEvent>.Broadcast(new ObstacleDestroyEvent(this));
			GameObject.Destroy(this.gameObject);
		}
		
		private void OnMouseDown()
		{
			this.Select();
		}

		private void OnMouseEnter()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			this._inMouseFocus = true;
		}

		private void OnMouseExit()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
            
			this._inMouseFocus = false;
		}

		public void BeginDrag()
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 offset) == true)
			{
				this._mouseDragOffset = this.transform.position - offset;
				this._isDrag = true;
				
				EventBus<BeginDragObstacleEvent>.Broadcast(new BeginDragObstacleEvent(this));
			}
		}
		
		public void ReceiveEvent(in BeginDragMouseEvent args)
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
			
			if (this._inMouseFocus == false)
			{
				return;
			}
			
			this.BeginDrag();
		}

		public void ReceiveEvent(in EndDragMouseEvent args)
		{
			if (ObstacleMoveManager.I.MoveAllowed == false)
			{
				return;
			}
            
			if (this._isDrag == false)
			{
				return;
			}
			
			this._isDrag = false;
			EventBus<EndDragObstacleEvent>.Broadcast(new EndDragObstacleEvent(this));
		}
		
		private void ResetTransform()
		{
			this.transform.position = this._startPosition;
			this.transform.rotation = this._startRotation;
		}

		public void ReceiveEvent(in ResetLevelRequestedUIEvent args)
		{
			this.ResetTransform();
		}
	}
}