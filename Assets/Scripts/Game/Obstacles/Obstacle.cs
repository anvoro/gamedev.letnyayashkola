using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game.Obstacles
{
	[RequireComponent(typeof(Collider))]
	public class Obstacle : MonoBehaviour
	{
		private Collider _collider;
		private bool _isOverlap;

		public bool IsOverlap
		{
			get => this._isOverlap;
			private set
			{
				bool needBroadcast = this._isOverlap != value;
				this._isOverlap = value;

				if (needBroadcast)
				{
					EventBus<ObstacleOverlapEvent>.Broadcast(new ObstacleOverlapEvent(this, this._isOverlap));
				}
			}
		}

		protected virtual void Awake()
		{
			this._collider = this.GetComponent<Collider>();
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Obstacle") == false)
			{
				return;
			}

			this.IsOverlap = true;
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Obstacle") == false)
			{
				return;
			}

			this.IsOverlap = false;
		}

		protected virtual void OnTriggerStay(Collider other)
		{
			if (other.gameObject.CompareTag("Obstacle") == false)
			{
				return;
			}

			this.IsOverlap = true;
		}

		public void SetTriggerMode(bool isTrigger)
		{
			this._collider.isTrigger = isTrigger;
		}
	}
}