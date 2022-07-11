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
		public RefID meshField;
		[Newtonsoft.Json.JsonIgnore]
		private ReferenceField<Mesh> _mesh;

		public RefID materialField;
		[Newtonsoft.Json.JsonIgnore]
		private ReferenceField<PBS_Metallic> _material;
		
		public Core.BuiltInMaterial builtInMaterial;

		[Newtonsoft.Json.JsonIgnore]
		private UnityEngine.MeshRenderer renderer;
		[Newtonsoft.Json.JsonIgnore]
		private UnityEngine.MeshFilter meshFilter;

		public MeshRenderer(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public MeshRenderer(RefID refID, RefID meshField, RefID materialField, RefID owner, bool isEnabled, int updateOrder) : base(refID, owner, isEnabled, updateOrder)
		{
			this.meshField = meshField;
			this.materialField = materialField;
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
			if (meshField == null)
			{
				meshField = new ReferenceField<Mesh>().refID;
			}
			_mesh = (ReferenceField<Mesh>)meshField.Resolve();
			
			if (materialField == null)
			{
				materialField = new ReferenceField<PBS_Metallic>().refID;
			}
			
			_material = materialField.Resolve() as ReferenceField<PBS_Metallic>;
			Engine.OnCommonUpdate += OnUpdate;
			Slot ownerSlot = (Slot)owner.Resolve();
			renderer = ownerSlot.gameObject.AddComponent<UnityEngine.MeshRenderer>();
			meshFilter = ownerSlot.gameObject.AddComponent<UnityEngine.MeshFilter>();
		}

		public override void OnDestroy()
		{
			WorldManager.onWorldLoaded -= OnInit;
			meshField.Resolve().Destroy();
			materialField.Resolve().Destroy();
			UnityEngine.GameObject.Destroy(renderer);
			UnityEngine.GameObject.Destroy(meshFilter);
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
				_mesh.target = refID;
				Mesh mesh = (Mesh)refID.Resolve();
				meshFilter.mesh = mesh.meshFilter.mesh;
			}
		}

		public void SetMaterial(RefID refID)
		{
			if (refID.ResolveType() == typeof(PBS_Metallic))
			{
				_material.target = refID;
				PBS_Metallic material = (PBS_Metallic)refID.Resolve();
				renderer.material = material.material;
			}
		}
		
		public override void OnInit()
		{
			base.OnInit();
			SetMesh(_mesh.target);
			SetMaterial(_material.target);
		}
	}
}
