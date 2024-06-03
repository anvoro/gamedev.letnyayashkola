using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Core.Manager.InputControllers
{
	public class MouseController : SingletonBase<MouseController>
	{
		[SerializeField]
		private float _dragDetectionDelay = .2f;
		
		private float _currentDragTime;
		private bool _isMouseDrag;
		
		private void Update()
		{
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				//Debug.Log("GetMouseButtonDown");
			}
			
			if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				//Debug.Log("GetMouseButtonUp");

				if (this._isMouseDrag == true)
				{
					this._isMouseDrag = false;
					this._currentDragTime = 0;
					
					//Debug.Log("EndDrag");
					EventBus<EndDragMouseEvent>.Broadcast(new EndDragMouseEvent());
				}
			}
			
			if (UnityEngine.Input.GetMouseButton(0))
			{
				//Debug.Log("GetMouseButton");
				
				this._currentDragTime += Time.deltaTime;

				if (this._isMouseDrag == false && this._currentDragTime >= this._dragDetectionDelay)
				{
					this._isMouseDrag = true;
					
					//Debug.Log("BeginDrag");
					EventBus<BeginDragMouseEvent>.Broadcast(new BeginDragMouseEvent());
				}
			}
		}
	}
}