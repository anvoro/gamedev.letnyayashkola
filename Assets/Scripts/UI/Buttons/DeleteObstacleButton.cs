using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
	[RequireComponent(typeof(Button))]
	public class DeleteObstacleButton : MonoBehaviour,
		IEventReceiver<ObstacleSelectedEvent>
	{
		private Button _button;

		private void Awake()
		{
			this._button = this.GetComponent<Button>();
			this._button.SetInteractable(false);

			this._button.onClick.AddListener(() =>
			{
				this._button.SetInteractable(false);
				EventBus<DeleteObstacleRequestUIEvent>.Broadcast(new DeleteObstacleRequestUIEvent());
			});

			EventBus<ObstacleSelectedEvent>.Subscribe(this);
		}

		public void ReceiveEvent(in ObstacleSelectedEvent args)
		{
			this._button.SetInteractable(args.Sender != null && args.Sender.IsPlayerPlaced);
		}
	}
}