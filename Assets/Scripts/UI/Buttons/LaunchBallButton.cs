using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class LaunchBallButton : MonoBehaviour
	{
		private Button _button;
		
		private void Awake()
		{
			this._button = this.GetComponent<Button>();
			
			this._button.onClick.AddListener(() =>
			{
				this._button.SetInteractable(false);
				EventBus<LaunchRequestedUIEvent>.Broadcast(new LaunchRequestedUIEvent());
			});
		}
	}
}