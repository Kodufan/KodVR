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

		public override void OnAsleep()
		{
		}

		public override void OnAttach()
		{
			owner.owningWorld.OnFocusGained += OnAwake;
			owner.owningWorld.OnFocusLost += OnAsleep;
			Engine.OnCommonUpdate += OnUpdate;
			collider = owner.gameObject.AddComponent<UnityEngine.MeshCollider>();
		}

		public override void OnAwake()
		{
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