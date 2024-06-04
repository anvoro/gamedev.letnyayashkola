using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Collider))]
	public class ObjectRotator : MonoBehaviour
	{
		[SerializeField] private float _sensitivity = .25f;
		private Vector3 _mouseDelta;
		private Vector3 _prevMousePosition;

		public bool IsRotating { get; private set; }

		public Transform ObjectToRotate { get; set; }

		private void Update()
		{
			if (this.IsRotating == false)
			{
				return;
			}

			this._mouseDelta = Input.mousePosition - this._prevMousePosition;
			Vector3 rotation = new Vector3(0f, -this._mouseDelta.x * this._sensitivity, 0f);

			this.ObjectToRotate.Rotate(rotation);

			this._prevMousePosition = Input.mousePosition;
		}

		private void OnMouseDown()
		{
			this.IsRotating = true;
			this._prevMousePosition = Input.mousePosition;
		}

		private void OnMouseUp()
		{
			this.IsRotating = false;
		}
	}
}