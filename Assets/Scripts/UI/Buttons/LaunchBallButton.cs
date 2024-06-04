using System;
using System.Collections;
using Core.EventBus;
using Game.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
	[RequireComponent(typeof(Button))]
	public class LaunchBallButton : MonoBehaviour,
		IEventReceiver<NextLaunchCooldownStartEvent>,
		IEventReceiver<NextLaunchCooldownEndEvent>
	{
		[SerializeField] private TMP_Text _text;
		[SerializeField] private Image _fillImage;
		[SerializeField] private string _launchText = "Launch";
		[SerializeField] private string _resetText = "Reset";

#if UNITY_EDITOR
		[SerializeField] private bool _pauseOnLaunch;
#endif
		
		private Button _button;
		private Coroutine _cooldownCoroutine;

		private BroadcastType _currentBroadcastType;

		private BroadcastType CurrentBroadcastType
		{
			get => this._currentBroadcastType;
			set
			{
				this._currentBroadcastType = value;
				this._text.text = this._currentBroadcastType switch
				{
					BroadcastType.Launch => this._launchText,
					BroadcastType.Reset => this._resetText,
					_ => throw new ArgumentOutOfRangeException()
				};
			}
		}

		private void Awake()
		{
			EventBus<NextLaunchCooldownStartEvent>.Subscribe(this);
			EventBus<NextLaunchCooldownEndEvent>.Subscribe(this);

			this.CurrentBroadcastType = BroadcastType.Launch;

			this._button = this.GetComponent<Button>();
			this._button.onClick.AddListener(this.Broadcast);

			this._fillImage.gameObject.SetActive(false);
		}

		public void ReceiveEvent(in NextLaunchCooldownEndEvent args)
		{
			if (this._cooldownCoroutine != null)
			{
				this.StopCoroutine(this._cooldownCoroutine);
			}

			this.OnCooldownEnd();
		}

		public void ReceiveEvent(in NextLaunchCooldownStartEvent args)
		{
			this._cooldownCoroutine = this.StartCoroutine(this.AnimateLaunchCooldown(args.Cooldown));
		}

		private void Broadcast()
		{
#if UNITY_EDITOR
			if (this._pauseOnLaunch)
			{
				Debug.Break();
			}
#endif
			switch (this.CurrentBroadcastType)
			{
				case BroadcastType.Launch:
					this.CurrentBroadcastType = BroadcastType.Reset;
					EventBus<LaunchRequestedUIEvent>.Broadcast(new LaunchRequestedUIEvent());
					break;

				case BroadcastType.Reset:
					this.CurrentBroadcastType = BroadcastType.Launch;
					EventBus<ResetBallRequestedUIEvent>.Broadcast(new ResetBallRequestedUIEvent());
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private IEnumerator AnimateLaunchCooldown(float launchCooldown)
		{
			this._button.SetInteractable(false);

			this._fillImage.gameObject.SetActive(true);

			float startTime = Time.unscaledTime;
			float goalTime = Time.unscaledTime + launchCooldown;

			while (Time.unscaledTime < goalTime)
			{
				float currentFill = 1 - (Time.unscaledTime - startTime) / launchCooldown;
				this._fillImage.fillAmount = currentFill;

				yield return null;
			}

			this._cooldownCoroutine = null;
		}

		private void OnCooldownEnd()
		{
			this._fillImage.fillAmount = 0f;
			this._fillImage.gameObject.SetActive(false);

			this._button.SetInteractable(true);
		}

		private enum BroadcastType
		{
			Launch = 0,
			Reset = 1
		}
	}
}