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

		public RefID meshField;
		[Newtonsoft.Json.JsonIgnore]
		private ReferenceField<Mesh> _mesh;

		UnityEngine.MeshCollider collider;

		public MeshCollider(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public MeshCollider(RefID refID, RefID meshField, RefID owner, bool isEnabled, int updateOrder) : base(refID, owner, isEnabled, updateOrder)
		{
			this.meshField = meshField;
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
			if (meshField == null)
			{
				meshField = new ReferenceField<Mesh>().refID;
			}
			_mesh = meshField.Resolve() as ReferenceField<Mesh>;
			Engine.OnCommonUpdate += OnUpdate;
			Slot ownerSlot = (Slot)owner.Resolve();
			collider = ownerSlot.gameObject.AddComponent<UnityEngine.MeshCollider>();
		}


		public override void OnDestroy()
		{
			meshField.Resolve().Destroy();
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
				this._mesh.target = refID;
				collider.sharedMesh = ((Mesh)refID.Resolve()).meshFilter.sharedMesh;
			}
		}

		public override void OnInit()
		{
			_mesh = meshField.Resolve() as ReferenceField<Mesh>;
			base.OnInit();
		}
	}
}