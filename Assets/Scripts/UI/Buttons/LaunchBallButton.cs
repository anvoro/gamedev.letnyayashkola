using System;
using System.Collections;
using Core.EventBus;
using Game.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class LaunchBallButton : MonoBehaviour
	{
		private enum BroadcastType
		{
			Launch = 0,
			Reset = 1,
		}
		
		private Button _button;

		[SerializeField]
		private TMP_Text _text;
		[SerializeField]
		private Image _fillImage;
		[SerializeField]
		private float _launchCooldown = 5f;

		[SerializeField]
		private string _launchText = "Launch";
		[SerializeField]
		private string _resetText = "Reset";

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
			this.CurrentBroadcastType = BroadcastType.Launch;
			
			this._button = this.GetComponent<Button>();
			this._button.onClick.AddListener(this.Broadcast);

			this._fillImage.gameObject.SetActive(false);
		}

		private void Broadcast()
		{
			switch (this.CurrentBroadcastType)
			{
				case BroadcastType.Launch:
					this.CurrentBroadcastType = BroadcastType.Reset;
					this.StartCoroutine(this.WaitLaunchCooldown());
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

		private IEnumerator WaitLaunchCooldown()
		{
			this._button.SetInteractable(false);
			
			this._fillImage.gameObject.SetActive(true);

			float startTime = Time.unscaledTime;
			float goalTime = Time.unscaledTime + this._launchCooldown;

			while (Time.unscaledTime < goalTime)
			{
				float currentFill = 1 - (Time.unscaledTime - startTime) / this._launchCooldown;
				this._fillImage.fillAmount = currentFill;

				yield return null;
			}

			this._fillImage.fillAmount = 0f;
			this._fillImage.gameObject.SetActive(false);
			
			this._button.SetInteractable(true);
		}
	}
}