using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Core;
using KodEngine.Component;

namespace KodEngine.Component
{
	public class MeshRenderer : Core.Component
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

		private PBS_Metallic _material;
		public PBS_Metallic material
		{
			get
			{
				return _material;
			}
			set
			{
				_material = value;
				OnChange();
			}
		}
		public BuiltInMaterial builtInMaterial;
		private UnityEngine.MeshRenderer renderer;
		private UnityEngine.MeshFilter meshFilter;

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
			Engine.OnCommonUpdate += OnUpdate;
			renderer = owner.gameObject.AddComponent<UnityEngine.MeshRenderer>();
			meshFilter = owner.gameObject.AddComponent<UnityEngine.MeshFilter>();

			builtInMaterial = new BuiltInMaterial();
			builtInMaterial.owner = owner;
			builtInMaterial.OnAttach();
		}

		public override void OnDestroy()
		{
		}

		public override void OnUpdate()
		{
			if (renderer != null && material != null)
			{
				renderer.material = material.material;
				//Debug.Log(material.material);
			}
		}

		public override void OnChange()
		{
			if (mesh != null)
			{
				meshFilter.mesh = mesh.meshObject.GetComponent<UnityEngine.MeshFilter>().mesh;
			}

			if (material == null)
			{
				renderer.material = builtInMaterial.material;
			}
		}
	}
}
