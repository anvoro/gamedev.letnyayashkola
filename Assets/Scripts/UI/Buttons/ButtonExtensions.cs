﻿using UnityEngine.UI;

namespace UI.Buttons
{
	public static class ButtonExtensions
	{
		public static void SetInteractable(this Button button, bool interactable)
		{
			button.interactable = interactable;
		}
	}
}