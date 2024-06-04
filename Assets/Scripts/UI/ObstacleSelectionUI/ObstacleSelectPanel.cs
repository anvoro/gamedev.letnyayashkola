using System.Collections.Generic;
using Core.Config;
using UnityEngine;

namespace UI.ObstacleSelectionUI
{
	public class ObstacleSelectPanel : MonoBehaviour
	{
		[SerializeField] private Transform _imagesParent;

		private readonly List<ObstacleSelectionImage> _images = new();
		private readonly Stack<ObstacleSelectionImage> _inactiveImages = new();

		private void Awake()
		{
			ObstacleSelectionImage[] images = this._imagesParent.GetComponentsInChildren<ObstacleSelectionImage>();
			this._images.AddRange(images);
		}

		public void Init(IReadOnlyList<ObstacleDatabase.DataItem> items)
		{
			int itemsToShowCount = items.Count;
			int imagesCount = this._images.Count;

			if (itemsToShowCount > imagesCount)
			{
				int delta = itemsToShowCount - imagesCount;
				for (int i = 0; i < delta; i++)
				{
					if (this._inactiveImages.Count > 0)
					{
						ObstacleSelectionImage newImage = this._inactiveImages.Pop();
						newImage.gameObject.SetActive(true);
					}
					else
					{
						ObstacleSelectionImage newImage = Instantiate(this._images[0], this._imagesParent);
						this._images.Add(newImage);
					}
				}
			}
			else if (itemsToShowCount < imagesCount)
			{
				int delta = imagesCount - itemsToShowCount;
				for (int i = 0; i < delta; i++)
				{
					ObstacleSelectionImage image = this._images[imagesCount - 1 - i];
					image.gameObject.SetActive(false);
					this._inactiveImages.Push(image);
				}
			}

			for (int i = 0; i < items.Count; i++)
			{
				ObstacleDatabase.DataItem item = items[i];
				this._images[i].Init(item);
			}
		}
	}
}