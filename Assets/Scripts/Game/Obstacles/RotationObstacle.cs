using UnityEngine;

namespace Game.Obstacles
{
	public class RotationObstacle : Obstacle
	{
		[SerializeField] private float _rotationSpeed = .2f;

		[SerializeField] private bool _clockwise;

		private void Update()
		{
			Vector3 rotation = new Vector3(
				0f,
				this._clockwise ? this._rotationSpeed : -this._rotationSpeed,
				0f);

			this.transform.Rotate(rotation);
		}
	}
}