using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game.Views
{
	[RequireComponent(typeof(Renderer))]
	[RequireComponent(typeof(MovableObstacle))]
	public class MovableObstacleView : MonoBehaviour, IEventReceiver<MovableObstacleSelectedEvent>, IEventReceiver<ObstacleOverlapEvent>
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
			EventBus<MovableObstacleSelectedEvent>.Subscribe(this);
			EventBus<ObstacleOverlapEvent>.Subscribe(this);
			
			this._parent = this.GetComponent<MovableObstacle>();
			this._renderer = this.GetComponent<Renderer>();
		}

		public void ReceiveEvent(in MovableObstacleSelectedEvent args)
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