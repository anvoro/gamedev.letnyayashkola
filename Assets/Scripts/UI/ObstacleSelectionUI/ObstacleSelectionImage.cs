using Core.Config;
using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ObstacleSelectionUI
{
	[RequireComponent(typeof(Image))]
	public class ObstacleSelectionImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,
		IEventReceiver<ObstacleDestroyEvent>
	{
		[SerializeField] private Image _prefabImage;

		private bool _isPointerDown;

		private ObstacleDatabase.DataItem _item;

		private Image _raycastImage;

		private int _spawnedObstacles;

		private int SpawnedObstacles
		{
			get => this._spawnedObstacles;
			set
			{
				this._spawnedObstacles = value;
				this._raycastImage.raycastTarget = this._spawnedObstacles < this._item.MaxPlacedCount;
			}
		}

		private void Awake()
		{
			this._raycastImage = this.GetComponent<Image>();

			EventBus<ObstacleDestroyEvent>.Subscribe(this);
		}

		public void ReceiveEvent(in ObstacleDestroyEvent args)
		{
			if (args.Sender.Id == this._item.Prefab.Id)
			{
				this.SpawnedObstacles--;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			this._isPointerDown = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (this._isPointerDown)
			{
				this.SpawnedObstacles++;
				EventBus<DraggedFromSelectionImageUIEvent>.Broadcast(
					new DraggedFromSelectionImageUIEvent(this._item.Prefab));
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this._isPointerDown = false;
		}

		public void Init(ObstacleDatabase.DataItem item)
		{
			this._item = item;

			Texture2D texture = this._item.Texture;
			Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
				new Vector2(0.5f, 0.5f), 100.0f);

			this._prefabImage.sprite = sprite;
		}
	}
}