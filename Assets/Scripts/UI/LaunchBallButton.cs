using Core.EventBus;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class LaunchBallButton : MonoBehaviour
	{
		public bool PauseOnLaunch = false;
		
		private void Awake()
		{
			this.GetComponent<Button>().onClick.AddListener(() =>
			{
				EventBus<LaunchBallEvent>.Broadcast(new LaunchBallEvent());

				if (this.PauseOnLaunch == true)
				{
					Debug.Break();
				}
			});
		}
	}
}