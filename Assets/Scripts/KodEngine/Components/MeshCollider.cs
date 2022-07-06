using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Core;
using KodEngine.Component;

namespace KodEngine.Component
{
	public class MeshCollider : Core.Component
	{
		private Mesh _mesh;
		public Mesh mesh
		{
			get
			{
				return _mesh;
			}
			set
			{
				_mesh = value;
				OnChange();
			}
		}

		UnityEngine.MeshCollider collider;

		public override string helpText
		{
			get
			{
				return "The MeshCollider component will create a collider centered on the attached slot with a mesh provided to it. This collider can be used for a multitude" +
					" of different functions, such as physics, interactable objects, raycast targets, or to detect intersection with other colliders.";
			}
			set
			{
			}
		}

		public override void OnAttach()
		{
			Engine.OnCommonUpdate += OnUpdate;
			collider = owner.gameObject.AddComponent<UnityEngine.MeshCollider>();
		}


		public override void OnDestroy()
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnChange()
		{
			if (mesh != null)
			{
				collider.sharedMesh = mesh.meshFilter.mesh;
			}
		}
	}
}