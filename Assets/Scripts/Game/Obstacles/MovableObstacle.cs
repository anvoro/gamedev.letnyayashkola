using System;
using Core.EventBus;
using Core.Manager;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class MovableObstacle : Obstacle,
		IEventReceiver<EndDragMouseEvent>,
		IEventReceiver<BeginDragMouseEvent>
	{
		private Vector3 _mouseDragOffset;
		private bool _isDrag;

		private bool _inMouseFocus;

		protected override void Awake()
		{
			EventBus<EndDragMouseEvent>.Subscribe(this);
			EventBus<BeginDragMouseEvent>.Subscribe(this);
			
			base.Awake();
		}

		private void OnDestroy()
		{
			EventBus<EndDragMouseEvent>.Unsubscribe(this);
			EventBus<BeginDragMouseEvent>.Unsubscribe(this);
		}

		private void Update()
		{
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
			this._inMouseFocus = true;
		}

		private void OnMouseExit()
		{
			this._inMouseFocus = false;
		}

		public void BeginDrag()
		{
			if (ObstacleMoveManager.I.TryGetMousePosition(out Vector3 offset) == true)
			{
				this._mouseDragOffset = this.transform.position - offset;
				this._isDrag = true;
				
				EventBus<BeginDragObstacleEvent>.Broadcast(new BeginDragObstacleEvent(this));
			}
		}
		
		public void ReceiveEvent(in BeginDragMouseEvent args)
		{
			if (this._inMouseFocus == false)
			{
				return;
			}
			
			this.BeginDrag();
		}

		public void ReceiveEvent(in EndDragMouseEvent args)
		{
			if (this._isDrag == false)
			{
				return;
			}
			
			this._isDrag = false;
			EventBus<EndDragObstacleEvent>.Broadcast(new EndDragObstacleEvent(this));
		}
	}
}