using System;
using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game
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
				
				if (needBroadcast == true)
				{
					EventBus<ObstacleOverlapEvent>.Broadcast(new ObstacleOverlapEvent
					{
						Sender = this,
						IsOverlap = this._isOverlap,
					});
				}
			}
		}
		
		protected virtual void Awake()
		{
			this._collider = this.GetComponent<Collider>();
		}

		public void SetTriggerMode(bool isTrigger)
		{
			this._collider.isTrigger = isTrigger;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Obstacle") == false)
			{
				return;
			}

			this.IsOverlap = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Obstacle") == false)
			{
				return;
			}
			
			this.IsOverlap = false;
		}
	}
}