using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Core;
using KodEngine.Component;
using KodEngine.KodEBase;

namespace KodEngine.Component
{
	public class MeshCollider : Core.Component
	{
		public ReferenceField<Mesh> mesh;

		UnityEngine.MeshCollider collider;

		public MeshCollider(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public MeshCollider(RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
		{
		}

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
			mesh = new ReferenceField<Mesh>();
			Engine.OnCommonUpdate += OnUpdate;
			Slot ownerSlot = (Slot)owner.Resolve();
			collider = ownerSlot.gameObject.AddComponent<UnityEngine.MeshCollider>();
		}


		public override void OnDestroy()
		{
			mesh.Destroy();
		}

		public override void OnUpdate()
		{
		}

		public override void OnChange()
		{
		}

		public void SetMesh(RefID refID)
		{
			if (refID.ResolveType().IsSubclassOf(typeof(Mesh)))
			{
				this.mesh.target = refID;
				collider.sharedMesh = ((Mesh)refID.Resolve()).meshFilter.sharedMesh;
			}
		}
	}
}