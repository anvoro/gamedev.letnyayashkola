using Core.Config;
using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class ObstacleSelectionImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		[SerializeField]
		private Image _prefabImage;

		private ObstacleDatabase.DataItem _item;
		private bool _isPointerDown;
		
		public void Init(ObstacleDatabase.DataItem item)
		{
			this._item = item;

			Texture2D texture = this._item.Texture;
			Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
				new Vector2(0.5f, 0.5f), 100.0f);
			
			this._prefabImage.sprite = sprite;
		}

		
		public void OnPointerDown(PointerEventData eventData)
		{
			//Debug.Log($"{this._item.Prefab.name} is Clicked from ObstacleSelectionImage");
			this._isPointerDown = true;
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			this._isPointerDown = false;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (this._isPointerDown)
			{
				//Debug.Log($"{this._item.Prefab.name} is Dragged from ObstacleSelectionImage");
				EventBus<DraggedFromSelectionImageUIEvent>.Broadcast(new DraggedFromSelectionImageUIEvent(this._item.Prefab));
			}
		}
	}
}