using System;
using UnityEngine;

namespace Game.Views
{
	[RequireComponent(typeof(Renderer))]
	[RequireComponent(typeof(ObjectRotator))]
	public class ObjectRotatorView : MonoBehaviour
	{
		private ObjectRotator _parent;
		private Renderer _renderer;
		
		[SerializeField]
		private Material _idleMaterial;
		[SerializeField]
		private Material _selectedMaterial;

		private bool _isActive;

		private bool IsActive
		{
			get => this._isActive;
			set
			{
				this._isActive = value;
				this._renderer.material = this._isActive == true ? this._selectedMaterial : this._idleMaterial;
			}
		}

		private void Awake()
		{
			this._parent = this.GetComponent<ObjectRotator>();
			this._renderer = this.GetComponent<Renderer>();
		}

		private void Update()
		{
			if (this._parent.IsRotating == true && this.IsActive == false)
			{
				this.IsActive = true;
			}
		}

		private void OnMouseEnter()
		{
			this.IsActive = true;
		}
		
		private void OnMouseExit()
		{
			this.IsActive = false;
		}

		private void OnMouseUp()
		{
			this.IsActive = false;
		}
	}
}