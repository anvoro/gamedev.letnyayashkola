using System;
using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game.Views
{
	[RequireComponent(typeof(Renderer))]
	[RequireComponent(typeof(MovableObstacle))]
	public class MovableObstacleView : MonoBehaviour,
		IEventReceiver<ObstacleSelectedEvent>,
		IEventReceiver<ObstacleOverlapEvent>
	{
		private MovableObstacle _parent;
		private Renderer _renderer;
		
		[SerializeField]
		private Material _idleMaterial;
		[SerializeField]
		private Material _selectedMaterial;
		[SerializeField]
		private Material _collidedMaterial;

		private bool _isShowCollided;
		private Material _materialBeforeCollision;
		
		private void Awake()
		{
			EventBus<ObstacleSelectedEvent>.Subscribe(this);
			EventBus<ObstacleOverlapEvent>.Subscribe(this);
			
			this._parent = this.GetComponent<MovableObstacle>();
			this._renderer = this.GetComponent<Renderer>();
		}

		private void OnDestroy()
		{
			EventBus<ObstacleSelectedEvent>.Unsubscribe(this);
			EventBus<ObstacleOverlapEvent>.Unsubscribe(this);
		}

		public void ReceiveEvent(in ObstacleSelectedEvent args)
		{
			this._renderer.material = args.Sender == this._parent ? this._selectedMaterial : this._idleMaterial;
		}

		public void ReceiveEvent(in ObstacleOverlapEvent args)
		{
			if (args.Sender != this._parent)
			{
				return;
			}

			if (args.IsOverlap == true)
			{
				this._materialBeforeCollision = this._renderer.material;
				this._renderer.material = this._collidedMaterial;
			}
			else
			{
				this._renderer.material = this._materialBeforeCollision;
			}
		}
	}
}