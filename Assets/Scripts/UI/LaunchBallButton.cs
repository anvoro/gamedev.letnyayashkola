using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class LaunchBallButton : MonoBehaviour
	{
		private void Awake()
		{
			this.GetComponent<Button>().onClick.AddListener(() =>
			{
				EventBus<LaunchRequestedUIEvent>.Broadcast(new LaunchRequestedUIEvent());
			});
		}
	}
}