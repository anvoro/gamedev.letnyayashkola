using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Core.Manager.InputControllers
{
	public class MouseController : SingletonBase<MouseController>
	{
		[SerializeField] private float _dragDetectionDelay = .2f;

		private float _currentDragTime;
		private bool _isMouseDrag;

		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (this._isMouseDrag)
				{
					this._isMouseDrag = false;
					this._currentDragTime = 0;
					
					EventBus<EndDragMouseEvent>.Broadcast(new EndDragMouseEvent());
				}
			}

			if (Input.GetMouseButton(0))
			{
				this._currentDragTime += Time.deltaTime;

				if (this._isMouseDrag == false && this._currentDragTime >= this._dragDetectionDelay)
				{
					this._isMouseDrag = true;

					EventBus<BeginDragMouseEvent>.Broadcast(new BeginDragMouseEvent());
				}
			}
		}
	}
}