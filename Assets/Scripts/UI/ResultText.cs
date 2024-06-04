using Core.EventBus;
using Game.Events;
using TMPro;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(TMP_Text))]
	public class ResultText : MonoBehaviour,
		IEventReceiver<TargetReachedEvent>,
		IEventReceiver<FailEvent>
	{
		[SerializeField] private string _winText = "WIN!";
		[SerializeField] private Color _winColor = Color.green;

		[SerializeField] private string _loseText = "LOSE :(";
		[SerializeField] private Color _loseColor = Color.red;

		private TMP_Text _text;

		private void Awake()
		{
			EventBus<TargetReachedEvent>.Subscribe(this);
			EventBus<FailEvent>.Subscribe(this);

			this._text = this.GetComponent<TMP_Text>();
			this.HideText();
		}

		public void ReceiveEvent(in FailEvent args)
		{
			this._text.text = this._loseText;
			this._text.color = this._loseColor;

			this.ShowHideTextSequence();
		}

		public void ReceiveEvent(in TargetReachedEvent args)
		{
			this._text.text = this._winText;
			this._text.color = this._winColor;

			this.ShowHideTextSequence();
		}

		private void ShowHideTextSequence()
		{
			this._text.gameObject.SetActive(true);
			this.Invoke(nameof(this.HideText), 2f);
		}

		private void HideText()
		{
			this._text.gameObject.SetActive(false);
		}
	}
}