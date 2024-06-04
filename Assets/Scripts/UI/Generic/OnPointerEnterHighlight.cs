using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Generic
{
	[RequireComponent(typeof(Image))]
	public class OnPointerEnterHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private Color _highlightedColor;

		private Image _image;
		private Color _savedColor;

		private void Awake()
		{
			this._image = this.GetComponent<Image>();

			if (this._image.raycastTarget == false)
			{
				Debug.LogWarning(
					$"raycastTarget on '{this.gameObject.name}' is FALSE, {typeof(OnPointerEnterHighlight)} script cannot work");
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			this._savedColor = this._image.color;
			this._image.color = this._highlightedColor;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this._image.color = this._savedColor;
		}
	}
}