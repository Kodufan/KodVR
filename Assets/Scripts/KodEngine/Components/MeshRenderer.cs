using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Core;
using KodEngine.KodEBase;
using System.Reflection;

namespace KodEngine.Component
{
	public class MeshRenderer : Core.Component
	{
		public ReferenceField<Mesh> mesh; 
		public ReferenceField<PBS_Metallic> material;
		
		public Core.BuiltInMaterial builtInMaterial;
		private UnityEngine.MeshRenderer renderer;
		private UnityEngine.MeshFilter meshFilter;

		public MeshRenderer(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public MeshRenderer(RefID refID, RefID owner, bool isEnabled, int updateOrder) : base(refID, owner, isEnabled, updateOrder)
		{
		}

		public override string helpText
		{
			get
			{
				return "The MeshRenderer component will take in a mesh and a list of materials, rendering the mesh with said materials on the slot it has been attached to." +
					" This does not automatically assign colliders to the object. If you want your object to be anything besides a visual, please consider attaching colliders as well.";
			}
			set
			{
			}
		}

		public override void OnAttach()
		{
			mesh = new ReferenceField<Mesh>();
			material = new ReferenceField<PBS_Metallic>();
			Engine.OnCommonUpdate += OnUpdate;
			Slot ownerSlot = (Slot)owner.Resolve();
			renderer = ownerSlot.gameObject.AddComponent<UnityEngine.MeshRenderer>();
			meshFilter = ownerSlot.gameObject.AddComponent<UnityEngine.MeshFilter>();
		}

		public override void OnDestroy()
		{
			mesh.Destroy();
			material.Destroy();
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
				Mesh mesh = (Mesh)refID.Resolve();
				meshFilter.mesh = mesh.meshFilter.mesh;
			}
		}

		public void SetMaterial(RefID refID)
		{
			if (refID.ResolveType() == typeof(PBS_Metallic))
			{
				PBS_Metallic material = (PBS_Metallic)refID.Resolve();
				renderer.material = material.material;
			}
		}
	}
}
