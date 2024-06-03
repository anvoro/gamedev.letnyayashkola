using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
	[RequireComponent(typeof(Button))]
	public class ResetLevelButton : MonoBehaviour,
		IEventReceiver<NextLaunchCooldownStartEvent>,
		IEventReceiver<NextLaunchCooldownEndEvent>
	{
		private Button _button;
		
		private void Awake()
		{
			EventBus<NextLaunchCooldownStartEvent>.Subscribe(this);
			EventBus<NextLaunchCooldownEndEvent>.Subscribe(this);
			
			this._button = this.GetComponent<Button>();
			this._button.onClick.AddListener(() =>
			{
				EventBus<ResetLevelRequestedUIEvent>.Broadcast(new ResetLevelRequestedUIEvent());
			});
		}

		public void ReceiveEvent(in NextLaunchCooldownStartEvent args)
		{
			this._button.SetInteractable(false);
		}

		public void ReceiveEvent(in NextLaunchCooldownEndEvent args)
		{
			this._button.SetInteractable(true);
		}
	}
}