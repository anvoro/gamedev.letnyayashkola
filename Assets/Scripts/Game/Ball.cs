using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Rigidbody))]
	public class Ball : MonoBehaviour, IEventReceiver<LaunchBallEvent>
	{
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _force = 1000f;
		
		private void Awake()
		{
			this._rigidbody = this.GetComponent<Rigidbody>();
			EventBus<LaunchBallEvent>.Subscribe(this);
		}
		
		public void AddForce(Vector3 direction, float force)
		{
			this._rigidbody.AddForce(direction * force);
		}
		
		public void ReceiveEvent(in LaunchBallEvent args)
		{
			this.AddForce(this.gameObject.transform.forward, this._force);
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