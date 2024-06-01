using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Collider))]
	public class ObjectRotator : MonoBehaviour
	{
		private Vector3 _prevMousePosition;
		private Vector3 _mouseDelta;

		public bool IsRotating { get; private set; }

		[SerializeField] private float _sensitivity = .25f;

		public Transform ObjectToRotate { get; set; }
	
		private void Update()
		{
			if (this.IsRotating == false)
			{
				return;
			}

			this._mouseDelta = Input.mousePosition - this._prevMousePosition;
			var rotation = new Vector3(0f, -(this._mouseDelta.x) * this._sensitivity, 0f);
		
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