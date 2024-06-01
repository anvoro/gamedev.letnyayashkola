using Core.EventBus;
using Game.Events;
using UnityEngine;

namespace Game.Views
{
	[RequireComponent(typeof(Renderer))]
	[RequireComponent(typeof(MovableObstacle))]
	public class MovableObstacleView : MonoBehaviour, IEventReceiver<ObstacleSelectedEvent>
	{
		private MovableObstacle _parent;
		private Renderer _renderer;
		
		[SerializeField]
		private Material _idleMaterial;
		[SerializeField]
		private Material _selectedMaterial;
        
		private void Awake()
		{
			EventBus<ObstacleSelectedEvent>.Subscribe(this);
			
			this._parent = this.GetComponent<MovableObstacle>();
			this._renderer = this.GetComponent<Renderer>();
		}

		public void ReceiveEvent(in ObstacleSelectedEvent args)
		{
			this._renderer.material = args.Selected == this._parent ? this._selectedMaterial : this._idleMaterial;
		}
	}
}