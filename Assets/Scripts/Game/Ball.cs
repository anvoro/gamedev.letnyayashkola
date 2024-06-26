using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Rigidbody))]
	public class Ball : MonoBehaviour,
		IEventReceiver<LaunchBallEvent>,
		IEventReceiver<ResetBallEvent>,
		IEventReceiver<NextLaunchCooldownEndEvent>
	{
		[SerializeField] private float _force = 1000f;

		private Rigidbody _rigidbody;

		private Vector3 _startPosition;
		private Quaternion _startRotation;

		public Vector3 Velocity
		{
			get => this._rigidbody.velocity;
			set => this._rigidbody.velocity = value;
		}

		private void Awake()
		{
			this._startPosition = this.transform.position;
			this._startRotation = this.transform.rotation;

			this._rigidbody = this.GetComponent<Rigidbody>();
			
			EventBus<LaunchBallEvent>.Subscribe(this);
			EventBus<ResetBallEvent>.Subscribe(this);
			EventBus<NextLaunchCooldownEndEvent>.Subscribe(this);
		}
		
		public void ResetRigidbody()
		{
			this._rigidbody.velocity = new Vector3(0f, 0f, 0f);
			this._rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
		}

		private void ResetTransform()
		{
			this.ResetRigidbody();

			this.transform.position = this._startPosition;
			this.transform.rotation = this._startRotation;
		}

		public void AddForce(Vector3 direction, float force)
		{
			this._rigidbody.AddForce(direction * force);
		}
		
		public void ReceiveEvent(in LaunchBallEvent args)
		{
			this.AddForce(this.gameObject.transform.forward, this._force);
		}

		public void ReceiveEvent(in ResetBallEvent args)
		{
			this.ResetTransform();
		}
		
		public void ReceiveEvent(in NextLaunchCooldownEndEvent args)
		{
			this.ResetRigidbody();
		}
		
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.gameObject.transform.position,
				this.gameObject.transform.position + this.gameObject.transform.forward * 2f);

			if (this._rigidbody == null)
			{
				return;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawLine(this.gameObject.transform.position,
				this.gameObject.transform.position + this._rigidbody.velocity.normalized * 2f);
		}
#endif

	}
}